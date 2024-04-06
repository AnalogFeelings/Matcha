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

using System.Text;
using AnalogFeelings.Matcha.Enums;
using AnalogFeelings.Matcha.Interfaces;
using AnalogFeelings.Matcha.Models;
using Pastel;

namespace AnalogFeelings.Matcha.Sinks.Console;

using Console = System.Console;
using SeverityData = (string Header, string HeaderColor, string TextColor);

/// <summary>
/// A simple sink to output logs to the console.
/// </summary>
public sealed class ConsoleSink : IMatchaSink<ConsoleSinkConfig>
{
    /// <inheritdoc/>
    public required ConsoleSinkConfig Config { get; init; }

    /// <summary>
    /// Dictionary to quickly convert a severity to a string and its color.
    /// </summary>
    private readonly Dictionary<LogSeverity, SeverityData> _severityDict = new Dictionary<LogSeverity, SeverityData>()
    {
        [LogSeverity.Debug] = ("DBG", "#D10691", "#C95BA6"),
        [LogSeverity.Information] = ("INF", "#00FFFF", "#ADD8E6"),
        [LogSeverity.Success] = ("SCS", "#00FF00", "#90EE90"),
        [LogSeverity.Warning] = ("WRN", "#FFFF00", "#EEE8AA"),
        [LogSeverity.Error] = ("ERR", "#FF0000", "#CD5C5C"),
        [LogSeverity.Fatal] = ("FTL", "#FF8000", "#FFA245")
    };

    /// <summary>
    /// Constant array of newline sequences.
    /// </summary>
    private readonly string[] _newlineArray = [ "\n", "\r\n" ];

    /// <summary>
    /// Provides a semaphore to prevent race conditions.
    /// </summary>
    private readonly SemaphoreSlim _semaphore = new SemaphoreSlim(1, 1);

    /// <summary>
    /// Lower-left corner of a box.
    /// </summary>
    private const char _BOX_UPRIGHT = '\u2514';
    /// <summary>
    /// Middle-left corner of a box.
    /// </summary>
    private const char _BOX_VERTRIGHT = '\u251c';
    /// <summary>
    /// Horizontal border of a box.
    /// </summary>
    private const char _BOX_HORIZONTAL = '\u2500';

    /// <inheritdoc/>
    public async Task WriteLogAsync(LogEntry entry)
    {
        if (!Config.Enabled)
            return;
        if ((int)Config.SeverityFilterLevel > (int)entry.Severity)
            return;

        await _semaphore.WaitAsync();

        try
        {
            if(Config.UseColors)
                ConsoleExtensions.Enable();
            else
                ConsoleExtensions.Disable();

            StringBuilder logHeaderBuilder = new StringBuilder();
            int logHeaderLength = 1;
        
            // Using a char is faster.
            logHeaderBuilder.Append('[');

            if (Config.OutputDate)
            {
                string date = entry.Time.ToString(Config.DateFormat);
                string dateColored = date.Pastel(ColorConstants.LIGHT_GRAY);

                logHeaderBuilder.Append(dateColored).Append(' ');

                // Account for the space.
                logHeaderLength += date.Length + 1;
            }

            SeverityData data = _severityDict[entry.Severity];
            string header = data.Header.Pastel(data.HeaderColor);

            logHeaderBuilder.Append(header).Append(']');

            logHeaderLength += data.Header.Length + 1;

            string logHeader = logHeaderBuilder.ToString().Pastel(ColorConstants.WHITE);
            
            string newLineHeaderFirst = string.Empty;
            string newLineHeaderLast = string.Empty;
            
            string formattedContent = string.Format(entry.Content, entry.Format);
            string[] splittedContent = formattedContent.Split(_newlineArray, StringSplitOptions.None);
            
            StringBuilder fullBuilder = new StringBuilder();
            StringBuilder newLineHeaderBuilder = new StringBuilder();
                    
            if (splittedContent.Length > 1)
            {
                newLineHeaderBuilder.Append(_BOX_UPRIGHT).Append(new string(_BOX_HORIZONTAL, logHeaderLength - 1));

                newLineHeaderLast = newLineHeaderBuilder.ToString().Pastel(ColorConstants.WHITE);
            }
            if (splittedContent.Length > 2)
            {
                // Only bother initializing middle header if we got more than 2 lines total.
                newLineHeaderBuilder.Clear();
                
                newLineHeaderBuilder.Append(_BOX_VERTRIGHT).Append(new string(_BOX_HORIZONTAL, logHeaderLength - 1));
                
                newLineHeaderFirst = newLineHeaderBuilder.ToString().Pastel(ColorConstants.WHITE);
            }

            for (int i = 0; i < splittedContent.Length; i++)
            {
                string contentLine = splittedContent[i];
                string contentLineColored = contentLine.Pastel(data.TextColor);
                
                if (i == 0)
                {
                    fullBuilder.Append(logHeader);
                }
                // Are we on the second line?
                else if (i == 1)
                {
                    // Theres only 2 lines, put the end already.
                    if(splittedContent.Length == 2)
                        fullBuilder.Append(newLineHeaderLast);
                    else if(splittedContent.Length > 2)
                        fullBuilder.Append(newLineHeaderFirst);
                }
                else if (i > 1)
                {
                    // We are on the last line!
                    if(i == splittedContent.Length - 1)
                        fullBuilder.Append(newLineHeaderLast);
                    else
                        fullBuilder.Append(newLineHeaderFirst);
                }
                
                fullBuilder.Append(' ').Append(contentLineColored);
                fullBuilder.Append(Environment.NewLine);
            }
        
            Console.Write(fullBuilder.ToString());
        }
        finally
        {
            _semaphore.Release();
        }
    }
}