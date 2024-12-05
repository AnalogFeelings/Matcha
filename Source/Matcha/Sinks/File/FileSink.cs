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

using System.Runtime.CompilerServices;
using System.Text;
using AnalogFeelings.Matcha.Extensions;
using AnalogFeelings.Matcha.Interfaces;
using AnalogFeelings.Matcha.Models;

namespace AnalogFeelings.Matcha.Sinks.File;

using File = System.IO.File;

/// <summary>
/// A simple sink to output logs to a file in the system.
/// </summary>
public sealed class FileSink : IMatchaSink<FileSinkConfig>, IDisposable
{
    /// <inheritdoc/>
    public required FileSinkConfig Config { get; init; }
    
    /// <summary>
    /// The writer for the output file.
    /// </summary>
    private StreamWriter? _writer;
    
    /// <summary>
    /// Provides a semaphore to prevent race conditions.
    /// </summary>
    private readonly SemaphoreSlim _semaphore = new SemaphoreSlim(1, 1);
    
    // Reduces GC pressure.
    private readonly StringBuilder _fullBuilder = new StringBuilder();
    private readonly StringBuilder _indentBuilder = new StringBuilder();

    /// <summary>
    /// Initializes the file stream writer.
    /// </summary>
    public void InitializeSink()
    {
        string logFileName = DateTime.Now.ToString(Config.FileNameFormat) + ".txt";
        string logFilePath = Path.Combine(Config.FilePath, SanitizeFileName(logFileName));
        FileMode logFileMode = FileMode.Append;

        try
        {
            if (!Directory.Exists(Config.FilePath))
                Directory.CreateDirectory(Config.FilePath);

            if (Config.Overwrite)
                logFileMode = FileMode.Create;

            _writer = new StreamWriter(File.Open(logFilePath, logFileMode, FileAccess.Write, FileShare.Read));
        }
        catch (Exception)
        {
            // Ignored.
        }
    }

    /// <inheritdoc/>
    public async Task WriteLogAsync(LogEntry entry)
    {
        if (_writer == null)
            return;
        
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
            
            await _writer.WriteAsync(_fullBuilder.ToString());
            await _writer.FlushAsync();
        }
        finally
        {
            _fullBuilder.Clear();
            _indentBuilder.Clear();
            
            _semaphore.Release();
        }
    }

    /// <summary>
    /// Sanitizes a file name to avoid issues with NTFS or ReFS.
    /// </summary>
    /// <param name="filename">The file name to sanitize.</param>
    /// <returns>The sanitized file name.</returns>
    private string SanitizeFileName(string filename)
    {
        foreach (char character in Path.GetInvalidFileNameChars())
        {
            filename = filename.Replace(character, '-');
        }

        return filename;
    }

    /// <inheritdoc/>
    public void Dispose()
    {
        _writer?.Dispose();
    }
}