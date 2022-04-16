#!/bin/bash

# This script populates the history directory from scratch.
# The directory is stored as an orphan branch called 'history'
# in the https://github.com/nodatime/nodatime.org repo.
#
# The aim is that scripts such as buildapi.sh can just
# fetch the branch (with a depth of 1) and use it as-is.
# This script should only need to be rerun when we've made
# significant changes, e.g. releasing a new major or minor
# version, or changing the build scripts.

# Steps:
# 1. Blow away any current history directory
# 2. Checkout the current history branch from github
# 3. Populate
# 4. Commit

# Note that this does *not* push the branch back up to github,
# but after making sure that it all looks okay, that would
# normally be the next step.

# Contents:

# Top-level: one directory for each package (including NodaTime and NodaTime.Testing separated out)

# Within each package directory:
# - Released versions (1.0.x, 2.0.x etc)

# Within each version directory:
# - nupkg file for all but unstable
# - api directory with docfx output
# - (Where appropriate) overwrite directory with snippets

# - 1.0.x / 1.1.x / 1.2.x / 1.3.x / 1.4.x
#   - Docfx metadata in "api" directory
# - 2.0.x, 2.1.x, 2.2.x, 2.3.x, 2.4.x, 3.0.x
#   - Docfx metadata in "api" directory
#   - Docfx snippets for 2.1 onwards
# - packages
#   - nupkg files for each minor version, e.g NodaTime-1.0.x.nupkg

set -e

fetch_packages() {
  declare -r package=$1  
  shift
  while (( "$#" ))
  do
    VERSION=$1
    curl -sSL -o $package/$VERSION.x/$package-$VERSION.0.nupkg https://www.nuget.org/api/v2/package/$package/$VERSION.0
    shift
  done
}

source docfx_functions.sh
install_docfx

echo "Removing old history directory"
rm -rf history

echo "Cloning current history branch"
git clone https://github.com/nodatime/nodatime.org.git -q -b history history

cd history

echo "Cleaning current directory"
git rm -qrf .
git clean -qdf

echo "Building snippet extractor"
dotnet build ../SnippetExtractor

# Output directories
mkdir NodaTime
mkdir NodaTime.Testing
mkdir NodaTime.Serialization.JsonNet
mkdir NodaTime.Serialization.Protobuf
mkdir NodaTime.Serialization.SystemTextJson

# Do lots of building in a temporary directory, then just copy what we actually need
mkdir main
cd main

for version in 1.0.x 1.1.x 1.2.x 1.3.x 1.4.x 2.0.x 2.1.x 2.2.x 2.3.x 2.4.x 3.0.x
do
  echo "Cloning $version"
  git clone -q https://github.com/nodatime/nodatime.git --depth 1 -b $version $version
done

# 1.x versions have different sets of packages, as sometimes there's serialization
# and sometimes not.
for version in 1.0.x 1.1.x 1.2.x 1.3.x 1.4.x
do
  echo "Building docfx metadata for $version"
  if [[ $version != "1.0.x" && $version != "1.1.x" ]]
  then
    generate_metadata .. $version/src $version net45 NodaTime NodaTime.Testing NodaTime.Serialization.JsonNet
  else
    generate_metadata .. $version/src $version net45 NodaTime NodaTime.Testing
  fi
done

# 2.x+ versions need to be restored first, but all have the same set of packages.
for version in 2.0.x 2.1.x 2.2.x 2.3.x 2.4.x 3.0.x
do
  [[ $version == 2* ]] && target_framework="net45" || target_framework="netstandard2.0"
  echo "Building docfx metadata for $version using target framework $target_framework"
  dotnet restore -v quiet $version/src/NodaTime
  dotnet restore -v quiet $version/src/NodaTime.Testing
  generate_metadata .. $version/src $version $target_framework NodaTime NodaTime.Testing
done

# Snippets
# Note that we skip 2.1-2.3, as we can't build those easily now.
# We build using the main SnippetExtractor, so that as things change
# we only need to change that rather than all branches
for version in 2.4.x 3.0.x
do
  echo "Generating snippets for $version"
  [[ $version == 2* ]] && solution_file="NodaTime-All.sln" || solution_file="NodaTime.sln"
  (cd $version;
   dotnet publish src/NodaTime.Demo;
   mkdir ../../NodaTime/$version/overwrite;
   dotnet run --no-build --project ../../../SnippetExtractor -- src/$solution_file NodaTime.Demo ../../NodaTime/$version/overwrite)
done
cd ..

# Clone the whole serialization repo, so we can checkout specific tags.
git clone -q https://github.com/nodatime/nodatime.serialization.git serialization
# Build JsonNet versions
cd serialization
for version in 2.0.x 2.1.x 2.2.x
do
  git checkout -q NodaTime.Serialization.JsonNet-$(echo $version | sed s/x/0/g)
  dotnet restore -v quiet src/NodaTime.Serialization.JsonNet
  generate_metadata .. src $version net45 NodaTime.Serialization.JsonNet
done

# Build protobuf versions
for version in 1.0.x
do
  git checkout -q NodaTime.Serialization.Protobuf-$(echo $version | sed s/x/0/g)
  dotnet restore -v quiet src/NodaTime.Serialization.Protobuf
  generate_metadata .. src $version netstandard2.0 NodaTime.Serialization.Protobuf
done

# Build SystemTextJson
git checkout -q NodaTime.Serialization.SystemTextJson-1.0.0-beta01
dotnet restore -v quiet src/NodaTime.Serialization.SystemTextJson
generate_metadata .. src $version netstandard2.0 NodaTime.Serialization.SystemTextJson

cd ..

# Fetch all the nuget packages
echo "Fetching nuget packages"
fetch_packages NodaTime 1.0 1.1 1.2 1.3 1.4 2.0 2.1 2.2 2.3 2.4 3.0
fetch_packages NodaTime.Testing 1.0 1.1 1.2 1.3 1.4 2.0 2.1 2.2 2.3 2.4 3.0
fetch_packages NodaTime.Serialization.JsonNet 1.2 1.3 1.4 2.0 2.1 2.2
fetch_packages NodaTime.Serialization.Protobuf 1.0
# fetch_packages doesn't support betas...
curl -sSL -o NodaTime.Serialization.SystemTextJson/1.0.x/NodaTime.Serialization.SystemTextJson-1.0.0-beta01.nupkg https://www.nuget.org/api/v2/package/NodaTime.Serialization.SystemTextJson/1.0.0-beta01

echo "Generating xref maps"
mkdir xrefs
dotnet run --project ../SandcastleXrefGenerator -- \
    Newtonsoft.Json 12.0.1 netstandard2.0 \
    https://www.newtonsoft.com/json/help/html/ \
    > xrefs/Newtonsoft.Json-xrefmap.yml

# Clean up
rm -rf main
rm -rf serialization

cat > README.md <<"End-of-readme"
The contents of this branch are generated by the `build/buildhistory.sh`
script in the main branch. All commits to this branch should
originate from that script. No hand edits!
End-of-readme

# Just tell Travis not to build this branch. We don't
# care what this branch says about other branches.
cat > .travis.yml<<"End-of-travis"
branches:
  except:
  - history
End-of-travis

# Preserve previous snippets for 2.1-2.3
git checkout HEAD -- NodaTime/2.1.x/overwrite/snippets.md
git checkout HEAD -- NodaTime/2.2.x/overwrite/snippets.md
git checkout HEAD -- NodaTime/2.3.x/overwrite/snippets.md


git add --all
git commit -q -m "Regenerated history directory"

cd ..
echo "Done. Check for errors, then push to github"
