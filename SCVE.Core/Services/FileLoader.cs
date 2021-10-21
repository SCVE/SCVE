using System;
using System.IO;
using System.Linq;
using SCVE.Core.App;
using SCVE.Core.Rendering;
using SCVE.Core.Utilities;

namespace SCVE.Core.Services
{
    public class FileLoader
    {
        private const string BaseDirectory = "assets/ShaderPrograms";

        public Program LoadProgram(string name)
        {
            // Note: it's easier to store 16 based extension because of the length
            
            var directoryInfo = new DirectoryInfo($"{BaseDirectory}/{name}");
            if (directoryInfo.Exists)
            {
                var files = directoryInfo.EnumerateFiles();
                var compiledFileName = files.Select(f => Path.GetFileName(f.Name)).FirstOrDefault(f => f.StartsWith("compiled"));

                if (compiledFileName is null)
                {
                    var vertexShaderSourceGlsl = File.ReadAllText($"{BaseDirectory}/{name}/vertex.glsl");
                    var fragmentShaderSourceGlsl = File.ReadAllText($"{BaseDirectory}/{name}/fragment.glsl");

                    using var vertexShader = Application.Instance.RenderEntitiesCreator.CreateShader(vertexShaderSourceGlsl, ScveShaderType.Vertex);
                    using var fragmentShader = Application.Instance.RenderEntitiesCreator.CreateShader(fragmentShaderSourceGlsl, ScveShaderType.Fragment);

                    vertexShader.Compile();
                    fragmentShader.Compile();

                    var program = Application.Instance.RenderEntitiesCreator.CreateProgram();

                    program.AttachShader(vertexShader);
                    program.AttachShader(fragmentShader);
                    program.Link();

                    program.DetachShader(vertexShader);
                    program.DetachShader(fragmentShader);

                    var shaderProgramBinaryData = program.GetBinary();

                    File.WriteAllBytes($"{BaseDirectory}/{name}/compiled_{shaderProgramBinaryData.Extension:X}.bin", shaderProgramBinaryData.Data);

                    return program;
                }
                else
                {
                    var substring = compiledFileName[(compiledFileName.IndexOf("_", StringComparison.Ordinal) + 1)..];
                    substring = substring[..substring.IndexOf(".bin", StringComparison.Ordinal)];
                    int extension = Convert.ToInt32(substring, 16);

                    var bytes = File.ReadAllBytes($"{BaseDirectory}/{name}/compiled_{extension:X}.bin");
                    var program = Application.Instance.RenderEntitiesCreator.CreateProgram(bytes, extension);

                    return program;
                }
            }
            else
            {
                Logger.Error($"ShaderProgram ({name}) was not found! Fallback to (Default)");
                var program = LoadProgram("Default");
                if (program is null)
                {
                    throw new ScveException("Default shader was not found in assets folder");
                }

                return program;
            }

            return null;
        }
    }
}