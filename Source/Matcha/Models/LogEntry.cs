#region License Information (MIT)
// Matcha - A simple but neat logging library for .NET.
// Copyright (C) 2022-2024 Analog Feelings
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

using Matcha.Enums;

namespace Matcha.Models;

/// <summary>
/// A structure containing a Matcha log entry and its related data.
/// </summary>
public record LogEntry
{
    /// <summary>
    /// The log entry's severity.
    /// </summary>
    public required LogSeverity Severity { get; init; }
    
    /// <summary>
    /// The log entry's message.
    /// </summary>
    public required string Content { get; init; }
    
    /// <summary>
    /// The time the log entry was created at.
    /// </summary>
    public required DateTime Time { get; init; }
    
    /// <summary>
    /// The source method of the log entry.
    /// </summary>
    public required string Source { get; init; }
    
    /// <summary>
    /// The formatting parameters.
    /// </summary>
    public object[]? Format { get; init; }
}