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

		private StreamWriter LogFileWriter;
		private Regex AnsiRegex = new Regex("\\x1b([NOP\\\\X^_c]|(\\[[0-9;]*[A-HJKSTfimnsu])|(].+(\\x07|(\\x1b\\\\))))", RegexOptions.Compiled);

		private object LogLock = new object();
		private bool Disposed = false;

		/// <summary>
		/// The constructor for <see cref="MatchaLogger"/>.
		/// </summary>
		/// <param name="Settings">An instance of the <see cref="MatchaLoggerSettings"/> class.</param>
		public MatchaLogger(MatchaLoggerSettings Settings)
		{
			if (Settings.LogToFile)
			{
				FileMode TargetMode = FileMode.Append;

				string LogFilename = DateTime.Now.ToString(Settings.FilenameFormat) + ".txt";
				string TotalPath = Path.Combine(Settings.LogFilePath, LogFilename);

				if (!Directory.Exists(Settings.LogFilePath)) Directory.CreateDirectory(Settings.LogFilePath);
				if (Settings.OverwriteIfExists) TargetMode = FileMode.OpenOrCreate;

				LogFileWriter = new StreamWriter(File.Open(TotalPath, TargetMode, FileAccess.Write, FileShare.ReadWrite));
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
			lock (LogLock)
			{
				if (!LoggerSettings.AllowedSeverities.HasFlag(Severity)) return;

				string AssembledMessage = "[".Pastel(Color.White);

				if (LoggerSettings.OutputDate)
				{
					AssembledMessage += DateTime.Now.ToString(LoggerSettings.DateFormat).Pastel(Color.LightGray);
					AssembledMessage += " ";
				}

				Color MessageColor = Color.LightBlue;
				switch (Severity)
				{
					case LogSeverity.Debug:
						AssembledMessage += "DBG".Pastel(Color.Teal);
						MessageColor = Color.LightSeaGreen;
						break;
					case LogSeverity.Information:
						AssembledMessage += "MSG".Pastel(Color.Cyan);
						break;
					case LogSeverity.Success:
						AssembledMessage += "SUC".Pastel(Color.Green);
						MessageColor = Color.LightGreen;
						break;
					case LogSeverity.Warning:
						AssembledMessage += "WRN".Pastel(Color.Yellow);
						MessageColor = Color.Gold;
						break;
					case LogSeverity.Error:
						AssembledMessage += "ERR".Pastel(Color.Red);
						MessageColor = Color.IndianRed;
						break;
					case LogSeverity.Fatal:
						AssembledMessage += "FTL".Pastel(Color.DarkRed);
						MessageColor = Color.DarkRed;
						break;
				}

				AssembledMessage += "] ".Pastel(Color.White);
				AssembledMessage += Message.Pastel(MessageColor);

				if (LoggerSettings.LogToConsole) Console.WriteLine(AssembledMessage);

				if (LoggerSettings.ColorizeOutput) AssembledMessage = AnsiRegex.Replace(AssembledMessage, "");
				if (LoggerSettings.LogToFile)
				{
					LogFileWriter.WriteLine(AssembledMessage);
					LogFileWriter.Flush();
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
			if (!this.Disposed)
			{
				if (Disposing)
				{
					if (LogFileWriter != null) LogFileWriter.Dispose();
				}

				Disposed = true;
			}
		}
	}
}
