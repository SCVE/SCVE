using System;
using SCVE.Editor.Abstractions;

namespace SCVE.Editor.ImGuiUi.Panels
{
    public abstract class ImGuiModalPanel : IImGuiPanel
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