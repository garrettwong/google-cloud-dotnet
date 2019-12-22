#!/bin/bash

set -e
source ../toolversions.sh

install_docfx

build_api_docs() {
  log_build_action "Building docs for $1"
  local api=$1

  # Special case "root" where we don't need to generate the sources
  if [ "$api" == "root" ]
  then
    cp -r root output/root
    mkdir -p output/root/obj/api
  elif [[ ! -d "../apis/$api/docs" ]]
  then
    if [[ -d "../apis/$api" ]]
    then
      echo "No docs directory for $api; ignoring"
      return 0
    else
      echo "$api does not exist; aborting"
      return 1
    fi
  else
    dotnet run --no-build --no-restore -p ../tools/Google.Cloud.Tools.GenerateDocfxSources -- $api
  fi
  cp filterConfig.yml output/$api
  $DOCFX metadata --logLevel Warning -f output/$api/docfx.json | tee errors.txt | grep -v "Invalid file link"
  (! grep --quiet 'Build failed.' errors.txt)
  dotnet run --no-build --no-restore -p ../tools/Google.Cloud.Tools.GenerateSnippetMarkdown -- $api
  
  # Copy external dependency yml files into the API and concatenate toc.yml
  for dep in $(cat output/$api/dependencies)
  do
    cp dependencies/api/$dep/*.yml output/$api/obj/api
    cat dependencies/api/$dep/toc >> output/$api/obj/api/toc.yml
  done
  
  $DOCFX build --logLevel Warning output/$api/docfx.json | tee errors.txt | grep -v "Invalid file link"
  (! grep --quiet 'Build failed.' errors.txt)

  # Special case root: that should end up in the root of the assembled
  # site.
  if [ "$api" == "root" ]
  then
    # We don't want to end up with a "site" directory under assembled...
    # we want the contents of site directly under assembled.
    # There may well be a better way of doing this!
    cp -r -t output/assembled output/$api/site/*
  else
    cp -r output/$api/site output/assembled/$api
  fi
  echo Finished building docs for $api
}

if [[ ! -d "dependencies" ]]
then
  log_build_action "Fetching external dependencies repo"
  git clone https://github.com/googleapis/google-cloud-dotnet dependencies --quiet -b dependencies --depth=1
fi

rm -rf output
mkdir output
mkdir output/assembled

# Build the tools once, then we can run them without restoring/building each time
dotnet build ../tools/Google.Cloud.Tools.GenerateDocfxSources -v quiet -nologo -clp:NoSummary
dotnet build ../tools/Google.Cloud.Tools.GenerateSnippetMarkdown -v quiet -nologo -clp:NoSummary

apis=$@
if [ -z "$apis" ]
then
  # Build all APIs, which means every ../apis subdirectory with a "docs" subdirectory,
  # and "root" which is the special top-level docs.
  apis="`/usr/bin/find ../apis -mindepth 2 -maxdepth 2 -name docs -type d | cut -d/ -f3` root"
fi

log_build_action "(Start) Building docs"
for api in $apis
do 
  build_api_docs $api
done
log_build_action "(End) Building docs"
