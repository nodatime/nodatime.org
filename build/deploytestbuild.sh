#!/bin/bash

set -e

ROOT=$(git rev-parse --show-toplevel)
(cd $ROOT/src/NodaTime.Web;
 declare -r PUBLISH=$ROOT/src/NodaTime.Web/bin/Debug/net6.0/publish;
 rm -rf $PUBLISH;
 dotnet publish -c Debug;
 
 echo "Built at $(date -u -Iseconds)" > $PUBLISH/wwwroot/build.txt;
 cp $ROOT/build/deployment/Dockerfile $PUBLISH;
 gcloud.cmd builds submit --config=$ROOT/build/deployment/testcloudbuild.yaml $PUBLISH)
