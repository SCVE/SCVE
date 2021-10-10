using System;
using SCVE.Core.Entities;

namespace SCVE.OpenTK.Glfw
{
    public class GlfwWindow : ScveWindow
    {
        public GlfwWindow(string title, int width, int height, bool isMain, IntPtr handle) : base(title, width, height, isMain, handle)
        {
        }
    }
}