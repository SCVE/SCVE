using System.Collections.Generic;
using SCVE.Core.Entities;
using SCVE.Core.Lifecycle;

namespace SCVE.Core.Services
{
    public abstract class WindowManager : IInitable, ITerminatable
    {
        public ScveWindow MainWindow { get; protected set; }
        protected readonly List<ScveWindow> Windows;

        protected WindowManager()
        {
            Windows = new();
        }

        public abstract ScveWindow Create(WindowProps props);
        
        public abstract void Init();

        public abstract void PollEvents();
        
        public abstract bool WindowShouldClose(ScveWindow window);
        
        public abstract void Close(ScveWindow window);

        public virtual void WindowSetMain(ScveWindow window)
        {
            MainWindow = window;
        }

        public abstract void Terminate();
    }
}