using System;
using System.IO;
using System.Linq;
using SCVE.Core.App;
using SCVE.Core.Rendering;

namespace SCVE.Core.Services
{
    public class FileLoader
    {
        private const string BaseDirectory = "assets/ShaderPrograms";

        public Program LoadProgram(string name)
        {
            if (Directory.Exists($"{BaseDirectory}/{name}"))
            {
                var directoryInfo = new DirectoryInfo($"{BaseDirectory}/{name}");
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
                    var substring = compiledFileName.Substring(compiledFileName.IndexOf("_") + 1);
                    substring = substring.Substring(0, substring.IndexOf(".bin"));
                    int extension = Convert.ToInt32(substring, 16);

                    var bytes = File.ReadAllBytes($"{BaseDirectory}/{name}/compiled_{extension:X}.bin");
                    var program = Application.Instance.RenderEntitiesCreator.CreateProgram(bytes, extension);

                    return program;
                }
            }
            else
            {
                var program = LoadProgram("Default");
                if (program is null)
                {
                    throw new ScveException("Default shader was not found in assets folder");
                }
            }

            return null;
        }
    }
}