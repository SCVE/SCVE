using System;
using System.Collections.Generic;
using Engine.Platform.OpenGL;
using OpenTK.Mathematics;

namespace Engine.EngineCore.Renderer
{
    public abstract class Shader
    {
        public abstract void Bind();
        public abstract void Unbind();
        public abstract void SetInt(string name, int value);
        public abstract unsafe void SetIntArray(string name, int* values, uint count);
        public abstract void SetFloat(string name, float value);
        public abstract void SetFloat2(string name, Vector2 value);
        public abstract void SetFloat3(string name, Vector3 value);
        public abstract void SetFloat4(string name, Vector4 value);
        public abstract void SetMat4(string name, Matrix4 value);
        public abstract string GetName();

        public static Shader Create(string filepath)
        {
            switch (Renderer.GetAPI())
            {
                case RendererAPI.API.None: throw new ArgumentException("RendererAPI::None is currently not supported!");
                case RendererAPI.API.OpenGL:  return new OpenGLShader(filepath);
                default:
                    throw new ArgumentOutOfRangeException("Unknown RendererAPI!");
            }
        }
        
        public static Shader Create(string name, string vertexSrc, string fragmentSrc)
        {
            switch (Renderer.GetAPI())
            {
                case RendererAPI.API.None: throw new ArgumentException("RendererAPI::None is currently not supported!");
                case RendererAPI.API.OpenGL:  return new OpenGLShader(name, vertexSrc, fragmentSrc);
                default:
                    throw new ArgumentOutOfRangeException("Unknown RendererAPI!");
            }
        }
    }

    public class ShaderLibrary
    {
        public void Add(string name, Shader shader)
        {
            if (Exists(name))
            {
                throw new ArgumentException("Shader already exists!");
            }

            _shaders[name] = shader;
        }
        
        public void Add(Shader shader)
        {
            var name = shader.GetName();
            
            Add(name, shader);
        }
        
        public Shader Load(string filepath)
        {
            var shader = Shader.Create(filepath);
            Add(shader);
            return shader;
        }
        
        public Shader Load(string name, string filepath)
        {
            var shader = Shader.Create(filepath);
            Add(name, shader);
            return shader;
        }
        
        public Shader Get(string name)
        {
            if (!Exists(name))
            {
                throw new ArgumentException("Shader not found!");
            }

            return _shaders[name];
        }
        
        public bool Exists(string name)
        {
            return _shaders.ContainsKey(name);
        }

        private Dictionary<string, Shader> _shaders = new();
    }
}