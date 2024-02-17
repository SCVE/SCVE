using ImGuiNET;
using Silk.NET.Input;
using Silk.NET.OpenGL;

namespace SCVE.Bootstrap;

public abstract class BootstrappedApplication
{
    public GL GL { get; private set; }
    public ImFontPtr OpenSansFont { get; private set; }
    public abstract void OnImGuiRender();

    public virtual void Init(GL openGl, ImFontPtr openSansFont)
    {
        GL = openGl;
        OpenSansFont = openSansFont;
    }

    public abstract void OnKeyDown(Key key);
    public abstract void OnKeyPressed(Key key);
    public abstract void OnKeyReleased(Key key);
    public abstract void Update(double delta);
    public abstract void Exit();
}