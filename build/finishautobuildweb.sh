# This script is designed to be sourced from autobuildweb.sh.
# It allows the second phase of the script to be updated without
# refetching the file for the first phase.

echo "Environment:"
env
echo ""

# Build site and run smoke tests
(cd $output/nodatime.org/build; ./buildweb.sh)

declare -r publish=$output/nodatime.org/src/NodaTime.Web/bin/Release/net6.0/publish

# Add diagnostic text files
echo "Combined: $combined_commit" > $publish/wwwroot/commit.txt
echo "nodatime: $nodatime_commit" >> $publish/wwwroot/commit.txt
echo "nodatime.org: $nodatime_org_commit" >> $publish/wwwroot/commit.txt
echo "nodatime.serialization: $nodatime_serialization_commit" >> $publish/wwwroot/commit.txt
echo "Built at $(date -u -Iseconds)" > $publish/wwwroot/build.txt 

echo "Build and test successful. Pushing."

(cd $output/nodatime.org;
 cp build/deployment/Dockerfile $publish;
 gcloud.cmd container builds submit \
   --config=build/deployment/cloudbuild.yaml \
   --substitutions=TAG_NAME="$combined_commit" \
   $publish)

