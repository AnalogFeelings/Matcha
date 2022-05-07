using Matcha;

namespace MatchaExample
{
	internal class Program
	{
		static void Main(string[] args)
		{
			//Use the defaults.
			MatchaLoggerSettings Settings = new MatchaLoggerSettings()
			{
				OverwriteIfExists = true
			};

			MatchaLogger Logger = new MatchaLogger(Settings);

			Logger.Log("I'm a debug message!", LogSeverity.Debug);
			Logger.Log("I'm an information message!", LogSeverity.Information);
			Logger.Log("I'm a warning message!", LogSeverity.Warning);
			Logger.Log("I'm an error message!", LogSeverity.Error);

			Logger.LoggerSettings.LogToFile = false;

			Logger.Log("I'm a debug message, and I won't appear in the log file!", LogSeverity.Debug);
			Logger.Log("I'm an information message, and I also won't appear in the log file!", LogSeverity.Information);
			Logger.Log("I'm a warning message, but I don't appear in the log file either!", LogSeverity.Warning);
			Logger.Log("I'm an error message, and I too don't appear in the log file!", LogSeverity.Error);

			Logger.LoggerSettings.LogToFile = true;
			Logger.LoggerSettings.LogToConsole = false;

			Logger.Log("I'm a debug message, and I won't appear in the console!", LogSeverity.Debug);
			Logger.Log("I'm an information message, and I also won't appear in the console!", LogSeverity.Information);
			Logger.Log("I'm a warning message, but I don't appear in the console either!", LogSeverity.Warning);
			Logger.Log("I'm an error message, and I too don't appear in the console!", LogSeverity.Error);

			Logger.LoggerSettings.LogToFile = false;
			Logger.LoggerSettings.LogToConsole = true;

			Logger.ToggleColorization(false);

			Logger.Log("I'm a debug message, I'm not colorized, and I won't appear in the log file!", LogSeverity.Debug);
			Logger.Log("I'm an information message, I'm also not colorized, and I won't appear in the log file either!", LogSeverity.Information);
			Logger.Log("I'm a warning message, I'm not colorized either, and guess what, I also don't appear in the log file!", LogSeverity.Warning);
			Logger.Log("I'm an error message, I'm not colorized, and I too don't appear in the log file!", LogSeverity.Error);
		}
	}
}
