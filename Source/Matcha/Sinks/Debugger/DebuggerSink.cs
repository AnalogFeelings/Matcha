﻿#region License Information (MIT)
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

using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Text;
using AnalogFeelings.Matcha.Extensions;
using AnalogFeelings.Matcha.Interfaces;
using AnalogFeelings.Matcha.Models;

namespace AnalogFeelings.Matcha.Sinks.Debugger;

using Debugger = System.Diagnostics.Debugger;

/// <summary>
/// A simple sink to output logs to the IDE debug output.
/// </summary>
public sealed class DebuggerSink : IMatchaSink<DebuggerSinkConfig>
{
    /// <inheritdoc/>
    public required DebuggerSinkConfig Config { get; init; }
    
    /// <summary>
    /// Provides a semaphore to prevent race conditions.
    /// </summary>
    private readonly SemaphoreSlim _semaphore = new SemaphoreSlim(1, 1);
    
    // Reduces GC pressure.
    private readonly StringBuilder _fullBuilder = new StringBuilder();
    private readonly StringBuilder _indentBuilder = new StringBuilder();

    /// <inheritdoc/>
    public void InitializeSink()
    {
        // Empty.
    }

    /// <inheritdoc/>
    public async Task WriteLogAsync(LogEntry entry)
    {
        if (!Debugger.IsAttached) return;
        
        await _semaphore.WaitAsync();

        try
        {
            _fullBuilder.Append('[');
            
            if (Config.OutputDate)
            {
                string date = entry.Time.ToString(Config.DateFormat);

                _fullBuilder.Append(date);
                _fullBuilder.Append(' ');
            }
            
            _fullBuilder.Append(SharedConstants.SeverityDictionary[entry.Severity]);
            _fullBuilder.Append(']');
            
            string[] splittedContent = entry.Content.SplitLines();
            
            Common.GenerateIndents(splittedContent.Length, _fullBuilder.Length, false, _indentBuilder, out string indentMiddle, out string indentLast);
            
            for (int i = 0; i < splittedContent.Length; i++)
            {
                string contentLine = splittedContent[i];

                // Are we on the second line?
                if (i == 1)
                {
                    // Theres only 2 lines, put the end already.
                    if (splittedContent.Length == 2)
                        _fullBuilder.Append(indentLast);
                    else if (splittedContent.Length > 2)
                        _fullBuilder.Append(indentMiddle);
                }
                else if (i > 1)
                {
                    // We are on the last line!
                    if (i == splittedContent.Length - 1)
                        _fullBuilder.Append(indentLast);
                    else
                        _fullBuilder.Append(indentMiddle);
                }

                _fullBuilder.Append(' ');
                _fullBuilder.Append(contentLine);
                _fullBuilder.AppendLine();
            }
            
            Trace.Write(_fullBuilder.ToString());
        }
        finally
        {
            _fullBuilder.Clear();
            _indentBuilder.Clear();
            
            _semaphore.Release();
        }
    }
}