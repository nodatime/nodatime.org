#!/bin/bash

rm -rf tmp/snippet_test
mkdir -p tmp/snippet_test

dotnet publish ../../nodatime/src/NodaTime.Demo
dotnet run -p SnippetExtractor -- ../../nodatime/src/NodaTime-All.sln NodaTime.Demo tmp/snippet_test
