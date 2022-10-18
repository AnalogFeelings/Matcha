using System;
using System.IO;

namespace Matcha
{
    /// <summary>
    /// This class is passed to the <see cref="MatchaLogger"/> constructor to tell Matcha how to operate.
    /// </summary>
    public class MatchaLoggerSettings
    {
        /// <summary>
        /// The time and date format for the target filename.
        /// Matcha will open the file and append to it if the file already exists.
        /// <para/>
        /// Set the <see cref="OverwriteIfExists"/> property to <see langword="true"/> if you don't want this behavior.
        /// <para/>
        /// This CANNOT be modified after <see cref="MatchaLogger"/> has been created.
        /// </summary>
        public string FilenameFormat = "yyy-M-d";

        /// <summary>
        /// The path where Matcha will store the log files.
        /// <para/>
        /// This CANNOT be modified after <see cref="MatchaLogger"/> has been created.
        /// </summary>
        public string LogFilePath = "Logs/";

        /// <summary>
        /// Tells Matcha if you want it to log to a file or not.
        /// </summary>
        public bool LogToFile = true;

        /// <summary>
        /// Tells Matcha if you want it to overwrite the log file if it already exists.
        /// <para/>
        /// This CANNOT be modified after <see cref="MatchaLogger"/> has been created.
        /// </summary>
        public bool OverwriteIfExists = false;

        /// <summary>
        /// Tells Matcha if you want it to log to the console, or whatever <see cref="Stream"/> the <see cref="Console"/> class has set.
        /// </summary>
        public bool LogToConsole = true;

        /// <summary>
        /// Tells Matcha if you want the output to be colorized using ANSI escape sequences.
        /// This has no effect under any Windows version older than 10; it will always be false.
        /// <para/>
        /// This is <see langword="true"/> by default.
        /// <para/>
        /// Use <see cref="MatchaLogger.ToggleColorization(bool)"/> to turn colorization on and off during runtime.
        /// </summary>
        public bool ColorizeOutput = true;

        /// <summary>
        /// Tells Matcha to output the date in the log message header.
        /// </summary>
        public bool OutputDate = true;

        /// <summary>
        /// The date format that Matcha will use for the log message header.
        /// </summary>
        public string DateFormat = "g";

        /// <summary>
        /// Tells Matcha to use short names e.g. MSG gets turned into "*", SUC gets turned into "√".
        /// <para/>
        /// This is <see langword="false"/> by default.
        /// <para/>
        /// Use <see cref="MatchaLogger.ToggleShortNames(bool)"/> to turn colorization on and off during runtime.
        /// </summary>
        public bool UseShortNames = false;
        
        /// <summary>
        /// A bit field that tells Matcha which log severities to output.
        /// <para/>
        /// For example, you may prefer to show <see cref="LogSeverity.Debug"/> logs on the DEBUG configuration, or hide <see cref="LogSeverity.Information"/> on RELEASE configuration.
        /// </summary>
        public LogSeverity AllowedSeverities = LogSeverity.Debug | LogSeverity.Information | LogSeverity.Success | LogSeverity.Warning | LogSeverity.Error | LogSeverity.Fatal;
    }
}
