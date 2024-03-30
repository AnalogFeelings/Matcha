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

using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;

namespace Matcha.Helpers;

/// <summary>
/// Contains helper methods for dealing with ANSI escape sequences.
/// </summary>
internal static class AnsiHelper
{
    private static readonly Regex _AnsiRegex = new Regex(@"\x1b([NOP\\X^_c]|(\[[0-9;]*[A-HJKSTfimnsu])|(].+(\x07|(\x1b\\))))", RegexOptions.Compiled);

    /// <summary>
    /// Removes all ANSI escape sequences from the provided string.
    /// </summary>
    /// <param name="target">The target string.</param>
    /// <returns>A new string without the ANSI escape sequences.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static string RemoveAnsiSequences(string target) => 
        _AnsiRegex.Replace(target, string.Empty);
}