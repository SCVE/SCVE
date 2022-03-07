using System;

namespace SCVE.Editor.ImGuiUi
{
    public abstract class ImGuiModalPanel : IImGuiRenderable
    {
        protected bool IsOpen;

        public string Name { get; set; }

        private Action _closed;
        private Action _dismissed;

        public void Open(Action closed, Action dismissed)
        {
            IsOpen = true;
            _closed = closed;
            _dismissed = dismissed;
        }

        protected void Close()
        {
            IsOpen = false;
            _closed?.Invoke();
        }

        protected void Dismiss()
        {
            IsOpen = false;
            _dismissed?.Invoke();
        }

        public abstract void OnImGuiRender();
    }
}