# 🍵 Matcha

[![GitHub issues](https://img.shields.io/github/issues/analogfeelings/matcha?label=Issues&style=flat-square)](https://github.com/AnalogFeelings/Matcha/issues)
[![GitHub Issues or Pull Requests](https://img.shields.io/github/issues-pr/analogfeelings/matcha?style=flat-square&label=Pull%20Requests)](https://github.com/AnalogFeelings/Matcha/pulls)
[![GitHub Repo stars](https://img.shields.io/github/stars/analogfeelings/matcha?label=Stargazers&style=flat-square)](https://github.com/AnalogFeelings/Matcha/stargazers)
[![GitHub](https://img.shields.io/github/license/analogfeelings/matcha?label=License&style=flat-square)](https://github.com/AnalogFeelings/Matcha/blob/neo/LICENSE.txt)
[![GitHub commit activity](https://img.shields.io/github/commit-activity/w/analogfeelings/matcha?label=Commit%20Activity&style=flat-square)](https://github.com/AnalogFeelings/Matcha/graphs/commit-activity)
[![Mastodon Follow](https://img.shields.io/mastodon/follow/109309123442839534?domain=https%3A%2F%2Ftech.lgbt%2F&style=flat-square&logo=mastodon&logoColor=white&label=Follow%20Me!&color=6364ff)](https://tech.lgbt/@analog_feelings)

A simple but neat logging library for .NET 7.0 and higher. It includes XML documentation!

The library is built with extensibility in mind, by adding a "sink" system where each `Log` call is sent to all sinks for processing instead of centralizing
everything into the main logger class.

It comes with 3 default logging sinks:
- Console
- Debugger
- File

**Important!**
All of the built-in sinks are thread-safe, but the library doesn't enforce this. Use third party sinks with caution!

## 🤔 Usage
Create the needed sinks and their configurations and pass them to the constructor to `MatchaLogger`.  
Adding or removing sinks at runtime is not allowed, but this may change in a future release.

You may change a sink's configuration at runtime by keeping a reference to it.  
Matcha will automatically dispose any sinks that implement `IDisposable` when Matcha itself is disposed.

## 🖌️ Custom Sinks
Creating a custom sink requires you to implement `IMatchaSink<out T>`, where `T` may be any
class, record or struct that inherits from `SinkConfig`.

`WriteLogAsync` may be marked as async, and the sink can optionally also implement `IDisposable` if it
has resources that need to be disposed.

**Tip!**
Implement a `SemaphoreSlim` for thread safety, and release it inside a `finally` block.
This will prevent deadlocks if an error occurs during logging.

## 📥 Downloads
You can find this package in [NuGet](https://www.nuget.org/packages/MatchaLogger/).

Alternatively, you could download it from the [releases](https://github.com/AnalogFeelings/Matcha/releases/latest) page.

# ⚖️ License
Licensed under the MIT License. You can read it [here](LICENSE.txt).
 
```
THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
SOFTWARE.
```
