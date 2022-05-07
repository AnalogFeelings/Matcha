using System;

namespace Matcha
{
	/// <summary>
	/// This flag enum can be used to visually differentiate different log types, and to allow Matcha to filter out certain logs
	/// if the user wishes to.
	/// </summary>
	[Flags]
	public enum LogSeverity
	{
		Debug,
		Information,
		Warning,
		Error
	}
}
