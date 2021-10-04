using Engine.EngineCore.Events;

namespace Engine.EngineCore.Core
{
    public class Layer
    {
        public Layer(string name)
        {
            _debugName = name;
        }
        
        public virtual void OnAttach() {}
        public virtual void OnDetach() {}
        public virtual void OnUpdate(Timestep ts) {}
        public virtual void OnImGuiRender() {}
        public virtual void OnEvent(Event @event) {}

        public string GetName() { return _debugName; }

        private string _debugName;
    }
}