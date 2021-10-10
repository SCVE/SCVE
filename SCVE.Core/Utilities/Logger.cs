using System;

namespace SCVE.Core.Utilities
{
    public enum LogLevel : int
    {
        Trace = 0,
        Warn = 1,
        Error = 2,
        Fatal = 3
    }

    public class Logger
    {
        public static LogLevel MinLevel = LogLevel.Warn;

        public static void Trace(string trace)
        {
            if (MinLevel > LogLevel.Trace) return;

            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine($"TRACE: {trace}");
            Console.ResetColor();
        }

        public static void Warn(string warn)
        {
            if (MinLevel > LogLevel.Warn) return;
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine($"WARN: {warn}");
            Console.ResetColor();
        }

        public static void Error(string error)
        {
            if (MinLevel > LogLevel.Error) return;
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"ERROR: {error}");
            Console.ResetColor();
        }

        public static void Fatal(string fatal)
        {
            if (MinLevel > LogLevel.Fatal) return;
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"FATAL: {fatal}");
            Console.ResetColor();
        }
    }
}