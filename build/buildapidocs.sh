#!/bin/bash

set -e

source docfx_functions.sh
install_docfx

if [[ ! -d history ]]
then
  # FIXME: This should probably be in nodatime.org
  echo "Cloning history branch"
  git clone https://github.com/nodatime/nodatime.git -q --depth 1 -b history history
fi

rm -rf tmp/docfx

echo "Copying metadata for previous versions"
for version in 1.0.x 1.1.x 1.2.x 1.3.x 1.4.x 2.0.x 2.1.x 2.2.x 2.3.x 2.4.x; do
  mkdir -p tmp/docfx/obj/$version
  cp -r history/$version/api tmp/docfx/obj/$version
  if [[ -d history/$version/overwrite ]]
  then
    cp -r history/$version/overwrite tmp/docfx/obj/$version
  fi
  cp docfx/toc.yml tmp/docfx/obj/$version
done

echo "Building metadata for current branch"

# Main source code
mkdir tmp/docfx/unstable
# Note: this avoids copying the .git directory
cp -r ../../nodatime/* tmp/docfx/unstable
dotnet build tmp/docfx/unstable/src/NodaTime.sln
 
# Serialization
mkdir -p tmp/docfx/serialization
cp -r ../../nodatime.serialization/* tmp/docfx/serialization
dotnet build tmp/docfx/serialization/src/NodaTime.Serialization.sln

# Metadata build for main source code and serialization
cp -r docfx/template tmp/docfx
cp docfx/docfx-unstable.json tmp/docfx/docfx.json
"$DOCFX" metadata tmp/docfx/docfx.json -f --warningsAsErrors

cp docfx/toc.yml tmp/docfx/obj/unstable
cp docfx/toc.yml tmp/docfx/obj/serialization

echo "Building all tools"
dotnet build Tools.sln

# Awooga! Awooga! Horrible hack! docfx doesn't support C# 8 yet, and refers to nullable
# references types as if they were nullable value types. Fix this up in a purely textual way for now.
dotnet run -p DocfxNullableReferenceFixer -- --fix tmp/docfx/obj/unstable/api

# Create diffs between versions and other annotations

dotnet run -p ReleaseDiffGenerator -- \
  tmp/docfx/obj/1.0.x \
  tmp/docfx/obj/1.1.x \
  tmp/docfx/obj/1.2.x \
  tmp/docfx/obj/1.3.x \
  tmp/docfx/obj/1.4.x \
  tmp/docfx/obj/2.0.x \
  tmp/docfx/obj/2.1.x \
  tmp/docfx/obj/2.2.x \
  tmp/docfx/obj/2.3.x \
  tmp/docfx/obj/2.4.x \
  tmp/docfx/obj/unstable

# Extract annotations
dotnet run -p DocfxAnnotationGenerator -- \
    tmp/docfx history/packages tmp/docfx/unstable/src 1.0.x 1.1.x 1.2.x 1.3.x 1.4.x 2.0.x 2.1.x 2.2.x 2.3.x 2.4.x unstable

# Extract snippets from NodaTime.Demo (unstable only, for now)
dotnet publish tmp/docfx/unstable/src/NodaTime.Demo
dotnet run -p SnippetExtractor -- tmp/docfx/unstable/src/NodaTime.sln NodaTime.Demo tmp/docfx/obj/unstable/overwrite

echo "Running main docfx build"
"$DOCFX" build tmp/docfx/docfx.json
cp docfx/logo.svg tmp/docfx/_site
