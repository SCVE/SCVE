using System.Numerics;
using ImGuiNET;
using SCVE.Editor.Editing;
using SCVE.Editor.Editing.Editing;

namespace SCVE.Editor.ImGuiUi
{
    public class ClipImGuiRenderer
    {
        public void Render(ref ImDrawListPtr painter, Clip clip, Vector2 topLeft, Vector2 bottomRight)
        {
            var mousePos = ImGui.GetMousePos();

            // clip outer rect
            painter.AddRectFilled(
                topLeft,
                bottomRight,
                0xFFAAAAAA,
                Settings.Instance.ClipRounding
            );

            // clip border
            painter.AddRect(
                topLeft,
                bottomRight,
                0xFF000000,
                Settings.Instance.ClipRounding
            );

            // clip inner rect
            if (topLeft.X < mousePos.X && bottomRight.X > mousePos.X &&
                topLeft.Y < mousePos.Y && bottomRight.Y > mousePos.Y)
            {
                painter.AddRectFilled(
                    topLeft + new Vector2(Settings.Instance.ClipPadding),
                    bottomRight - new Vector2(Settings.Instance.ClipPadding),
                    0xFFDDDDDD,
                    Settings.Instance.ClipRounding
                );
            }
            else
            {
                painter.AddRectFilled(
                    topLeft + new Vector2(Settings.Instance.ClipPadding),
                    bottomRight - new Vector2(Settings.Instance.ClipPadding),
                    0xFFAAAAAA,
                    Settings.Instance.ClipRounding
                );
            }

            var clipTextSize = ImGui.CalcTextSize(clip.ShortName());
            painter.AddText(
                topLeft + new Vector2(Settings.Instance.ClipPadding + Settings.Instance.ClipRounding, Settings.Instance.ClipPadding) +
                new Vector2(0, ((bottomRight.Y - Settings.Instance.ClipPadding) - (topLeft.Y + Settings.Instance.ClipPadding)) / 2 - clipTextSize.Y / 2),
                0xFFFFFFFF,
                clip.ShortName()
            );
        }

        public void RenderGhost(ref ImDrawListPtr painter, GhostClip clip, ref Vector2 topLeft, ref Vector2 bottomRight)
        {
            painter.AddRectFilled(
                topLeft,
                bottomRight,
                0x88888888,
                Settings.Instance.ClipRounding
            );

            // clip border
            painter.AddRect(
                topLeft,
                bottomRight,
                0xFFAAAAAA,
                Settings.Instance.ClipRounding
            );

            var clipTextSize = ImGui.CalcTextSize(clip.ShortName());
            painter.AddText(
                topLeft + new Vector2(Settings.Instance.ClipPadding + Settings.Instance.ClipRounding, Settings.Instance.ClipPadding) +
                new Vector2(0, ((bottomRight.Y - Settings.Instance.ClipPadding) - (topLeft.Y + Settings.Instance.ClipPadding)) / 2 - clipTextSize.Y / 2),
                0xFF000000,
                clip.ShortName()
            );
        }
    }
}