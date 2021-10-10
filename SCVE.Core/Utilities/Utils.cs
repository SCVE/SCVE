using System;
using System.Diagnostics;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace SCVE.Core.Utilities
{
    public class Utils
    {
        [MethodImpl(MethodImplOptions.NoInlining)]
        public static string GetCurrentMethodName()
        {
            var st = new StackTrace(2);
            var sf = st.GetFrame(0);

            return $"{sf.GetMethod().DeclaringType.Name}.{sf.GetMethod().Name}()";
        }

        public static void TODO(MethodBase method)
        {
            var methodName = $"{method.DeclaringType.Name}.{method.Name}()";
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"TODO: {methodName}");
            Console.ResetColor();
        }

        public static void PrintCurrentMethod()
        {
            Console.WriteLine($"{GetCurrentMethodName()}");
        }
    }
}