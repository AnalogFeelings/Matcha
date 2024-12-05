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

namespace AnalogFeelings.Matcha.Sinks.Console;

/// <summary>
/// Contains constant strings for color strings.
/// </summary>
internal static class ColorConstants
{
    /// <summary>
    /// The ANSI escape sequence for white.
    /// </summary>
    public const string WHITE = "\u001b[38;2;255;255;255m";

    /// <summary>
    /// The ANSI escape sequence for light gray.
    /// </summary>
    public const string LIGHT_GRAY = "\u001b[38;2;211;211;211m";

    /// <summary>
    /// The ANSI escape sequence for cyan.
    /// </summary>
    public const string CYAN = "\u001b[38;2;0;255;255m";

    /// <summary>
    /// The ANSI escape sequence for light blue.
    /// </summary>
    public const string LIGHT_BLUE = "\u001b[38;2;173;216;230m";

    /// <summary>
    /// The ANSI escape sequence for green.
    /// </summary>
    public const string GREEN = "\u001b[38;2;0;255;0m";

    /// <summary>
    /// The ANSI escape sequence for light green.
    /// </summary>
    public const string LIGHT_GREEN = "\u001b[38;2;144;238;144m";

    /// <summary>
    /// The ANSI escape sequence for yellow.
    /// </summary>
    public const string YELLOW = "\u001b[38;2;255;255;0m";

    /// <summary>
    /// The ANSI escape sequence for light yellow.
    /// </summary>
    public const string LIGHT_YELLOW = "\u001b[38;2;238;232;170m";

    /// <summary>
    /// The ANSI escape sequence for red.
    /// </summary>
    public const string RED = "\u001b[38;2;255;0;0m";

    /// <summary>
    /// The ANSI escape sequence for light red.
    /// </summary>
    public const string LIGHT_RED = "\u001b[38;2;205;92;92m";

    /// <summary>
    /// The ANSI escape sequence for orange.
    /// </summary>
    public const string ORANGE = "\u001b[38;2;255;128;0m";

    /// <summary>
    /// The ANSI escape sequence for brown.
    /// </summary>
    public const string BROWN = "\u001b[38;2;255;162;69m";

    /// <summary>
    /// The ANSI escape sequence for brown.
    /// </summary>
    public const string PINK = "\u001b[38;2;209;6;145m";

    /// <summary>
    /// The ANSI escape sequence for light pink.
    /// </summary>
    public const string LIGHT_PINK = "\u001b[38;2;201;91;166m";

    /// <summary>
    /// The ANSI escape sequence to reset all styles.
    /// </summary>
    public const string ANSI_RESET = "\u001b[0m";
}