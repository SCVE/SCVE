using Silk.NET.Input;

namespace SCVE.Editor.Abstractions
{
    public interface IKeyPressReceiver
    {
        void OnKeyPressed(Key key);
    }
}