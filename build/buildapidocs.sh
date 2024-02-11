#!/bin/bash

set -e

source docfx_functions.sh
dotnet tool restore > /dev/null

copy_metadata() {
  version=$1
  target=$2
  shift 2
  mkdir -p tmp/web/$target/api
  mkdir -p tmp/web/$target/overwrite
  cp docfx/toc.yml tmp/web/$target

  for package in $*
  do
    cp tmp/metadata/$package/$version/api/* tmp/web/$target/api
    if [ -d tmp/metadata/$package/$version/overwrite ]
    then
      cp tmp/metadata/$package/$version/overwrite/* tmp/web/$target/overwrite
    fi
  done

  # Combine TOCs
  tocs=()
  for package in $*
  do
    tocs+="tmp/metadata/$package/$version/api/toc.yml "
  done
  dotnet run --project TocCombiner -- ${tocs[*]} > tmp/web/$target/api/toc.yml
}

if [[ ! -d history ]]
then
  echo "Cloning history branch"
  git clone https://github.com/nodatime/nodatime.org.git -q --depth 1 -b history history
fi

rm -rf tmp
mkdir -p tmp/metadata

echo "Copying metadata for previous versions"
cp -r history/Noda* tmp/metadata

echo "Building packages and metadata for local code (may not be committed)"
dotnet pack --nologo -v quiet ../../nodatime/src/NodaTime -o $PWD/tmp/metadata/NodaTime/unstable
dotnet pack --nologo -v quiet ../../nodatime/src/NodaTime.Testing -o $PWD/tmp/metadata/NodaTime.Testing/unstable
dotnet pack --nologo -v quiet ../../nodatime.serialization/src/NodaTime.Serialization.JsonNet -o $PWD/tmp/metadata/NodaTime.Serialization.JsonNet/unstable
dotnet pack --nologo -v quiet ../../nodatime.serialization/src/NodaTime.Serialization.Protobuf -o $PWD/tmp/metadata/NodaTime.Serialization.Protobuf/unstable
dotnet pack --nologo -v quiet ../../nodatime.serialization/src/NodaTime.Serialization.SystemTextJson -o $PWD/tmp/metadata/NodaTime.Serialization.SystemTextJson/unstable
generate_metadata tmp/metadata ../../nodatime/src unstable net8.0 NodaTime
generate_metadata tmp/metadata ../../nodatime.serialization/src unstable netstandard2.0 NodaTime.Serialization.JsonNet NodaTime.Serialization.Protobuf NodaTime.Serialization.SystemTextJson

# NodaTime.Testing is awkward: the project dependency means
# the NodaTime csproj needs to be included, but we want to exclude its APIs.
# We have a custom docfx.json for this, then we need to munge the toc etc.
# (docfx filters don't work well here)
echo "Building metadata for NodaTime.Testing"
dotnet docfx metadata --disableGitFeatures --logLevel Warning testing-docfx.json
rm -rf tmp/metadata/NodaTime.Testing/unstable/api
mkdir tmp/metadata/NodaTime.Testing/unstable/api
# Sort out the TOC
dotnet run --project TestingTocPruner \
  tmp/metadata/NodaTime.Testing/unstable/fullapi/toc.yml \
  tmp/metadata/NodaTime.Testing/unstable/api/toc.yml
# Sort out the manifest
echo "{" > tmp/metadata/NodaTime.Testing/unstable/api/.manifest
grep NodaTime.Testing tmp/metadata/NodaTime.Testing/unstable/fullapi/.manifest >> tmp/metadata/NodaTime.Testing/unstable/api/.manifest
# Remove the last character (the trailing comma)
sed -i '$ s/.$//' tmp/metadata/NodaTime.Testing/unstable/api/.manifest
echo "}" >> tmp/metadata/NodaTime.Testing/unstable/api/.manifest
# Copy just the relevant files
cp tmp/metadata/NodaTime.Testing/unstable/fullapi/NodaTime.Testing.* tmp/metadata/NodaTime.Testing/unstable/api
# Tidy up
rm -rf tmp/metadata/NodaTime.Testing/unstable/fullapi

echo "Building all tools"
dotnet build -v quiet Tools.sln

# Create diffs between versions and other annotations, just for NodaTime
dotnet run --project ReleaseDiffGenerator -- tmp/metadata/NodaTime

# Extract annotations
dotnet run --project DocfxAnnotationGenerator -- tmp/metadata/*

# Extract snippets from NodaTime.Demo (unstable only, for now)
dotnet publish -c Debug -v quiet ../../nodatime/src/NodaTime.Demo
dotnet run --project SnippetExtractor -- ../../nodatime/src/NodaTime.sln NodaTime.Demo tmp/metadata/NodaTime/unstable/overwrite

# Reorganize the files to suit docfx build
# NodaTime and NodaTime.Testing
for dir in tmp/metadata/NodaTime/*
do
  version=$(basename $dir)
  copy_metadata $version $version NodaTime NodaTime.Testing
done

copy_metadata unstable serialization NodaTime.Serialization.JsonNet NodaTime.Serialization.Protobuf NodaTime.Serialization.SystemTextJson
cp docfx/serialization-toc.yml tmp/web/serialization/toc.yml

# Put common overwrite files where they can be used by all versions
mkdir -p tmp/web/commonoverwrite
cp docfx/namespaces.md tmp/web/commonoverwrite

# Copy local xref maps and templates
cp -r history/xrefs tmp
cp -r docfx/template tmp


# Run the main build for everything apart from serialization
for dir in tmp/metadata/NodaTime/*
do
  version=$(basename $dir)
  sed "s/VERSION/$version/g" docfx/docfx-web-template.json > tmp/docfx-web-$version.json
  echo "Running main docfx build for $version"
  dotnet docfx build --disableGitFeatures --logLevel Warning tmp/docfx-web-$version.json
  mv tmp/site/xrefmap.yml tmp/site/$version.xrefmap.yml
done

# Run the main build for serialization
cp docfx/docfx-web-serialization.json tmp
dotnet docfx build --disableGitFeatures --logLevel Warning tmp/docfx-web-serialization.json
mv tmp/site/xrefmap.yml tmp/site/serialization.xrefmap.yml

cp docfx/logo.svg tmp/site
