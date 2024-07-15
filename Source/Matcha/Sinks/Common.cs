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
using System.Text;
using AnalogFeelings.Matcha.Sinks.Console;

namespace AnalogFeelings.Matcha.Sinks;

/// <summary>
/// Common methods for sinks.
/// </summary>
public static class Common
{
    /// <summary>
    /// Generates the indentation strings.
    /// </summary>
    /// <param name="lineCount">The amount of lines in the log entry.</param>
    /// <param name="headerLength">The length of the log header section.</param>
    /// <param name="useColor">Specifies if the function should generate color codes.</param>
    /// <param name="indentBuilder">The string builder to use.</param>
    /// <param name="indentMiddle">The target string to place the middle indent sequence in.</param>
    /// <param name="indentLast">The target string to place the last indent sequence in.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void GenerateIndents(int lineCount, int headerLength, bool useColor, StringBuilder indentBuilder, out string indentMiddle, out string indentLast)
    {
        indentMiddle = indentLast = string.Empty;

        if (lineCount == 1) return;

        int amountOfSpaces = headerLength - 4;
        int amountOfDashes = headerLength - (headerLength - 3);
        Span<char> spaces = stackalloc char[amountOfSpaces];
        Span<char> dashes = stackalloc char[amountOfDashes];

        spaces.Fill(' ');
        dashes.Fill(SharedConstants.BOX_HORIZONTAL);

        if (useColor)
        {
            indentBuilder.Append(ColorConstants.WHITE);
            indentBuilder.Append(spaces);
            indentBuilder.Append(SharedConstants.BOX_UPRIGHT);
            indentBuilder.Append(dashes);
            indentBuilder.Append(ColorConstants.ANSI_RESET);
        }
        else
        {
            indentBuilder.Append(spaces);
            indentBuilder.Append(SharedConstants.BOX_UPRIGHT);
            indentBuilder.Append(dashes);
        }

        indentLast = indentBuilder.ToString();

        if (lineCount <= 2) return;

        // Only bother initializing middle header if we got more than 2 lines total.
        indentBuilder.Clear();

        if (useColor)
        {
            indentBuilder.Append(ColorConstants.WHITE);
            indentBuilder.Append(spaces);
            indentBuilder.Append(SharedConstants.BOX_VERTRIGHT);
            indentBuilder.Append(dashes);
            indentBuilder.Append(ColorConstants.ANSI_RESET);
        }
        else
        {
            indentBuilder.Append(spaces);
            indentBuilder.Append(SharedConstants.BOX_VERTRIGHT);
            indentBuilder.Append(dashes);
        }

        indentMiddle = indentBuilder.ToString();
    }
}