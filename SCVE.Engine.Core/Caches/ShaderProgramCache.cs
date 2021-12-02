using System.Collections.Generic;
using SCVE.Engine.Core.Main;
using SCVE.Engine.Core.Rendering;

namespace SCVE.Engine.Core.Caches
{
    public class ShaderProgramCache
    {
        private Dictionary<string, ShaderProgram> _programs = new();
        
        public ShaderProgram LoadOrCache(string name)
        {
            if (_programs.ContainsKey(name)) return _programs[name];
            
            var program = Main.ScveEngine.Instance.FileLoaders.ShaderProgram.Load(name);
            _programs.Add(name, program);
            return program;
        }
    }
}