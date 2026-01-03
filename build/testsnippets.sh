#!/bin/bash

rm -rf tmp/snippet_test
mkdir -p tmp/snippet_test

dotnet publish -c Debug ../../nodatime/src/NodaTime.Demo
dotnet run --project SnippetExtractor -- ../../nodatime/src/NodaTime.slnx NodaTime.Demo tmp/snippet_test
