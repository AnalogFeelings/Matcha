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

    // Reduces GC pressure.
    private readonly StringBuilder _logHeaderBuilder = new StringBuilder();
    private readonly StringBuilder _fullBuilder = new StringBuilder();
    private readonly StringBuilder _indentBuilder = new StringBuilder();

    private const char _BOX_UPRIGHT = '\u2514';
    private const char _BOX_VERTRIGHT = '\u251c';
    private const char _BOX_HORIZONTAL = '\u2500';

    /// <inheritdoc/>
    public async Task WriteLogAsync(LogEntry entry)
    {
        await _semaphore.WaitAsync();

        try
        {
            if(Config.UseColors)
                ConsoleExtensions.Enable();
            else
                ConsoleExtensions.Disable();
            
            int logHeaderLength = 1;
        
            // Using a char is faster.
            _logHeaderBuilder.Append('[');

            if (Config.OutputDate)
            {
                string date = entry.Time.ToString(Config.DateFormat);
                string dateColored = date.Pastel(ColorConstants.LIGHT_GRAY);

                _logHeaderBuilder.Append(dateColored).Append(' ');

                // Account for the space.
                logHeaderLength += date.Length + 1;
            }

            SeverityData data = _severityDict[entry.Severity];
            string header = data.Header.Pastel(data.HeaderColor);

            _logHeaderBuilder.Append(header).Append(']');

            logHeaderLength += data.Header.Length + 1;

            string logHeader = _logHeaderBuilder.ToString().Pastel(ColorConstants.WHITE);
            
            string formattedContent = string.Format(entry.Content, entry.Format);
            string[] splittedContent = formattedContent.Split(_newlineArray, StringSplitOptions.None);

            string indentMiddle = string.Empty;
            string indentLast = string.Empty;
            
            if (splittedContent.Length > 1)
            {
                _indentBuilder.Append(_BOX_UPRIGHT).Append(new string(_BOX_HORIZONTAL, logHeaderLength - 1));

                indentLast = _indentBuilder.ToString().Pastel(ColorConstants.WHITE);
            }
            if (splittedContent.Length > 2)
            {
                // Only bother initializing middle header if we got more than 2 lines total.
                _indentBuilder.Clear();
                
                _indentBuilder.Append(_BOX_VERTRIGHT).Append(new string(_BOX_HORIZONTAL, logHeaderLength - 1));
                
                indentMiddle = _indentBuilder.ToString().Pastel(ColorConstants.WHITE);
            }

            for (int i = 0; i < splittedContent.Length; i++)
            {
                string contentLine = splittedContent[i];
                string contentLineColored = contentLine.Pastel(data.TextColor);
                
                if (i == 0)
                {
                    _fullBuilder.Append(logHeader);
                }
                // Are we on the second line?
                else if (i == 1)
                {
                    // Theres only 2 lines, put the end already.
                    if(splittedContent.Length == 2)
                        _fullBuilder.Append(indentLast);
                    else if(splittedContent.Length > 2)
                        _fullBuilder.Append(indentMiddle);
                }
                else if (i > 1)
                {
                    // We are on the last line!
                    if(i == splittedContent.Length - 1)
                        _fullBuilder.Append(indentLast);
                    else
                        _fullBuilder.Append(indentMiddle);
                }
                
                _fullBuilder.Append(' ').Append(contentLineColored);
                _fullBuilder.Append(Environment.NewLine);
            }
        
            Console.Write(_fullBuilder.ToString());
        }
        finally
        {
            _logHeaderBuilder.Clear();
            _fullBuilder.Clear();
            _indentBuilder.Clear();
            
            _semaphore.Release();
        }
    }
}