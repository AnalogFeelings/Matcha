![Matcha](Matcha.png)
# Matcha

![GitHub](https://img.shields.io/github/license/aestheticalz/matcha?label=License&style=flat-square)
![Lines of code](https://img.shields.io/tokei/lines/github/aestheticalz/matcha?label=Total%20Lines&style=flat-square)
![GitHub issues](https://img.shields.io/github/issues/aestheticalz/matcha?label=Issues&style=flat-square)
![GitHub Repo stars](https://img.shields.io/github/stars/aestheticalz/matcha?label=Stars&style=flat-square)
![GitHub commit activity](https://img.shields.io/github/commit-activity/w/aestheticalz/matcha?label=Commit%20Activity&style=flat-square)
![Nuget](https://img.shields.io/nuget/v/MatchaLogger?label=NuGet&style=flat-square)

A simple but neat logging library for .NET 5.0. Includes XML documentation!

## Usage
Create an instance of the `MatchaLoggerSettings` and intitialize it's members before passing it to the constructor of `MatchaLogger`. `MatchaLogger` implements `IDisposable`, so whenever you want to close the logger and dispose the log file stream, call the `Dispose()` method.

## License
Licensed under the MIT License.
