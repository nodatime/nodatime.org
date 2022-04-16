@Title="Building and testing"

## Visual Studio (Windows)

Noda Time is developed on Visual Studio 2022. All editions of Visual
Studio 2022, including the community edition, should be able to
build Noda Time, so long as you also have the .NET SDK
installed. We periodically update the version of the .NET SDK
that we build with, typically to the latest Long Term Support (LTS)
version, but occasionally a more recent version in order to use the
latest C# features. See the [root global.json
file](https://github.com/nodatime/nodatime/blob/main/global.json)
to determine which version of the SDK is required, then [download
it](https://dotnet.microsoft.com/download) if necessary.

To fetch the source code from the main GitHub repository, you'll need a
[git][] client. You may also want a Git GUI, such as [SourceTree][].

[git]: https://git-scm.com/
[SourceTree]: https://www.sourcetreeapp.com/

### Fetching and building

To fetch the source code, just clone the GitHub repository:

```bat
> git clone https://github.com/nodatime/nodatime.git
```

To build everything under Visual Studio, simply open the `src\NodaTime.sln` file.

To build with just the .NET SDK, run

```bat
> dotnet build src/NodaTime.sln
```

To run the tests:

```bat
> dotnet test src/NodaTime.Test
```

### Other scripts

Bash scripts are used for more complex tasks such as updating TZDB.
Many contributors will never need to run these scripts,
but just occasionally they may be useful when investigating a CI
failure.

These should largely work from any bash environment, but the one
used by the maintainers is the version included with [Git for
Windows](https://git-scm.com/download/win), also sometimes known as
"git bash". Please [file an
issue](https://github.com/nodatime/nodatime/issues/new) if you need
to run a script but have trouble doing so.
