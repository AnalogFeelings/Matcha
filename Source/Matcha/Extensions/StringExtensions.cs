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

namespace AnalogFeelings.Matcha.Extensions;

/// <summary>
/// Contains utility methods for string objects.
/// </summary>
internal static class StringExtensions
{
    /// <summary>
    /// Constant array of newline sequences.
    /// </summary>
    private static readonly string[] _newlineArray = ["\n", "\r\n"];

    /// <summary>
    /// Splits a string based off newline character sequences.
    /// </summary>
    /// <param name="target">The string to split.</param>
    /// <returns>A collection containing the splitted lines.</returns>
    /// <remarks>
    /// Leading and trailing spaces and newlines are removed.
    /// </remarks>
    public static string[] SplitLines(this string target)
    {
        string trimmed = target.Trim();
        string[] splitted = trimmed.Split(_newlineArray, StringSplitOptions.None);

        return splitted.Where(x => !string.IsNullOrWhiteSpace(x)).ToArray();
    }
}