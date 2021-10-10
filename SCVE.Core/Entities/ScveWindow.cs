using System;
using SCVE.Core.App;
using SCVE.Core.Utilities;

namespace SCVE.Core.Entities
{
    public abstract class ScveWindow
    {
        public IntPtr Handle { get; private set; }
        
        public string Title { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public bool IsMain { get; set; }

        protected ScveWindow(string title, int width, int height, bool isMain, IntPtr handle)
        {
            Title = title;
            Width = width;
            Height = height;
            IsMain = isMain;
            Handle = handle;
        }

        public bool ShouldClose()
        {
            return Application.Instance.WindowManager.WindowShouldClose(this);
        }

        public void SwapBuffers()
        {
            Application.Instance.WindowManager.SwapBuffers(this);
        }

        public void OnClose()
        {
            Logger.Warn($"Window ({Title}): OnClose()");
        }
    }
}