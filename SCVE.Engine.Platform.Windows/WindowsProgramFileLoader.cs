using System;
using System.IO;
using System.Linq;
using SCVE.Engine.Core;
using SCVE.Engine.Core.Loading.Loaders;
using SCVE.Engine.Core.Main;
using SCVE.Engine.Core.Misc;
using SCVE.Engine.Core.Rendering;
using SCVE.Engine.Core.Utilities;

namespace SCVE.Engine.Platform.Windows
{
    public class WindowsProgramFileLoader : IShaderProgramLoader
    {
        private static readonly string BaseDirectory = $"assets/{nameof(ShaderProgram)}";

        public ShaderProgram Load(string fileName)
        {
            // Note: it's easier to store 16 based extension because of the length
            
            var directoryInfo = new DirectoryInfo($"{BaseDirectory}/{fileName}");
            if (directoryInfo.Exists)
            {
                var files = directoryInfo.EnumerateFiles();
                var compiledFileName = files.Select(f => Path.GetFileName(f.Name)).FirstOrDefault(f => f.StartsWith("compiled"));

                if (compiledFileName is null)
                {
                    var vertexShaderSourceGlsl = File.ReadAllText($"{BaseDirectory}/{fileName}/vertex.glsl");
                    var fragmentShaderSourceGlsl = File.ReadAllText($"{BaseDirectory}/{fileName}/fragment.glsl");

                    using var vertexShader   = ScveEngine.Instance.RenderEntitiesCreator.CreateShader(vertexShaderSourceGlsl, ScveShaderType.Vertex);
                    using var fragmentShader = ScveEngine.Instance.RenderEntitiesCreator.CreateShader(fragmentShaderSourceGlsl, ScveShaderType.Fragment);

                    vertexShader.Compile();
                    fragmentShader.Compile();

                    var program = ScveEngine.Instance.RenderEntitiesCreator.CreateProgram();

                    program.AttachShader(vertexShader);
                    program.AttachShader(fragmentShader);
                    program.Link();

                    program.DetachShader(vertexShader);
                    program.DetachShader(fragmentShader);

                    var shaderProgramBinaryData = program.GetBinary();

                    File.WriteAllBytes($"{BaseDirectory}/{fileName}/compiled_{shaderProgramBinaryData.Extension:X}.bin", shaderProgramBinaryData.Data);

                    return program;
                }
                else
                {
                    var substring = compiledFileName[(compiledFileName.IndexOf("_", StringComparison.Ordinal) + 1)..];
                    substring = substring[..substring.IndexOf(".bin", StringComparison.Ordinal)];
                    int extension = Convert.ToInt32(substring, 16);

                    var bytes = File.ReadAllBytes($"{BaseDirectory}/{fileName}/compiled_{extension:X}.bin");
                    var program = ScveEngine.Instance.RenderEntitiesCreator.CreateProgram(bytes, extension);

                    return program;
                }
            }
            else
            {
                Logger.Error($"ShaderProgram ({fileName}) was not found! Fallback to (Default)");
                var program = Load("Default");
                if (program is null)
                {
                    throw new ScveException("Default shader was not found in assets folder");
                }

                return program;
            }
        }
    }
}