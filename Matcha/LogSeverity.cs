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
        Debug = 1,
        Information = 2,
        Success = 4,
        Warning = 8,
        Error = 16,
        Fatal = 32
    }
}
