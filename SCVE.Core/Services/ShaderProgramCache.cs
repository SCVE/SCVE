using System.Collections.Generic;
using SCVE.Core.App;
using SCVE.Core.Rendering;

namespace SCVE.Core.Services
{
    public class ShaderProgramCache
    {
        private Dictionary<string, Program> _programs = new();
        
        public Program LoadOrCache(string name)
        {
            if (_programs.ContainsKey(name)) return _programs[name];
            
            var program = Application.Instance.FileLoader.LoadProgram(name);
            _programs.Add(name, program);
            return program;
        }
    }
}