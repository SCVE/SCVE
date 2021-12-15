﻿using System.Drawing;
using ImGuiNET;
using Silk.NET.GLFW;
using Silk.NET.Input;
using Silk.NET.OpenGL;
using Silk.NET.OpenGL.Extensions.ImGui;
using Silk.NET.Windowing;

namespace SCVE.Editor
{
    public static class Program
    {
        private static void Main(string[] args)
        {
            // Create a Silk.NET window as usual
            using var window = Window.Create(WindowOptions.Default);
            
            // Declare some variables
            ImGuiController controller   = null;
            GL              gl           = null;
            IInputContext   inputContext = null;

            var editorApp = new EditorApp();

            // Our loading function
            window.Load += () =>
            {
                controller = new ImGuiController(
                    gl = window.CreateOpenGL(), // load OpenGL
                    window, // pass in our window
                    inputContext = window.CreateInput(), // create an input context
                    () =>
                    {
                        var io = ImGui.GetIO();
                        io.ConfigFlags |= ImGuiConfigFlags.DockingEnable;
                        editorApp.openSansFont = io.Fonts.AddFontFromFileTTF("assets/Font/OpenSans-Regular.ttf", 18);
                    }
                );
            };

            // Handle resizes
            window.FramebufferResize += s =>
            {
                // Adjust the viewport to the new window size
                gl.Viewport(s);
            };

            // The render function
            window.Render += delta =>
            {
                // Make sure ImGui is up-to-date
                controller.Update((float)delta);

                // This is where you'll do any rendering beneath the ImGui context
                // Here, we just have a blank screen.
                gl.ClearColor(Color.FromArgb(255, (int)(.45f * 255), (int)(.55f * 255), (int)(.60f * 255)));
                gl.Clear((uint)ClearBufferMask.ColorBufferBit);

                editorApp.OnImGuiRender();

                // Make sure ImGui renders too!
                controller.Render();
            };
            
            // The closing function
            window.Closing += () =>
            {
                ImGui.SaveIniSettingsToDisk("imgui.ini");
                // Dispose our controller first
                controller?.Dispose();

                // Dispose the input context
                inputContext?.Dispose();

                // Unload OpenGL
                gl?.Dispose();
            };

            // Now that everything's defined, let's run this bad boy!
            window.Run();
        }
    }
}