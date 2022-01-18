using System.Collections.Generic;
using System.Linq;
using SCVE.Engine.Core.Misc;

namespace SCVE.Editor.Modules
{
    public class Modules
    {
        private List<IModule> _modules;

        public Modules()
        {
            _modules = new List<IModule>();
        }

        public void Update()
        {
            foreach (var module in _modules)
            {
                module.OnUpdate();
            }
        }

        public void Init()
        {
            foreach (var module in _modules)
            {
                module.Init();
            }
        }

        public void CrossReference()
        {
            foreach (var module in _modules)
            {
                module.CrossReference(this);
            }
        }

        public void Add<T>(T module) where T : IModule
        {
            if (Get<T>() is not null)
            {
                throw new ScveException($"Can't add a module of existing type: {module.GetType()}");
            }

            _modules.Add(module);
        }

        public T Get<T>() where T : IModule
        {
            return (T)_modules.FirstOrDefault(m => m.GetType() == typeof(T));
        }
    }
}