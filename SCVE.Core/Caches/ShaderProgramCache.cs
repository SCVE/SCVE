using System.Collections.Generic;
using SCVE.Core.App;
using SCVE.Core.Rendering;

namespace SCVE.Core.Caches
{
    public class ShaderProgramCache
    {
        private Dictionary<string, ShaderProgram> _programs = new();
        
        public ShaderProgram LoadOrCache(string name)
        {
            if (_programs.ContainsKey(name)) return _programs[name];
            
            var program = Application.Instance.FileLoaders.ShaderProgram.Load(name);
            _programs.Add(name, program);
            return program;
        }
    }
}