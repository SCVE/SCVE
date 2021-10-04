using System;
using PeNet;

namespace NativeTests
{
    class Program
    {
        static void Main(string[] args)
        {
            var peFile = new PeFile(@"C:\Projects\CSharp\SCVE\NativeTests\bin\Debug\net5.0\glfw3.dll");
            
            foreach (var exportedFunction in peFile.ExportedFunctions)
            {
                Console.WriteLine(exportedFunction.Name);
            }
            
            int a = 5;
        }
    }
}