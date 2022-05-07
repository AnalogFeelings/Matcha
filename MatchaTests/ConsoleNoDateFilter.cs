using Matcha;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Pastel;
using System;
using System.Drawing;
using System.IO;

namespace MatchaTests
{
	[TestClass]
	public class ConsoleNoDateFilter
	{
		[TestMethod]
		public void NoDateWithFilterAndColor()
		{
			using (StringWriter Writer = new StringWriter())
			{
				Console.SetOut(Writer);

				MatchaLoggerSettings Settings = new MatchaLoggerSettings()
				{
					LogToFile = false,
					ColorizeOutput = true,
					OutputDate = false,
					OverwriteIfExists = true,
					AllowedSeverities = LogSeverity.Debug | LogSeverity.Error
				};

				MatchaLogger Logger = new MatchaLogger(Settings);

				string ExpectedDebug = "[".Pastel(Color.White) +
					"DBG".Pastel(Color.Teal) +
					"] ".Pastel(Color.White) +
					"This is a debug message!".Pastel(Color.LightSeaGreen);
				string ExpectedInformation = string.Empty;
				string ExpectedWarning = string.Empty;
				string ExpectedError = "[".Pastel(Color.White) +
					"ERR".Pastel(Color.Red) +
					"] ".Pastel(Color.White) +
					"This is an error message!".Pastel(Color.IndianRed);

				string Result;

				//=====================DEBUG MESSAGE=====================//
				Logger.Log("This is a debug message!", LogSeverity.Debug);

				Result = Writer.ToString().Trim();
				Assert.AreEqual(ExpectedDebug, Result);

				Writer.GetStringBuilder().Clear();

				//======================INFO MESSAGE======================//
				Logger.Log("This is an information message!", LogSeverity.Information);

				Result = Writer.ToString().Trim();
				Assert.AreEqual(ExpectedInformation, Result);

				Writer.GetStringBuilder().Clear();

				//======================WARN MESSAGE======================//
				Logger.Log("This is a warning message!", LogSeverity.Warning);

				Result = Writer.ToString().Trim();
				Assert.AreEqual(ExpectedWarning, Result);

				Writer.GetStringBuilder().Clear();

				//=====================ERROR MESSAGE=====================//
				Logger.Log("This is an error message!", LogSeverity.Error);

				Result = Writer.ToString().Trim();
				Assert.AreEqual(ExpectedError, Result);

				Writer.GetStringBuilder().Clear();

				Logger.Dispose();
			}
		}

		[TestMethod]
		public void NoDateWithFilterNoColor()
		{
			using (StringWriter Writer = new StringWriter())
			{
				Console.SetOut(Writer);

				MatchaLoggerSettings Settings = new MatchaLoggerSettings()
				{
					LogToFile = false,
					ColorizeOutput = false,
					OutputDate = false,
					OverwriteIfExists = true,
					AllowedSeverities = LogSeverity.Debug | LogSeverity.Error
				};

				MatchaLogger Logger = new MatchaLogger(Settings);

				string ExpectedDebug = "[" +
					"DBG" +
					"] " +
					"This is a debug message!";
				string ExpectedInformation = string.Empty;
				string ExpectedWarning = string.Empty;
				string ExpectedError = "[" +
					"ERR" +
					"] " +
					"This is an error message!";

				string Result;

				//=====================DEBUG MESSAGE=====================//
				Logger.Log("This is a debug message!", LogSeverity.Debug);

				Result = Writer.ToString().Trim();
				Assert.AreEqual(ExpectedDebug, Result);

				Writer.GetStringBuilder().Clear();

				//======================INFO MESSAGE======================//
				Logger.Log("This is an information message!", LogSeverity.Information);

				Result = Writer.ToString().Trim();
				Assert.AreEqual(ExpectedInformation, Result);

				Writer.GetStringBuilder().Clear();

				//======================WARN MESSAGE======================//
				Logger.Log("This is a warning message!", LogSeverity.Warning);

				Result = Writer.ToString().Trim();
				Assert.AreEqual(ExpectedWarning, Result);

				Writer.GetStringBuilder().Clear();

				//=====================ERROR MESSAGE=====================//
				Logger.Log("This is an error message!", LogSeverity.Error);

				Result = Writer.ToString().Trim();
				Assert.AreEqual(ExpectedError, Result);

				Writer.GetStringBuilder().Clear();

				Logger.Dispose();
			}
		}
	}
}
