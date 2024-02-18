# This script is designed to be sourced from autobuildweb.sh.
# It allows the second phase of the script to be updated without
# refetching the file for the first phase.

echo "Environment:"
env
echo ""

# Build site and run smoke tests
(cd $output/nodatime.org/build;
 ./buildweb.sh;
 dotnet test ../src/NodaTime.Web.SmokeTest)

echo "Build and test successful. Pushing."

(cd $output/nodatime.org;
 cp build/deployment/Dockerfile src/NodaTime.Web/bin/Release/net8.0/publish;
 gcloud.cmd container builds submit \
   --config=build/deployment/cloudbuild.yaml \
   --substitutions=TAG_NAME="$combined_commit" \
   $publish)
