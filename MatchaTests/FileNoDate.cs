using Matcha;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.IO;

namespace MatchaTests
{
	[TestClass]
	public class FileNoDate
	{
		[TestMethod]
		public void NoDateWithFolder()
		{
			MatchaLoggerSettings Settings = new MatchaLoggerSettings()
			{
				LogToFile = true,
				ColorizeOutput = true,
				OutputDate = false,
				OverwriteIfExists = true,
			};

			string TargetFilename = Path.Combine(Settings.LogFilePath, DateTime.Now.ToString(Settings.FilenameFormat) + ".txt");

			MatchaLogger Logger = new MatchaLogger(Settings);

			string ExpectedContent = "[" + "DBG" + "] " + "This is a debug message!" + "\r\n";
			ExpectedContent += "[" + "MSG" + "] " + "This is an information message!" + "\r\n";
			ExpectedContent += "[" + "WRN" + "] " + "This is a warning message!" + "\r\n";
			ExpectedContent += "[" + "ERR" + "] " + "This is an error message!" + "\r\n";

			Logger.Log("This is a debug message!", LogSeverity.Debug);
			Logger.Log("This is an information message!", LogSeverity.Information);
			Logger.Log("This is a warning message!", LogSeverity.Warning);
			Logger.Log("This is an error message!", LogSeverity.Error);

			string RetrievedContent;

			using (FileStream FileStream = new FileStream(TargetFilename, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
			using (StreamReader Reader = new StreamReader(FileStream))
			{
				RetrievedContent = Reader.ReadToEnd();
			}

			Assert.AreEqual(ExpectedContent, RetrievedContent);

			Logger.Dispose();

			File.Delete(TargetFilename);
		}

		[TestMethod]
		public void NoDateNoFolder()
		{
			MatchaLoggerSettings Settings = new MatchaLoggerSettings()
			{
				LogToFile = true,
				ColorizeOutput = true,
				OutputDate = false,
				OverwriteIfExists = true,
				LogFilePath = "./"
			};

			string TargetFilename = Path.Combine(Settings.LogFilePath, DateTime.Now.ToString(Settings.FilenameFormat) + ".txt");

			MatchaLogger Logger = new MatchaLogger(Settings);

			string ExpectedContent = "[" + "DBG" + "] " + "This is a debug message!" + "\r\n";
			ExpectedContent += "[" + "MSG" + "] " + "This is an information message!" + "\r\n";
			ExpectedContent += "[" + "WRN" + "] " + "This is a warning message!" + "\r\n";
			ExpectedContent += "[" + "ERR" + "] " + "This is an error message!" + "\r\n";

			Logger.Log("This is a debug message!", LogSeverity.Debug);
			Logger.Log("This is an information message!", LogSeverity.Information);
			Logger.Log("This is a warning message!", LogSeverity.Warning);
			Logger.Log("This is an error message!", LogSeverity.Error);

			string RetrievedContent;

			using (FileStream FileStream = new FileStream(TargetFilename, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
			using (StreamReader Reader = new StreamReader(FileStream))
			{
				RetrievedContent = Reader.ReadToEnd();
			}

			Assert.AreEqual(ExpectedContent, RetrievedContent);

			Logger.Dispose();

			File.Delete(TargetFilename);
		}
	}
}