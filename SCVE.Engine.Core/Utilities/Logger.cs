﻿#define LOG_CONSTRUCT
using System;


namespace SCVE.Engine.Core.Utilities
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

        public static void Trace(string format, params object[] args)
        {
            Trace(string.Format(format, args));
        }

        public static void Warn(string warn)
        {
            if (MinLevel > LogLevel.Warn) return;
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine($"WARN: {warn}");
            Console.ResetColor();
        }

        public static void Warn(string format, params object[] args)
        {
            Warn(string.Format(format, args));
        }

        public static void WarnIndent(string warn, int indent = 0)
        {
            if (MinLevel > LogLevel.Warn) return;
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.Write("WARN: ");
            for (int i = 0; i < indent; i++)
            {
                Console.Write("  ");
            }
            Console.WriteLine($"{warn}");
            Console.ResetColor();
        }

        public static void WarnIndent(string format, int indent = 0, params object[] args)
        {
            Warn(string.Format(format, args));
        }

        public static void Error(string error)
        {
            if (MinLevel > LogLevel.Error) return;
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"ERROR: {error}");
            Console.ResetColor();
        }

        public static void Error(string format, params object[] args)
        {
            Error(string.Format(format, args));
        }

        public static void Fatal(string fatal)
        {
            if (MinLevel > LogLevel.Fatal) return;
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"FATAL: {fatal}");
            Console.ResetColor();
        }

        public static void Fatal(string format, params object[] args)
        {
            Fatal(string.Format(format, args));
        }

        public static void Construct(string name)
        {
#if LOG_CONSTRUCT
            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.WriteLine($"Constructing: {name}");
            Console.ResetColor();
#endif
        }
    }
}