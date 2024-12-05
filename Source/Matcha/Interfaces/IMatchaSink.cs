#region License Information (MIT)
// Matcha - A simple but neat logging library for .NET.
// Copyright (C) 2022 Analog Feelings
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in all
// copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
// SOFTWARE.
#endregion

using AnalogFeelings.Matcha.Models;

namespace AnalogFeelings.Matcha.Interfaces;

/// <summary>
/// An interface to implement a Matcha logging sink.
/// </summary>
/// <remarks>
/// Sinks may implement <see cref="IDisposable"/>, Matcha will
/// dispose them automatically.
/// </remarks>
public interface IMatchaSink<out T> where T : SinkConfig
{
    /// <summary>
    /// The sink's configuration.
    /// </summary>
    public T Config { get; }

    /// <summary>
    /// Called when a sink is being initialized.
    /// </summary>
    public void InitializeSink();

    /// <summary>
    /// Called when a log entry is ready to be written to the sink.
    /// </summary>
    /// <param name="entry">The target log entry.</param>
    public Task WriteLogAsync(LogEntry entry);
}