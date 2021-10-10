using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading;
using Engine.EngineCore.Renderer;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;

namespace Engine.Platform.OpenGL
{
    public class Utils
    {
        public static TextureTarget TextureTarget(bool multisampled)
        {
            return multisampled ? OpenTK.Graphics.OpenGL.TextureTarget.Texture2dMultisample : OpenTK.Graphics.OpenGL.TextureTarget.Texture2d;
        }

        public static TextureHandle CreateTexture(bool multisampled)
        {
            return GL.CreateTexture(TextureTarget(multisampled));
        }

        public static TextureHandle[] CreateTextures(bool multisampled, int count)
        {
            TextureHandle[] handles = new TextureHandle[count];
            GL.CreateTextures(TextureTarget(multisampled), handles);
            return handles;
        }

        public static void BindTexture(bool multisampled, TextureHandle id)
        {
            GL.BindTexture(TextureTarget(multisampled), id);
        }

        public static void AttachColorTexture(TextureHandle id, int samples, InternalFormat internalFormat, PixelFormat format, uint width, uint height, int index)
        {
            bool multisampled = samples > 1;
            if (multisampled)
            {
                GL.TexImage2DMultisample(OpenTK.Graphics.OpenGL.TextureTarget.Texture2dMultisample, samples, internalFormat, (int)width, (int)height, false);
            }
            else
            {
                GL.TexImage2D(OpenTK.Graphics.OpenGL.TextureTarget.Texture2d, 0, (int)internalFormat, (int)width, (int)height, 0, format, PixelType.UnsignedByte, IntPtr.Zero);

                GL.TexParameteri(OpenTK.Graphics.OpenGL.TextureTarget.Texture2d, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Linear);
                GL.TexParameteri(OpenTK.Graphics.OpenGL.TextureTarget.Texture2d, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Linear);
                GL.TexParameteri(OpenTK.Graphics.OpenGL.TextureTarget.Texture2d, TextureParameterName.TextureWrapR, (int)TextureWrapMode.ClampToEdge);
                GL.TexParameteri(OpenTK.Graphics.OpenGL.TextureTarget.Texture2d, TextureParameterName.TextureWrapS, (int)TextureWrapMode.ClampToEdge);
                GL.TexParameteri(OpenTK.Graphics.OpenGL.TextureTarget.Texture2d, TextureParameterName.TextureWrapT, (int)TextureWrapMode.ClampToEdge);
            }

            GL.FramebufferTexture2D(FramebufferTarget.Framebuffer, (FramebufferAttachment)((uint)FramebufferAttachment.ColorAttachment0 + index), TextureTarget(multisampled), id, 0);
        }

        public static void AttachDepthTexture(TextureHandle id, int samples, InternalFormat format, FramebufferAttachment attachmentType, uint width, uint height)
        {
            bool multisampled = samples > 1;
            if (multisampled)
            {
                GL.TexImage2DMultisample(OpenTK.Graphics.OpenGL.TextureTarget.Texture2dMultisample, samples, format, (int)width, (int)height, false);
            }
            else
            {
                GL.TexStorage2D(OpenTK.Graphics.OpenGL.TextureTarget.Texture2d, 1, (SizedInternalFormat)format, (int)width, (int)height);

                GL.TexParameteri(OpenTK.Graphics.OpenGL.TextureTarget.Texture2d, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Linear);
                GL.TexParameteri(OpenTK.Graphics.OpenGL.TextureTarget.Texture2d, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Linear);
                GL.TexParameteri(OpenTK.Graphics.OpenGL.TextureTarget.Texture2d, TextureParameterName.TextureWrapR, (int)TextureWrapMode.ClampToEdge);
                GL.TexParameteri(OpenTK.Graphics.OpenGL.TextureTarget.Texture2d, TextureParameterName.TextureWrapS, (int)TextureWrapMode.ClampToEdge);
                GL.TexParameteri(OpenTK.Graphics.OpenGL.TextureTarget.Texture2d, TextureParameterName.TextureWrapT, (int)TextureWrapMode.ClampToEdge);
            }

            GL.FramebufferTexture2D(FramebufferTarget.Framebuffer, attachmentType, TextureTarget(multisampled), id, 0);
        }

