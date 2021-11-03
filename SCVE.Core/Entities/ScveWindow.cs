using System;
using SCVE.Core.Rendering;
using SCVE.Core.Utilities;

namespace SCVE.Core.Entities
{
    public abstract class ScveWindow
    {
        public IntPtr Handle { get; protected set; }

        public string Title { get; protected set; }
        public int Width { get; protected set; }
        public int Height { get; protected set; }

        public Context Context { get; protected set; }
        public static ScveWindow Instance { get; set; }

        protected ScveWindow(WindowProps props)
        {
            Title = props.Title;
            Width = props.Width;
            Height = props.Height;
            Instance = this;
        }

        public abstract void SetVSync(bool vSync);
        
        public abstract void Shutdown();

        public virtual void SetTitle(string title)
        {
            this.Title = title;
        }

        public void SwapBuffers()
        {
            Context.SwapBuffers();
        }

        public void OnClose()
        {
            Logger.Warn($"Window ({Title}) closing");
        }

        public abstract void OnUpdate();
    }
}