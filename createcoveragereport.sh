#!/bin/bash

set -e
source toolversions.sh

if [ ! -d coverage ]
then
 mkdir coverage
fi

rm -f coverage/all.dvcr

if ! compgen -G "coverage/*.dvcr" > /dev/null
then
  echo "No coverage reports found to merge."
  # This isn't an error
  exit 0
fi

install_dotcover
install_reportgenerator

codecov_params=
upload_report=false
while (( "$#" )); do
  if [[ "$1" == "--upload" ]]
  then
    upload_report=true
  elif [[ "$1" == "--upload_reportname" ]]
  then
    shift
    codecov_params="$codecov_params --flag $1"
  elif [[ "$1" == "--upload_commit" ]]
  then
    shift
    codecov_params="$codecov_params --c $1"
  elif [[ "$1" == "--upload_build" ]]
  then
    shift
    codecov_params="$codecov_params --b $1"
  else
    echo "Unexpected param: $1"
    exit 1
  fi
  shift
done

echo "Merging reports..."
$DOTCOVER merge --Source="$(echo coverage/*.dvcr | sed 's/ /;/g')" --Output=coverage/all.dvcr ""

echo "Generating detailed xml report..."
$DOTCOVER report --Source=coverage/all.dvcr --Output=coverage/coverage.xml --ReportType=DetailedXML ""

# We assume the tools solution has already been restored as part of the build
echo "Filtering xml report..."
dotnet run -p tools/Google.Cloud.Tools.TrimCoverageReport/Google.Cloud.Tools.TrimCoverageReport.csproj -- coverage/coverage.xml coverage/coverage-filtered.xml

echo "Running ReportGenerator to create an html report..."

$REPORTGENERATOR \
   -reports:coverage/coverage-filtered.xml \
   -targetdir:coverage/report \
   -verbosity:Error

if [[ "$upload_report" = true ]]
then
  # -y option to confirm all prompts.
  # --no-progress to avoid our log file being spammed with download progress
  choco install codecov -y --no-progress

  # Assume we've created the coverage file by this point. If we haven't, there should already have been an error.
  # Pass whatever parameters we recieved.
  codecov -f "coverage/coverage-filtered.xml" $codecov_params
fi
