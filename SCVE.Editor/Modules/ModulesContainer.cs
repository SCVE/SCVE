﻿using System.Collections.Generic;
using System.Linq;
using SCVE.Engine.Core.Misc;

namespace SCVE.Editor.Modules
{
    public class ModulesContainer
    {
        private List<IModule> _modules;

        public ModulesContainer()
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
                module.OnInit();
            }
        }

        public void CrossReference()
        {
            foreach (var module in _modules)
            {
                module.CrossReference(this);
            }
        }

        public void Add<T>() where T : class, IModule, new()
        {
            if (Get<T>() is not null)
            {
                throw new ScveException($"Can't add a module of existing type: {typeof(T)}");
            }

            _modules.Add(new T());
        }

        public T Get<T>() where T : IModule
        {
            return (T)_modules.FirstOrDefault(m => m.GetType() == typeof(T));
        }
    }
}