        public static bool IsDepthFormat(FramebufferTextureFormat format)
        {
            switch (format)
            {
                case FramebufferTextureFormat.DEPTH24STENCIL8: return true;
            }

            return false;
        }

        public static PixelFormat HazelFBTextureFormatToGL(FramebufferTextureFormat format)
        {
            switch (format)
            {
                case FramebufferTextureFormat.RGBA8: return PixelFormat.Rgba;
                case FramebufferTextureFormat.RED_INTEGER: return PixelFormat.RedInteger;
            }

            throw new ArgumentException("Unknown format");
        }

        // --------------------------------------

        public static ShaderType ShaderTypeFromString(string type)
        {
            switch (type)
            {
                case "vertex":
                    return ShaderType.VertexShader;
                case "fragment":
                case "pixel":
                    return ShaderType.FragmentShader;
                default:
                    throw new ArgumentException("Unknown shader type!");
            }
        }

        public static uint GLShaderStageToShaderC(ShaderType stage)
        {
            switch (stage)
            {
                case ShaderType.VertexShader:
                    return 0; //shaderc_glsl_vertex_shader;
                case ShaderType.FragmentShader:
                    return 1; // shaderc_glsl_fragment_shader;
                default:
                    throw new ArgumentException("Unknown stage type!");
            }
        }

        public static string GLShaderStageToString(ShaderType stage)
        {
            switch (stage)
            {
                case ShaderType.VertexShader:
                    return "GL_VERTEX_SHADER";
                case ShaderType.FragmentShader:
                    return "GL_FRAGMENT_SHADER";
                default:
                    throw new ArgumentException("Unknown stage type!");
            }
        }

        public static string GetCacheDirectory()
        {
            // TODO: make sure the assets directory is valid
            return "assets/cache/shader/opengl";
        }

        public static void CreateCacheDirectoryIfNeeded()
        {
            string cacheDirectory = GetCacheDirectory();
            if (!File.Exists(cacheDirectory))
                Directory.CreateDirectory(cacheDirectory);
        }

        public static string GLShaderStageCachedOpenGLFileExtension(ShaderType stage)
        {
            switch (stage)
            {
                case ShaderType.VertexShader:
                    return ".cached_opengl.vert";
                case ShaderType.FragmentShader:
                    return ".cached_opengl.frag";
                default:
                    throw new ArgumentException("Unknown stage type!");
            }
        }

        public static string GLShaderStageCachedVulkanFileExtension(ShaderType stage)
        {
            switch (stage)
            {
                case ShaderType.VertexShader:
                    return ".cached_vulkan.vert";
                case ShaderType.FragmentShader:
                    return ".cached_vulkan.frag";
                default:
                    throw new ArgumentException("Unknown stage type!");
            }
        }
        
        public static void ExecuteCmd(IEnumerable<string> commands)
        {
            StringBuilder outputBuilder = new StringBuilder();

            ProcessStartInfo startInfo = new ProcessStartInfo("cmd")
            {
                RedirectStandardInput = true,
                RedirectStandardOutput = true,
                CreateNoWindow = true,
                UseShellExecute = false
            };
            
            var process = new Process { StartInfo = startInfo, EnableRaisingEvents = true };
            process.OutputDataReceived += (sender, eventArgs) => outputBuilder.AppendLine(eventArgs.Data);
            process.Start();
            process.BeginOutputReadLine();

            foreach (var command in commands)
            {
                process.StandardInput.WriteLine(command);
            }

            // Force quit
            process.StandardInput.WriteLine("exit /c");

            process.StandardInput.Flush();
            process.StandardInput.Close();

            AutoResetEvent resetEvent = new AutoResetEvent(false);
            process.Exited += (sender, args) => { resetEvent.Set(); };
            resetEvent.WaitOne();

            // for debug purposes
            string output = outputBuilder.ToString();
        }
    }
}