using Pastel;
using System;
using System.Drawing;
using System.IO;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;

namespace Matcha
{
	/// <summary>
	/// The core class of Matcha. This is where you can tell it to log messages.
	/// </summary>
	public class MatchaLogger : IDisposable
	{
		/// <summary>
		/// The instance of <see cref="MatchaLoggerSettings"/> passed to <see cref="MatchaLogger"/>'s constructor.
		/// </summary>
		public MatchaLoggerSettings LoggerSettings { get; set; }

		private StreamWriter _LogFileWriter;
		private Regex _AnsiRegex = new Regex("\\x1b([NOP\\\\X^_c]|(\\[[0-9;]*[A-HJKSTfimnsu])|(].+(\\x07|(\\x1b\\\\))))", RegexOptions.Compiled);

		private object _LogLock = new object();
		private bool _Disposed = false;

		/// <summary>
		/// The constructor for <see cref="MatchaLogger"/>.
		/// </summary>
		/// <param name="Settings">An instance of the <see cref="MatchaLoggerSettings"/> class.</param>
		public MatchaLogger(MatchaLoggerSettings Settings)
		{
			if (Settings.LogToFile)
			{
				FileMode targetMode = FileMode.Append;

				string logFilename = DateTime.Now.ToString(Settings.FilenameFormat) + ".txt";
				string totalPath = Path.Combine(Settings.LogFilePath, logFilename);

				if (!Directory.Exists(Settings.LogFilePath)) Directory.CreateDirectory(Settings.LogFilePath);
				if (Settings.OverwriteIfExists) targetMode = FileMode.OpenOrCreate;

				_LogFileWriter = new StreamWriter(File.Open(totalPath, targetMode, FileAccess.Write, FileShare.ReadWrite));
			}

			//If user requests no colorization, or the OS is Windows but it's older than Windows 10.
			if (!Settings.ColorizeOutput || (RuntimeInformation.IsOSPlatform(OSPlatform.Windows) && Environment.OSVersion.Version.Major < 10))
			{
				ConsoleExtensions.Disable();
			}

			LoggerSettings = Settings;
		}

		/// <summary>
		/// The destructor for <see cref="MatchaLogger"/>.
		/// </summary>
		~MatchaLogger()
		{
			Dispose(false);
		}

		/// <summary>
		/// Call this function to output a log message to the console if <see cref="MatchaLoggerSettings.LogToConsole"/> is set to <see langword="true"/>.
		/// <para/>
		/// It will also output it to the log file is <see cref="MatchaLoggerSettings.LogToFile"/> is set to <see langword="true"/>.
		/// </summary>
		/// <param name="Message">The text to output.</param>
		/// <param name="Severity">The severity of the message.</param>
		public void Log(string Message, LogSeverity Severity)
		{
			lock (_LogLock)
			{
				if (!LoggerSettings.AllowedSeverities.HasFlag(Severity)) return;

				string assembledMessage = "[".Pastel(Color.White);

				if (LoggerSettings.OutputDate)
				{
					assembledMessage += DateTime.Now.ToString(LoggerSettings.DateFormat).Pastel(Color.LightGray);
					assembledMessage += " ";
				}

				Color messageColor = Color.LightBlue;
				switch (Severity)
				{
					case LogSeverity.Debug:
						assembledMessage += "DBG".Pastel(Color.Teal);
						messageColor = Color.LightSeaGreen;
						break;
					case LogSeverity.Information:
						assembledMessage += "MSG".Pastel(Color.Cyan);
						break;
					case LogSeverity.Success:
						assembledMessage += "SUC".Pastel(Color.Green);
						messageColor = Color.LightGreen;
						break;
					case LogSeverity.Warning:
						assembledMessage += "WRN".Pastel(Color.Yellow);
						messageColor = Color.PaleGoldenrod;
						break;
					case LogSeverity.Error:
						assembledMessage += "ERR".Pastel(Color.Red);
						messageColor = Color.IndianRed;
						break;
					case LogSeverity.Fatal:
						assembledMessage += "FTL".Pastel(Color.DarkRed);
						messageColor = Color.DarkRed;
						break;
				}

				assembledMessage += "] ".Pastel(Color.White);
				assembledMessage += Message.Pastel(messageColor);

				if (LoggerSettings.LogToConsole) Console.WriteLine(assembledMessage);

				if (LoggerSettings.LogToFile)
				{
					if (LoggerSettings.ColorizeOutput) assembledMessage = _AnsiRegex.Replace(assembledMessage, "");
					
					_LogFileWriter.WriteLine(assembledMessage);
					_LogFileWriter.Flush();
				}
			}
		}

		/// <summary>
		/// Turns colorization on or off.
		/// <para/>
		/// The change will be reflected in <see cref="MatchaLoggerSettings.ColorizeOutput"/>.
		/// </summary>
		/// <param name="Enabled">True if you want to colorize the output, false if you don't.</param>
		public void ToggleColorization(bool Enabled)
		{
			if (Enabled)
			{
				ConsoleExtensions.Enable();
				LoggerSettings.ColorizeOutput = true;

				return;
			}

			ConsoleExtensions.Disable();
			LoggerSettings.ColorizeOutput = false;
		}

		/// <summary>
		/// Disposes the logger, closing the log file writer if it exists.
		/// </summary>
		public void Dispose()
		{
			Dispose(true);

			GC.SuppressFinalize(this);
		}

		protected virtual void Dispose(bool Disposing)
		{
			if (!this._Disposed)
			{
				if (Disposing)
				{
					if (_LogFileWriter != null) _LogFileWriter.Dispose();
				}

				_Disposed = true;
			}
		}
	}
}
