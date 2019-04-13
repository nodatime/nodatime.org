#!/bin/bash

set -e

ROOT=$(git rev-parse --show-toplevel)
(cd $ROOT/src/NodaTime.Web;
 dotnet publish -c Debug;
 declare -r PUBLISH=$ROOT/src/NodaTime.Web/bin/Debug/netcoreapp2.2/publish
 
 # Remove the Windows and OSX gRPC binaries; replace the unused Linux one
 # with an empty file. (It has to be present, but can be empty.)
 rm -rf $PUBLISH/runtimes/{osx,win}
 > $PUBLISH/runtimes/linux/native/libgrpc_csharp_ext.x86.so
 
 echo "Built at $(date -u -Iseconds)" > $PUBLISH/wwwroot/build.txt;
 cp $ROOT/build/deployment/Dockerfile $PUBLISH;
 gcloud.cmd builds submit --config=$ROOT/build/deployment/testcloudbuild.yaml $PUBLISH)
