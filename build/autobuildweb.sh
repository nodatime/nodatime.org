#!/bin/bash

# This script is designed to be run on a scheduled basis, to build and deploy the web site
# in an automated manner. It takes the following steps, with input as a
# "web site build root directory" (henceforth known as $root).
#
# - Find the latest commit for the main repo
# - If $root/$commit exists, assume there have been no changes, and abort
# - Make a shallow clone (--depth 1) of the main repo to $root/$commit/nodatime
# - Run the fetched finishautobuildweb.sh to complete the remainder of the build
#
# It is anticipated that this file will not need to change often; it's effectively bootstrapping.

set -e

if [[ "$1" = "" ]]
then
  echo "Usage: autobuildweb.sh output-directory"
  echo "e.g. autobuildweb.sh c:\\users\\jon\\NodaTimeWeb"
  exit 1
fi

declare -r root=$1
declare -r nodatime_commit=$(curl -s -H Accept:application/vnd.github.VERSION.sha https://api.github.com/repos/nodatime/nodatime/commits/main)
declare -r nodatime_org_commit=$(curl -s -H Accept:application/vnd.github.VERSION.sha https://api.github.com/repos/nodatime/nodatime.org/commits/main)
declare -r nodatime_serialization_commit=$(curl -s -H Accept:application/vnd.github.VERSION.sha https://api.github.com/repos/nodatime/nodatime.serialization/commits/main)

declare -r combined_commit="${nodatime_commit:0:8}_${nodatime_org_commit:0:8}_${nodatime_serialization_commit:0:8}"
declare -r output=$root/$combined_commit

if [[ -d $output ]]
then
  echo "Directory $output already exists. Aborting."
  exit 0
fi

# Clone repos
# Note that we don't explicitly check that we've got the right commit, so there's a theoretical
# race condition, but it's very unlikely to cause a problem
git clone https://github.com/nodatime/nodatime.git --depth 1 $output/nodatime
git clone https://github.com/nodatime/nodatime.org.git --depth 1 $output/nodatime.org
git clone https://github.com/nodatime/nodatime.serialization.git --depth 1 $output/nodatime.serialization

# Hand off to the second part of the build
source $output/nodatime.org/build/finishautobuildweb.sh &> $output/nodatime.org/build/buildweb.log
