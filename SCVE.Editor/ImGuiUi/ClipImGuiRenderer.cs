using System.Numerics;
using ImGuiNET;
using SCVE.Editor.Editing;
using SCVE.Editor.Editing.Editing;

namespace SCVE.Editor.ImGuiUi
{
    public class ClipImGuiRenderer
    {
        public void Render(ref ImDrawListPtr painter, Clip clip, ref Vector2 topLeft, ref Vector2 bottomRight)
        {
            var mousePos = ImGui.GetMousePos();

            var clipPadding  = 3;
            var clipRounding = 3;

            // clip outer rect
            painter.AddRectFilled(
                topLeft,
                bottomRight,
                0xFFAAAAAA,
                clipRounding
            );

            // clip border
            painter.AddRect(
                topLeft,
                bottomRight,
                0xFF000000,
                clipRounding
            );

            // clip inner rect
            if (topLeft.X < mousePos.X && bottomRight.X > mousePos.X &&
                topLeft.Y < mousePos.Y && bottomRight.Y > mousePos.Y)
            {
                painter.AddRectFilled(
                    topLeft + new Vector2(clipPadding),
                    bottomRight - new Vector2(clipPadding),
                    0xFFDDDDDD,
                    clipRounding
                );
            }
            else
            {
                painter.AddRectFilled(
                    topLeft + new Vector2(clipPadding),
                    bottomRight - new Vector2(clipPadding),
                    0xFFAAAAAA,
                    clipRounding
                );
            }

            var clipTextSize = ImGui.CalcTextSize($"{clip.ShortName()}");
            painter.AddText(
                topLeft + new Vector2(clipPadding + clipRounding, clipPadding) +
                new Vector2(0, ((bottomRight.Y - clipPadding) - (topLeft.Y + clipPadding)) / 2 - clipTextSize.Y / 2),
                0xFF000000,
                clip.ShortName()
            );
        }

        public void RenderGhost(ref ImDrawListPtr painter, Clip clip, ref Vector2 topLeft, ref Vector2 bottomRight)
        {
            var clipPadding  = 3;
            var clipRounding = 3;

            painter.AddRectFilled(
                topLeft,
                bottomRight,
                0xFF888888,
                clipRounding
            );

            // clip border
            painter.AddRect(
                topLeft,
                bottomRight,
                0xFFAAAAAA,
                clipRounding
            );

            var clipTextSize = ImGui.CalcTextSize($"{clip.ShortName()}");
            painter.AddText(
                topLeft + new Vector2(clipPadding + clipRounding, clipPadding) +
                new Vector2(0, ((bottomRight.Y - clipPadding) - (topLeft.Y + clipPadding)) / 2 - clipTextSize.Y / 2),
                0xFF000000,
                clip.ShortName()
            );
        }
    }
}