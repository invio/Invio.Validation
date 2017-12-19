#!/usr/bin/env bash

#exit if any command fails
set -e

artifactsFolder="./artifacts"

if [ -d $artifactsFolder ]; then
  rm -R $artifactsFolder
fi

dotnet restore

dotnet test ./test/Invio.Validation.Tests/Invio.Validation.Tests.csproj -c Release

dotnet pack ./src/Invio.Validation -c Release -o ../../artifacts
