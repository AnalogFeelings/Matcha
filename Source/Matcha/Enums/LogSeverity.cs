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

namespace AnalogFeelings.Matcha.Enums;

/// <summary>
/// Denotes different log message severities.
/// </summary>
public enum LogSeverity
{
    /// <summary>
    /// Used to log debug data for developers.
    /// </summary>
    /// <remarks>
    /// This severity should be filtered on release builds.
    /// </remarks>
    Debug,

    /// <summary>
    /// Used to log non-urgent information about the program's execution.
    /// </summary>
    Information,

    /// <summary>
    /// Used to log a successful operation's completion.
    /// </summary>
    Success,

    /// <summary>
    /// Used to log a potentially dangerous situation.
    /// </summary>
    Warning,

    /// <summary>
    /// Used to log an error in the program's execution.
    /// </summary>
    Error,

    /// <summary>
    /// Used to log a catastrophic error which cannot be recovered from.
    /// </summary>
    Fatal,
}