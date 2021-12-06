using System;
using System.Collections.Generic;
using System.Linq;

namespace SCVE.Engine.Core.Input
{
    public abstract class EngineInput
    {
        public event Action<int, int> WindowSizeChanged;
        public event Action<KeyCode> KeyDown;
        public event Action<KeyCode> KeyUp;
        public event Action<KeyCode> KeyRepeat;
        public event Action WindowMaximized;
        public event Action WindowWindowed;
        public event Action WindowMinimized;
        public event Action CursorEnter;
        public event Action CursorLeave;
        public event Action<float, float> CursorMoved;
        public event Action<float, float> Scroll;
        public event Action<MouseCode> MouseButtonDown;
        public event Action<MouseCode> MouseButtonUp;

        public event Action<char> CharTyped;

        private int _windowSizeX;
        private int _windowSizeY;

        private float _cursorPositionX;
        private float _cursorPositionY;

        public IReadOnlyDictionary<KeyCode, bool> KeyboardState => _keyboardState;
        public IReadOnlyDictionary<MouseCode, bool> MouseState => _mouseState;

        private Dictionary<KeyCode, bool> _keyboardState;
        private Dictionary<MouseCode, bool> _mouseState;

        protected EngineInput()
        {
            _keyboardState = new(Enum.GetValues<KeyCode>().Distinct().ToDictionary(x => x, _ => false));
            _mouseState    = new(Enum.GetValues<MouseCode>().Distinct().ToDictionary(x => x, _ => false));
        }

        public virtual float GetWindowSizeX()
        {
            return _windowSizeX;
        }

        public virtual float GetWindowSizeY()
        {
            return _windowSizeY;
        }

        public virtual float GetCursorX()
        {
            return _cursorPositionX;
        }

        public virtual float GetCursorY()
        {
            return _cursorPositionY;
        }

        public virtual void RegisterWindowSizeChanged(int width, int height)
        {
            _windowSizeX = width;
            _windowSizeY = height;
            WindowSizeChanged?.Invoke(width, height);
        }

        public virtual void RegisterKeyDown(KeyCode code)
        {
            _keyboardState[code] = true;
            KeyDown?.Invoke(code);
        }

        public virtual void RegisterKeyUp(KeyCode code)
        {
            _keyboardState[code] = false;
            KeyUp?.Invoke(code);
        }

        public virtual void RegisterKeyRepeat(KeyCode code)
        {
            KeyRepeat?.Invoke(code);
        }

        public virtual void RegisterWindowMaximized()
        {
            WindowMaximized?.Invoke();
        }

        public virtual void RegisterWindowWindowed()
        {
            WindowWindowed?.Invoke();
        }

        public virtual void RegisterWindowMinimized()
        {
            WindowMinimized?.Invoke();
        }

        public virtual void RegisterCursorEnter()
        {
            CursorEnter?.Invoke();
        }

        public virtual void RegisterCursorLeave()
        {
            CursorLeave?.Invoke();
        }

        public virtual void RegisterCursorMoved(float x, float y)
        {
            _cursorPositionX = x;
            _cursorPositionY = y;
            CursorMoved?.Invoke(x, y);
        }

        public virtual void RegisterScroll(float offsetx, float offsety)
        {
            Scroll?.Invoke(offsetx, offsety);
        }

        public virtual void RegisterMouseButtonDown(MouseCode code)
        {
            _mouseState[code] = true;
            MouseButtonDown?.Invoke(code);
        }

        public virtual void RegisterMouseButtonUp(MouseCode code)
        {
            _mouseState[code] = false;
            MouseButtonUp?.Invoke(code);
        }

        public void RegisterCharTyped(char code)
        {
            CharTyped?.Invoke(code);
        }
    }
}