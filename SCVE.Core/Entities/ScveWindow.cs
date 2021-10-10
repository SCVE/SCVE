using System;
using SCVE.Core.App;

namespace SCVE.Core.Entities
{
    public abstract class ScveWindow
    {
        public IntPtr Handle { get; private set; }
        
        public string Title { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }

        protected ScveWindow(string title, int width, int height, IntPtr handle)
        {
            Title = title;
            Width = width;
            Height = height;
            Handle = handle;
        }

        public bool ShouldClose()
        {
            return Application.Instance.WindowManager.WindowShouldClose(this);
        }

        public void Close()
        {
            Application.Instance.WindowManager.Close(this);
        }
    }
}