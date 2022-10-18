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

		private readonly StreamWriter _LogFileWriter;
		private readonly Regex _AnsiRegex = new Regex("\\x1b([NOP\\\\X^_c]|(\\[[0-9;]*[A-HJKSTfimnsu])|(].+(\\x07|(\\x1b\\\\))))", RegexOptions.Compiled);
		private readonly object _LogLock = new object();
		
		private bool _Disposed = false;

		private string _DebugName;
		private string _InformationName;
		private string _SuccessName;
		private string _WarningName;
		private string _ErrorName;
		private string _FatalName;

		private const string _SHORT_DEBUG_NAME = "@";
		private const string _SHORT_INFORMATION_NAME = "*";
		private const string _SHORT_SUCCESS_NAME = "√";
		private const string _SHORT_WARNING_NAME = "!";
		private const string _SHORT_ERROR_NAME = "~";
		private const string _SHORT_FATAL_NAME = "X";

		private const string _LONG_DEBUG_NAME = "DBG";
		private const string _LONG_INFORMATION_NAME = "MSG";
		private const string _LONG_SUCCESS_NAME = "SUC";
		private const string _LONG_WARNING_NAME = "WRN";
		private const string _LONG_ERROR_NAME = "ERR";
		private const string _LONG_FATAL_NAME = "FTL";

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

			if (Settings.UseShortNames)
			{
				_DebugName = _SHORT_DEBUG_NAME;
				_InformationName = _SHORT_INFORMATION_NAME;
				_SuccessName = _SHORT_SUCCESS_NAME;
				_WarningName = _SHORT_WARNING_NAME;
				_ErrorName = _SHORT_ERROR_NAME;
				_FatalName = _SHORT_FATAL_NAME;
			}
			else
			{
				_DebugName = _LONG_DEBUG_NAME;
				_InformationName = _LONG_INFORMATION_NAME;
				_SuccessName = _LONG_SUCCESS_NAME;
				_WarningName = _LONG_WARNING_NAME;
				_ErrorName = _LONG_ERROR_NAME;
				_FatalName = _LONG_FATAL_NAME;
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
						assembledMessage += _DebugName.Pastel(Color.Teal);
						messageColor = Color.LightSeaGreen;
						break;
					case LogSeverity.Information:
						assembledMessage += _InformationName.Pastel(Color.Cyan);
						break;
					case LogSeverity.Success:
						assembledMessage += _SuccessName.Pastel(Color.Green);
						messageColor = Color.LightGreen;
						break;
					case LogSeverity.Warning:
						assembledMessage += _WarningName.Pastel(Color.Yellow);
						messageColor = Color.PaleGoldenrod;
						break;
					case LogSeverity.Error:
						assembledMessage += _ErrorName.Pastel(Color.Red);
						messageColor = Color.IndianRed;
						break;
					case LogSeverity.Fatal:
						assembledMessage += _FatalName.Pastel(Color.DarkRed);
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
		/// <param name="Enabled"><see langword="true"/> if you want to colorize the output, <see langword="false"/> if you don't.</param>
		public void ToggleColorization(bool Enabled)
		{
			lock (_LogLock)
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
		}

		/// <summary>
		/// Turns short names on or off.
		/// <para/>
		/// The change will be reflected in <see cref="MatchaLoggerSettings.UseShortNames"/>.
		/// </summary>
		/// <param name="Enabled"><see langword="true"/> if you want to use short names, <see langword="false"/> if you don't.</param>
		public void ToggleShortNames(bool Enabled)
		{
			lock (_LogLock)
			{
				LoggerSettings.UseShortNames = Enabled;

				if (LoggerSettings.UseShortNames)
				{
					_DebugName = _SHORT_DEBUG_NAME;
					_InformationName = _SHORT_INFORMATION_NAME;
					_SuccessName = _SHORT_SUCCESS_NAME;
					_WarningName = _SHORT_WARNING_NAME;
					_ErrorName = _SHORT_ERROR_NAME;
					_FatalName = _SHORT_FATAL_NAME;
				}
				else
				{
					_DebugName = _LONG_DEBUG_NAME;
					_InformationName = _LONG_INFORMATION_NAME;
					_SuccessName = _LONG_SUCCESS_NAME;
					_WarningName = _LONG_WARNING_NAME;
					_ErrorName = _LONG_ERROR_NAME;
					_FatalName = _LONG_FATAL_NAME;
				}
			}
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
