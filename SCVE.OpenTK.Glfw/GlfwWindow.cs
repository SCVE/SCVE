using System;
using SCVE.Core.App;
using SCVE.Core.Entities;

namespace SCVE.OpenTK.Glfw
{
    public class GlfwWindow : ScveWindow
    {
        public GlfwWindow(string title, int width, int height, IntPtr handle) : base(title, width, height, handle)
        {
        }
    }
}