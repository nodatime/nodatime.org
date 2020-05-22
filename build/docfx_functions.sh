# This is intended to be imported using the "source" function from
# any scripts that use tools.

declare -r BUILD_ROOT=$(realpath $(dirname ${BASH_SOURCE}))
declare -r DOCFX_VERSION=2.53.1

# Path to the version of docfx to use
declare -r DOCFX="$BUILD_ROOT/packages/docfx-$DOCFX_VERSION/docfx.exe"

# Function to install docfx if it's not already installed.
install_docfx() {
  if [[ ! -f $DOCFX ]]
  then
    (echo "Fetching docfx v${DOCFX_VERSION}";
     mkdir -p $BUILD_ROOT/packages;
     cd $BUILD_ROOT/packages;
     mkdir docfx-$DOCFX_VERSION;
     cd docfx-$DOCFX_VERSION;
     curl -sSL https://github.com/dotnet/docfx/releases/download/v${DOCFX_VERSION}/docfx.zip -o tmp.zip;
     unzip -q tmp.zip;
     rm tmp.zip)
  fi
}

# Parameters:
# - Root of target directory for API files (relative to current directory)
# - Source directory (relative to current directory)
# - Version
# - Target framework
# - Project names
generate_metadata() {
  target=$1
  source=$2
  version=$3
  framework=$4
  # Due to the confusion of Windows directories vs Unix directories,
  # it's relatively painful to create a genuine tmp file. Let's just
  # call it tmpdocfx.json and try to avoid accidentally committing it...
  docfxjson=tmpdocfx.json
  shift 4
  echo '{ "metadata": [' > $docfxjson
  for package in $*;
  do
    cat >> $docfxjson<<EOF
    {
      "src": [
        {
          "files": [ "$package/$package.csproj" ],
          "cwd": "$source"
        }
      ],
      "dest": "$target/$package/$version/api",
      "shouldSkipMarkup": true,
      "properties": {
        "TargetFramework": "$framework"
      }
    },
EOF
  done
  echo ']}' >> $docfxjson
  "$DOCFX" metadata --disableGitFeatures --logLevel Warning -f $docfxjson
  rm $docfxjson
}
