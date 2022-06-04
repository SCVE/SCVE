using System.Numerics;
using ImGuiNET;

namespace SCVE.Editor.ImGuiUi
{
    public class ImGuiThemeMicrosoft
    {
        public static void Apply()
        {
            var style = ImGui.GetStyle();

            int hspacing = 8;
            int vspacing = 6;
            style.DisplaySafeAreaPadding = new Vector2(0, 0);
            style.WindowPadding = new Vector2(hspacing / 2, vspacing);
            style.FramePadding = new Vector2(hspacing, vspacing);
            style.ItemSpacing = new Vector2(hspacing, vspacing);
            style.ItemInnerSpacing = new Vector2(hspacing, vspacing);
            style.IndentSpacing = 20.0f;

            style.WindowRounding = 0.0f;
            style.FrameRounding = 0.0f;

            style.WindowBorderSize = 0.0f;
            style.FrameBorderSize = 1.0f;
            style.PopupBorderSize = 1.0f;

            style.ScrollbarSize = 20.0f;
            style.ScrollbarRounding = 0.0f;
            style.GrabMinSize = 5.0f;
            style.GrabRounding = 0.0f;

            Vector4 white = new Vector4(1.00f, 1.00f, 1.00f, 1.00f);
            Vector4 transparent = new Vector4(0.00f, 0.00f, 0.00f, 0.00f);
            Vector4 dark = new Vector4(0.00f, 0.00f, 0.00f, 0.20f);
            Vector4 darker = new Vector4(0.00f, 0.00f, 0.00f, 0.50f);

            Vector4 background = new Vector4(0.95f, 0.95f, 0.95f, 1.00f);
            Vector4 text = new Vector4(0.10f, 0.10f, 0.10f, 1.00f);
            Vector4 border = new Vector4(0.60f, 0.60f, 0.60f, 1.00f);
            Vector4 grab = new Vector4(0.69f, 0.69f, 0.69f, 1.00f);
            Vector4 header = new Vector4(0.86f, 0.86f, 0.86f, 1.00f);
            Vector4 active = new Vector4(0.00f, 0.47f, 0.84f, 1.00f);
            Vector4 hover = new Vector4(0.00f, 0.47f, 0.84f, 0.20f);

            style.Colors[(int)ImGuiCol.Text] = text;
            style.Colors[(int)ImGuiCol.WindowBg] = background;
            style.Colors[(int)ImGuiCol.ChildBg] = background;
            style.Colors[(int)ImGuiCol.PopupBg] = white;

            style.Colors[(int)ImGuiCol.Border] = border;
            style.Colors[(int)ImGuiCol.BorderShadow] = transparent;

            style.Colors[(int)ImGuiCol.Button] = header;
            style.Colors[(int)ImGuiCol.ButtonHovered] = hover;
            style.Colors[(int)ImGuiCol.ButtonActive] = active;

            style.Colors[(int)ImGuiCol.FrameBg] = white;
            style.Colors[(int)ImGuiCol.FrameBgHovered] = hover;
            style.Colors[(int)ImGuiCol.FrameBgActive] = active;

            style.Colors[(int)ImGuiCol.MenuBarBg] = header;
            style.Colors[(int)ImGuiCol.Header] = header;
            style.Colors[(int)ImGuiCol.HeaderHovered] = hover;
            style.Colors[(int)ImGuiCol.HeaderActive] = active;

            style.Colors[(int)ImGuiCol.CheckMark] = text;
            style.Colors[(int)ImGuiCol.SliderGrab] = grab;
            style.Colors[(int)ImGuiCol.SliderGrabActive] = darker;

            // style.Colors[ImGuiCol.CloseButton] = transparent;
            // style.Colors[ImGuiCol.CloseButtonHovered] = transparent;
            // style.Colors[ImGuiCol.CloseButtonActive] = transparent;

            style.Colors[(int)ImGuiCol.ScrollbarBg] = header;
            style.Colors[(int)ImGuiCol.ScrollbarGrab] = grab;
            style.Colors[(int)ImGuiCol.ScrollbarGrabHovered] = dark;
            style.Colors[(int)ImGuiCol.ScrollbarGrabActive] = darker;
        }
    }
}