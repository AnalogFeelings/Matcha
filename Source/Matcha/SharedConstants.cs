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

using AnalogFeelings.Matcha.Enums;

namespace AnalogFeelings.Matcha;

/// <summary>
/// Contains shared constants used by the built-in sinks.
/// </summary>
internal static class SharedConstants
{
    /// <summary>
    /// Box drawing character in an "L" shape.
    /// </summary>
    public const char BOX_UPRIGHT = '\u2514';
    
    /// <summary>
    /// Box drawing character in a sideways "T" shape.
    /// </summary>
    public const char BOX_VERTRIGHT = '\u251c';
    
    /// <summary>
    /// Box drawing character in a "-" shape.
    /// </summary>
    public const char BOX_HORIZONTAL = '\u2500';
    
    /// <summary>
    /// Dictionary to quickly convert a severity to its string representation.
    /// </summary>
    public static readonly Dictionary<LogSeverity, string> SeverityDictionary = new Dictionary<LogSeverity, string>()
    {
        [LogSeverity.Debug] = "DBG",
        [LogSeverity.Information] = "INF",
        [LogSeverity.Success] = "SCS",
        [LogSeverity.Warning] = "WRN",
        [LogSeverity.Error] = "ERR",
        [LogSeverity.Fatal] = "FTL"
    };
}