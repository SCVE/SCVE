using Engine.EngineCore.Events;
using OpenTK.Windowing.GraphicsLibraryFramework;

namespace Engine.EngineCore.Core
{
    public abstract class Application
    {
        // --- public

        public Application(string name = "Hazel App", ApplicationCommandLineArgs args = new())
        {
            _commandLineArgs = args;
            _instance = this;
            _window = Window.Create(new WindowProps(name));
            _window.SetEventCallback(OnEvent);
            
            Renderer.Renderer.Init();
            
            // _imguiLayer = new ImGuiLayer();
            // PushOverlay(_imguiLayer);
        }

        ~Application()
        {
            Renderer.Renderer.Shutdown();
        }

        public void OnEvent(Event e)
        {

            EventDispatcher dispatcher = new(e);
            dispatcher.Dispatch<WindowCloseEvent>(OnWindowClose);
            dispatcher.Dispatch<WindowResizeEvent>(OnWindowResize);

            foreach (var layer in _layerStack)
            {
                if (e.Handled) 
                    break;
                layer.OnEvent(e);
            }
        }

        public void PushLayer(Layer layer)
        {
            _layerStack.PushLayer(layer);
            layer.OnAttach();
        }

        public void PushOverlay(Layer layer)
        {
            _layerStack.PushOverlay(layer);
            layer.OnAttach();
        }

        public Window GetWindow() { return _window; }

        public void Close()
        {
            _running = false;
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
            while (_running)
            {
                float time = (float)GLFW.GetTime();
                Timestep timestep = time - _lastFrameTime;
                _lastFrameTime = time;

                if (!_minimized)
                {
                    {
                        foreach (var layer in _layerStack)
                            layer.OnUpdate(timestep);
                    }

                    // _imguiLayer->Begin();
                    // {
                    //     foreach (Layer layer in _layerStack)
                    //         layer.OnImGuiRender();
                    // }
                    // _imguiLayer->End();
                }

                _window.OnUpdate();
            }
        }

        private bool OnWindowClose(WindowCloseEvent e)
        {
            _running = false;
            return true;
        }

        private bool OnWindowResize(WindowResizeEvent e)
        {
            if (e.GetWidth() == 0 || e.GetHeight() == 0)
            {
                _minimized = true;
                return false;
            }

            _minimized = false;
            Renderer.Renderer.OnWindowResize(e.GetWidth(), e.GetHeight());

            return false;
        }

        private ApplicationCommandLineArgs _commandLineArgs;

        private Window _window;

        // private ImGuiLayer _imguiLayer;

        private bool _running;

        private bool _minimized;

        private LayerStack _layerStack;

        private float _lastFrameTime = 0f;

        private static Application _instance;
    }
}