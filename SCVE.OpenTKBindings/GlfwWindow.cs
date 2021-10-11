using System;
using SCVE.Core.Entities;

namespace SCVE.OpenTKBindings
{
    public class GlfwWindow : ScveWindow
    {
        public GlfwWindow(string title, int width, int height, bool isMain, IntPtr handle) : base(title, width, height, isMain, handle)
        {
        }
    }
}