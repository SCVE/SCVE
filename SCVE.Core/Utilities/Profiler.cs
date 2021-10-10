using System;
using System.Collections.Generic;

namespace SCVE.Core.Utilities
{
    public class Profiler
    {
        public class Invokations
        {
            private static Dictionary<string, uint> _invokations = new();
            
            public static void Method()
            {
                var methodName = Utils.GetCurrentMethodName();
                if (_invokations.ContainsKey(methodName))
                {
                    _invokations[methodName]++;
                }
                else
                {
                    _invokations[methodName] = 1;
                }
            }

            public static void Print()
            {
                Console.WriteLine("Invokations!");
                foreach (var invokationPair in _invokations)
                {
                    Console.WriteLine($"  {invokationPair.Key} - {invokationPair.Value}");
                }
            }
        }
    }
}