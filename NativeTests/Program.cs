﻿using System;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace NativeTests
{
    class Program
    {
        [DllImport("dbghelp.dll", SetLastError = true, CharSet = CharSet.Unicode)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool SymInitialize(IntPtr hProcess, string UserSearchPath, [MarshalAs(UnmanagedType.Bool)] bool fInvadeProcess);

        [DllImport("dbghelp.dll", SetLastError = true, CharSet = CharSet.Unicode)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool SymCleanup(IntPtr hProcess);

        [DllImport("dbghelp.dll", SetLastError = true, CharSet = CharSet.Unicode)]
        public static extern ulong SymLoadModuleEx(IntPtr hProcess, IntPtr hFile,
            string ImageName, string ModuleName, long BaseOfDll, int DllSize, IntPtr Data, int Flags);

        [DllImport("dbghelp.dll", SetLastError = true, CharSet = CharSet.Unicode)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool SymEnumerateSymbols64(IntPtr hProcess,
            ulong BaseOfDll, SymEnumerateSymbolsProc64 EnumSymbolsCallback, IntPtr UserContext);
        
        public delegate bool SymEnumerateSymbolsProc64(string SymbolName,
            ulong SymbolAddress, uint SymbolSize, IntPtr UserContext);

        public static unsafe bool EnumSyms(string name, ulong address, uint size, IntPtr context)
        {
            Console.Out.WriteLine($"{address} ({size})- {name}");
            
            return true;
        }

        static ulong baseOfDll;
        private static IntPtr hCurrentProcess;

        static void Main(string[] args)
        {
            hCurrentProcess = Process.GetCurrentProcess().Handle;

            ulong baseOfDll;
            bool status;

            // Initialize sym.
            // Please read the remarks on MSDN for the hProcess
            // parameter.
            status = SymInitialize(hCurrentProcess, null, false);

            if (status == false)
            {
                Console.Out.WriteLine("Failed to initialize sym.");
                return;
            }

            // Load dll.
            baseOfDll = SymLoadModuleEx(hCurrentProcess,
                IntPtr.Zero,
                @"C:\Projects\CSharp\SCVE\NativeTests\bin\Debug\net5.0\glfw3.dll",
                null,
                0,
                0,
                IntPtr.Zero,
                0);

            if (baseOfDll == 0)
            {
                Console.Out.WriteLine("Failed to load module.");
                SymCleanup(hCurrentProcess);
                return;
            }

            Console.WriteLine("Symbols");
            
            // Enumerate symbols. For every symbol the 
            // callback method EnumSyms is called.
            if (SymEnumerateSymbols64(hCurrentProcess, baseOfDll, EnumSyms, IntPtr.Zero) == false)
            {
                Console.Out.WriteLine("Failed to enum symbols.");
            }

            // Cleanup.
            SymCleanup(hCurrentProcess);
        }
    }
}