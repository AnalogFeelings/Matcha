![Matcha](https://user-images.githubusercontent.com/51166756/167296398-592d7c48-adc2-489c-8e96-5ca4944fb192.png)
# Matcha

![GitHub](https://img.shields.io/github/license/analogfeelings/matcha?label=License&style=flat-square)
![Lines of code](https://img.shields.io/tokei/lines/github/analogfeelings/matcha?label=Total%20Lines&style=flat-square)
![GitHub issues](https://img.shields.io/github/issues/analogfeelings/matcha?label=Issues&style=flat-square)
![GitHub Repo stars](https://img.shields.io/github/stars/analogfeelings/matcha?label=Stars&style=flat-square)
![GitHub commit activity](https://img.shields.io/github/commit-activity/w/analogfeelings/matcha?label=Commit%20Activity&style=flat-square)
![Nuget](https://img.shields.io/nuget/v/MatchaLogger?label=NuGet&style=flat-square)

A simple but neat logging library for .NET Standard 2.0. Includes XML documentation!

## Usage
Create an instance of the `MatchaLoggerSettings` and intitialize it's members before passing it to the constructor of `MatchaLogger`. `MatchaLogger` implements `IDisposable`, so whenever you want to close the logger and dispose the log file stream, call the `Dispose()` method.

## Downloads
Find this package in [NuGet](https://www.nuget.org/packages/MatchaLogger/).

## License
Licensed under the MIT License.
