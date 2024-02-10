# This is intended to be imported using the "source" function from
# any scripts that use tools.

declare -r BUILD_ROOT=$(realpath $(dirname ${BASH_SOURCE}))

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
  # it's relatively painful to create a genuine tmp file.
  docfxjson=tmpdocfx.json
  shift 4
  echo '{ "metadata": [' > $docfxjson
  for package in $*
  do
    cat >> $docfxjson<<EOF
    {
      "src": [
        {
          "files": [ "$package/$package.csproj" ],
          "src": "$source"
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
  dotnet docfx metadata --disableGitFeatures --logLevel Warning $docfxjson
  rm $docfxjson
}
