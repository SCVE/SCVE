using Engine.EngineCore.Events;

namespace Engine.EngineCore.Core
{
    public abstract class Application
    {
        // --- public

        public Application(string name = "Hazel App", ApplicationCommandLineArgs args = new())
        {
        }

        public void OnEvent(Event e)
        {
        }

        public void PushLayer(Layer layer)
        {
        }

        public void PushOverlay(Layer layer)
        {
        }

        public Window GetWindow() { return _window; }

        public void Close()
        {
        }

        // public ImGuiLayer GetImGuiLayer() { return _imguiLayer; }

        public static Application Get()
        {
            return _instance;
        }

        public ApplicationCommandLineArgs GetCommandLineArgs()
        {
            return _commandLineArgs;
        }

        public abstract Application CreateApplication();

        // --- private

        public void Run()
        {
        }

        private bool OnWindowClose(WindowCloseEvent e)
        {
            return false;
        }

        private bool OnWindowResize(WindowResizeEvent e)
        {
            return false;
        }

        private ApplicationCommandLineArgs _commandLineArgs;

        private Window _window;

        // private ImGuiLayer _imguiLayer;

        private bool _running;

        private bool _minimized;

        private LayerStack _layerStack;

        private float lastFrameTime = 0f;

        private static Application _instance;
    }
}