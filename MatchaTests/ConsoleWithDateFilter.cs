using Matcha;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Pastel;
using System;
using System.Drawing;
using System.IO;

namespace MatchaTests
{
	[TestClass]
	public class ConsoleWithDateFilter
	{
		[TestMethod]
		public void WithDateWithFilterAndColor()
		{
			using (StringWriter Writer = new StringWriter())
			{
				Console.SetOut(Writer);

				MatchaLoggerSettings Settings = new MatchaLoggerSettings()
				{
					LogToFile = false,
					ColorizeOutput = true,
					OutputDate = true,
					OverwriteIfExists = true,
					AllowedSeverities = LogSeverity.Debug | LogSeverity.Error | LogSeverity.Fatal
				};

				string TargetDate = DateTime.Now.ToString(Settings.DateFormat);

				using (MatchaLogger Logger = new MatchaLogger(Settings))
				{
					string ExpectedDebug = "[".Pastel(Color.White) +
						TargetDate.Pastel(Color.LightGray) +
						" DBG".Pastel(Color.Teal) +
						"] ".Pastel(Color.White) +
						"This is a debug message!".Pastel(Color.LightSeaGreen);
					string ExpectedInformation = string.Empty;
					string ExpectedSuccess = string.Empty;
					string ExpectedWarning = string.Empty;
					string ExpectedError = "[".Pastel(Color.White) +
						TargetDate.Pastel(Color.LightGray) +
						" ERR".Pastel(Color.Red) +
						"] ".Pastel(Color.White) +
						"This is an error message!".Pastel(Color.IndianRed);
					string ExpectedFatal = "[".Pastel(Color.White) +
						TargetDate.Pastel(Color.LightGray) +
						" FTL".Pastel(Color.DarkRed) +
						"] ".Pastel(Color.White) +
						"This is a fatal message!".Pastel(Color.DarkRed);

					string Result;

					//==================DEBUG MESSAGE==================//
					Logger.Log("This is a debug message!", LogSeverity.Debug);

					Result = Writer.ToString().Trim();
					Assert.AreEqual(ExpectedDebug, Result);

					Writer.GetStringBuilder().Clear();

					//==================INFO MESSAGE==================//
					Logger.Log("This is an information message!", LogSeverity.Information);

					Result = Writer.ToString().Trim();
					Assert.AreEqual(ExpectedInformation, Result);

					Writer.GetStringBuilder().Clear();

					//==================SUCCESS MESSAGE==================//
					Logger.Log("This is a success message!", LogSeverity.Success);

					Result = Writer.ToString().Trim();
					Assert.AreEqual(ExpectedSuccess, Result);

					Writer.GetStringBuilder().Clear();

					//==================WARN MESSAGE==================//
					Logger.Log("This is a warning message!", LogSeverity.Warning);

					Result = Writer.ToString().Trim();
					Assert.AreEqual(ExpectedWarning, Result);

					Writer.GetStringBuilder().Clear();

					//==================ERROR MESSAGE==================//
					Logger.Log("This is an error message!", LogSeverity.Error);

					Result = Writer.ToString().Trim();
					Assert.AreEqual(ExpectedError, Result);

					Writer.GetStringBuilder().Clear();

					//==================FATAL MESSAGE==================//
					Logger.Log("This is a fatal message!", LogSeverity.Fatal);

					Result = Writer.ToString().Trim();
					Assert.AreEqual(ExpectedFatal, Result);

					Writer.GetStringBuilder().Clear();
				}
			}
		}

		[TestMethod]
		public void WithDateWithFilterNoColor()
		{
			using (StringWriter Writer = new StringWriter())
			{
				Console.SetOut(Writer);

				MatchaLoggerSettings Settings = new MatchaLoggerSettings()
				{
					LogToFile = false,
					ColorizeOutput = false,
					OutputDate = true,
					OverwriteIfExists = true,
					AllowedSeverities = LogSeverity.Debug | LogSeverity.Error | LogSeverity.Fatal
				};

				string TargetDate = DateTime.Now.ToString(Settings.DateFormat);

				using (MatchaLogger Logger = new MatchaLogger(Settings))
				{
					string ExpectedDebug = "[" +
						TargetDate +
						" DBG" +
						"] " +
						"This is a debug message!";
					string ExpectedInformation = string.Empty;
					string ExpectedSuccess = string.Empty;
					string ExpectedWarning = string.Empty;
					string ExpectedError = "[" +
						TargetDate +
						" ERR" +
						"] " +
						"This is an error message!";
					string ExpectedFatal = "[" +
						TargetDate +
						" FTL" +
						"] " +
						"This is a fatal message!";

					string Result;

					//==================DEBUG MESSAGE==================//
					Logger.Log("This is a debug message!", LogSeverity.Debug);

					Result = Writer.ToString().Trim();
					Assert.AreEqual(ExpectedDebug, Result);

					Writer.GetStringBuilder().Clear();

					//==================INFO MESSAGE==================//
					Logger.Log("This is an information message!", LogSeverity.Information);

					Result = Writer.ToString().Trim();
					Assert.AreEqual(ExpectedInformation, Result);

					Writer.GetStringBuilder().Clear();

					//==================SUCCESS MESSAGE==================//
					Logger.Log("This is a success message!", LogSeverity.Success);

					Result = Writer.ToString().Trim();
					Assert.AreEqual(ExpectedSuccess, Result);

					Writer.GetStringBuilder().Clear();

					//==================WARN MESSAGE==================//
					Logger.Log("This is a warning message!", LogSeverity.Warning);

					Result = Writer.ToString().Trim();
					Assert.AreEqual(ExpectedWarning, Result);

					Writer.GetStringBuilder().Clear();

					//==================ERROR MESSAGE==================//
					Logger.Log("This is an error message!", LogSeverity.Error);

					Result = Writer.ToString().Trim();
					Assert.AreEqual(ExpectedError, Result);

					Writer.GetStringBuilder().Clear();

					//==================FATAL MESSAGE==================//
					Logger.Log("This is a fatal message!", LogSeverity.Fatal);

					Result = Writer.ToString().Trim();
					Assert.AreEqual(ExpectedFatal, Result);

					Writer.GetStringBuilder().Clear();
				}
			}
		}
	}
}
