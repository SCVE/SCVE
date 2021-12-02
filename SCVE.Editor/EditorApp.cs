using Dear_ImGui_Sample;
using ImGuiNET;
using OpenTK.Mathematics;
using SCVE.Engine.Core.Input;
using SCVE.Engine.Core.Lifecycle;
using SCVE.Engine.Core.Main;
using SCVE.Engine.Core.Rendering;

namespace SCVE.Editor
{
    public class EditorApp : IEngineRunnable
    {
        ImGuiController _controller;

        public void Init()
        {
            ScveEngine.Instance.Input.WindowSizeChanged += InputOnWindowSizeChanged;
            ScveEngine.Instance.Input.Scroll += InputOnScroll;
            ScveEngine.Instance.Input.KeyDown += InputOnKeyDown;

            _controller = new ImGuiController(ScveEngine.Instance.MainWindow.Width, ScveEngine.Instance.MainWindow.Height);
        }

        private void InputOnKeyDown(KeyCode obj)
        {
            _controller.PressChar((char)obj);
        }

        private void InputOnScroll(float arg1, float arg2)
        {
            _controller.MouseScroll(new Vector2(arg1, arg2));
        }

        private void InputOnWindowSizeChanged(int width, int height)
        {
            // Tell ImGui of the new size
            _controller.WindowResized(width, height);
        }

        public void Render(IRenderer renderer)
        {
            ImGui.ShowDemoWindow();
            _controller.Render();
        }

        public void Update(float deltaTime)
        {
            _controller.Update(deltaTime);
        }
    }
}