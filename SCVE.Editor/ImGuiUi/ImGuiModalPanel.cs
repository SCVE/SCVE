using System;

namespace SCVE.Editor.ImGuiUi
{
    public abstract class ImGuiModalPanel : IImGuiRenderable
    {
        protected bool IsOpen;
        
        public string Name { get; set; }

        public void Open()
        {
            IsOpen = true;
        }
        
        protected void Close()
        {
            IsOpen = false;
            Closed?.Invoke();
        }

        public event Action Closed;
        public abstract void OnImGuiRender();
    }
}