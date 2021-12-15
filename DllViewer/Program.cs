using System;
using System.Linq;
using CppAst;
using CppAst.CodeGen.Common;
using CppPinvokeGenerator;
using CppPinvokeGenerator.Templates;

namespace DllViewer
{
    class Program
    {
        static void Main(string[] args)
        {
            var compilation = CppParser.ParseFile(@"C:\Projects\CPP\cimgui\cimgui.cpp");

            foreach (var cppFunction in compilation.Functions)
            {
                Console.WriteLine(cppFunction.Name + " (" + string.Join(", ", cppFunction.Parameters.Select(p => p.Type.GetDisplayName() + p.Name)) + ")");
            }

            var typeMapper      = new TypeMapper(compilation);
            typeMapper.RegisterUnsupportedTypes("");
            var templateManager = new TemplateManager();
            templateManager.SetGlobalFunctionsClassName("CImGui");
            
            PinvokeGenerator.Generate(typeMapper, templateManager, "CImGui", "CImGui.NativeLib", "Bindings.Generated.c", "CImGui.Generated.cs");
        }
    }
}