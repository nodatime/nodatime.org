# This script is designed to be sourced from autobuildweb.sh.
# It allows the second phase of the script to be updated without
# refetching the file for the first phase.

# Build site and run smoke tests
(cd $output/nodatime.org/build; ./buildweb.sh)

echo $combined_commit > $output/nodatime.org/src/NodaTime.Web/bin/Release/netcoreapp2.2/publish/wwwroot/commit.txt
echo "Built at $(date -u -Iseconds)" > $output/nodatime.org/src/NodaTime.Web/bin/Release/netcoreapp2.2/publish/wwwroot/build.txt 

echo "Build and test successful. Pushing."

(cd $output/nodatime.org;
 cp build/deployment/Dockerfile src/NodaTime.Web/bin/Release/netcoreapp2.2/publish;
 gcloud.cmd container builds submit \
   --config=build/deployment/cloudbuild.yaml \
   --substitutions=TAG_NAME="$combined_commit" \
   src/NodaTime.Web/bin/Release/netcoreapp2.2/publish)

