using System;
using System.Numerics;
using ImGuiNET;

namespace SCVE.Editor.ImGuiUi
{
    public class ImGuiThemeAdobeSpectrum
    {
        private static uint NONE = 0x00000000; // transparent

        private class SpectrumDefaultTheme
        {
            public uint WHITE = Color(0xFFFFFF);
            public uint BLACK = Color(0x000000);
            public uint GRAY50 = Color(0xFFFFFF);
            public uint GRAY75 = Color(0xFAFAFA);
            public uint GRAY100 = Color(0xF5F5F5);
            public uint GRAY200 = Color(0xF4F4F4);
            public uint GRAY300 = Color(0xEAEAEA);
            public uint GRAY400 = Color(0xD3D3D3);
            public uint GRAY500 = Color(0xBCBCBC);
            public uint GRAY600 = Color(0x959595);
            public uint GRAY700 = Color(0x767676);
            public uint GRAY800 = Color(0x505050);
            public uint GRAY900 = Color(0x323232);
            public uint BLUE400 = Color(0x378EF0);
            public uint BLUE500 = Color(0x2680EB);
            public uint BLUE600 = Color(0x1473E6);
            public uint BLUE700 = Color(0x0D66D0);
            public uint RED400 = Color(0xEC5B62);
            public uint RED500 = Color(0xE34850);
            public uint RED600 = Color(0xD7373F);
            public uint RED700 = Color(0xC9252D);
            public uint ORANGE400 = Color(0xF29423);
            public uint ORANGE500 = Color(0xE68619);
            public uint ORANGE600 = Color(0xDA7B11);
            public uint ORANGE700 = Color(0xCB6F10);
            public uint GREEN400 = Color(0x33AB84);
            public uint GREEN500 = Color(0x2D9D78);
            public uint GREEN600 = Color(0x268E6C);
            public uint GREEN700 = Color(0x12805C);

            public static SpectrumDefaultTheme Theme = new SpectrumDefaultTheme();
        }

        private static uint Color(uint c)
        {
            // add alpha.
            // also swap red and blue channel for some reason.
            // todo: figure out why, and fix it.
            uint a = 0xFF;
            uint r = (c >> 16) & 0xFF;
            uint g = (c >> 8) & 0xFF;
            uint b = (c >> 0) & 0xFF;
            return (a << 24)
                   | (r << 0)
                   | (g << 8)
                   | (b << 16);
        }

        private class SpectrumLightTheme
        {
            public uint GRAY50 = Color(0xFFFFFF);
            public uint GRAY75 = Color(0xFAFAFA);
            public uint GRAY100 = Color(0xF5F5F5);
            public uint GRAY200 = Color(0xEAEAEA);
            public uint GRAY300 = Color(0xE1E1E1);
            public uint GRAY400 = Color(0xCACACA);
            public uint GRAY500 = Color(0xB3B3B3);
            public uint GRAY600 = Color(0x8E8E8E);
            public uint GRAY700 = Color(0x707070);
            public uint GRAY800 = Color(0x4B4B4B);
            public uint GRAY900 = Color(0x2C2C2C);
            public uint BLUE400 = Color(0x2680EB);
            public uint BLUE500 = Color(0x1473E6);
            public uint BLUE600 = Color(0x0D66D0);
            public uint BLUE700 = Color(0x095ABA);
            public uint RED400 = Color(0xE34850);
            public uint RED500 = Color(0xD7373F);
            public uint RED600 = Color(0xC9252D);
            public uint RED700 = Color(0xBB121A);
            public uint ORANGE400 = Color(0xE68619);
            public uint ORANGE500 = Color(0xDA7B11);
            public uint ORANGE600 = Color(0xCB6F10);
            public uint ORANGE700 = Color(0xBD640D);
            public uint GREEN400 = Color(0x2D9D78);
            public uint GREEN500 = Color(0x268E6C);
            public uint GREEN600 = Color(0x12805C);
            public uint GREEN700 = Color(0x107154);
            public uint INDIGO400 = Color(0x6767EC);
            public uint INDIGO500 = Color(0x5C5CE0);
            public uint INDIGO600 = Color(0x5151D3);
            public uint INDIGO700 = Color(0x4646C6);
            public uint CELERY400 = Color(0x44B556);
            public uint CELERY500 = Color(0x3DA74E);
            public uint CELERY600 = Color(0x379947);
            public uint CELERY700 = Color(0x318B40);
            public uint MAGENTA400 = Color(0xD83790);
            public uint MAGENTA500 = Color(0xCE2783);
            public uint MAGENTA600 = Color(0xBC1C74);
            public uint MAGENTA700 = Color(0xAE0E66);
            public uint YELLOW400 = Color(0xDFBF00);
            public uint YELLOW500 = Color(0xD2B200);
            public uint YELLOW600 = Color(0xC4A600);
            public uint YELLOW700 = Color(0xB79900);
            public uint FUCHSIA400 = Color(0xC038CC);
            public uint FUCHSIA500 = Color(0xB130BD);
            public uint FUCHSIA600 = Color(0xA228AD);
            public uint FUCHSIA700 = Color(0x93219E);
            public uint SEAFOAM400 = Color(0x1B959A);
            public uint SEAFOAM500 = Color(0x16878C);
            public uint SEAFOAM600 = Color(0x0F797D);
            public uint SEAFOAM700 = Color(0x096C6F);
            public uint CHARTREUSE400 = Color(0x85D044);
            public uint CHARTREUSE500 = Color(0x7CC33F);
            public uint CHARTREUSE600 = Color(0x73B53A);
            public uint CHARTREUSE700 = Color(0x6AA834);
            public uint PURPLE400 = Color(0x9256D9);
            public uint PURPLE500 = Color(0x864CCC);
            public uint PURPLE600 = Color(0x7A42BF);
            public uint PURPLE700 = Color(0x6F38B1);

            public static SpectrumLightTheme Theme = new SpectrumLightTheme();
        }

        private static Vector4 ColorConvertU32ToFloat4(uint v) => ImGui.ColorConvertU32ToFloat4(v);

        public static void Apply()
        {
            var style = ImGui.GetStyle();
            style.GrabRounding = 4.0f;
            var colors = style.Colors;
            var theme = SpectrumDefaultTheme.Theme;
            colors[(int) ImGuiCol.Text] = ColorConvertU32ToFloat4(theme.GRAY800); // text on hovered controls is gray900
            colors[(int) ImGuiCol.TextDisabled] = ColorConvertU32ToFloat4(theme.GRAY500);
            colors[(int) ImGuiCol.WindowBg] = ColorConvertU32ToFloat4(theme.GRAY100);
            colors[(int) ImGuiCol.ChildBg] = new Vector4(0.00f, 0.00f, 0.00f, 0.00f);
            colors[(int) ImGuiCol.PopupBg] = ColorConvertU32ToFloat4(theme.GRAY50); // not sure about this. Note: applies to tooltips too.
            colors[(int) ImGuiCol.Border] = ColorConvertU32ToFloat4(theme.GRAY300);
            colors[(int) ImGuiCol.BorderShadow] = ColorConvertU32ToFloat4(NONE); // We don't want shadows. Ever.
            colors[(int) ImGuiCol.FrameBg] = ColorConvertU32ToFloat4(theme.GRAY75); // this isnt right, spectrum does not do this, but it's a good fallback
            colors[(int) ImGuiCol.FrameBgHovered] = ColorConvertU32ToFloat4(theme.GRAY50);
            colors[(int) ImGuiCol.FrameBgActive] = ColorConvertU32ToFloat4(theme.GRAY200);
            colors[(int) ImGuiCol.TitleBg] = ColorConvertU32ToFloat4(theme.GRAY300); // those titlebar values are totally made up, spectrum does not have this.
            colors[(int) ImGuiCol.TitleBgActive] = ColorConvertU32ToFloat4(theme.GRAY200);
            colors[(int) ImGuiCol.TitleBgCollapsed] = ColorConvertU32ToFloat4(theme.GRAY400);
            colors[(int) ImGuiCol.MenuBarBg] = ColorConvertU32ToFloat4(theme.GRAY100);
            colors[(int) ImGuiCol.ScrollbarBg] = ColorConvertU32ToFloat4(theme.GRAY100); // same as regular background
            colors[(int) ImGuiCol.ScrollbarGrab] = ColorConvertU32ToFloat4(theme.GRAY400);
            colors[(int) ImGuiCol.ScrollbarGrabHovered] = ColorConvertU32ToFloat4(theme.GRAY600);
            colors[(int) ImGuiCol.ScrollbarGrabActive] = ColorConvertU32ToFloat4(theme.GRAY700);
            colors[(int) ImGuiCol.CheckMark] = ColorConvertU32ToFloat4(theme.BLUE500);
            colors[(int) ImGuiCol.SliderGrab] = ColorConvertU32ToFloat4(theme.GRAY700);
            colors[(int) ImGuiCol.SliderGrabActive] = ColorConvertU32ToFloat4(theme.GRAY800);
            colors[(int) ImGuiCol.Button] = ColorConvertU32ToFloat4(theme.GRAY75); // match default button to Spectrum's 'Action Button'.
            colors[(int) ImGuiCol.ButtonHovered] = ColorConvertU32ToFloat4(theme.GRAY50);
            colors[(int) ImGuiCol.ButtonActive] = ColorConvertU32ToFloat4(theme.GRAY200);
            colors[(int) ImGuiCol.Header] = ColorConvertU32ToFloat4(theme.BLUE400);
            colors[(int) ImGuiCol.HeaderHovered] = ColorConvertU32ToFloat4(theme.BLUE500);
            colors[(int) ImGuiCol.HeaderActive] = ColorConvertU32ToFloat4(theme.BLUE600);
            colors[(int) ImGuiCol.Separator] = ColorConvertU32ToFloat4(theme.GRAY400);
            colors[(int) ImGuiCol.SeparatorHovered] = ColorConvertU32ToFloat4(theme.GRAY600);
            colors[(int) ImGuiCol.SeparatorActive] = ColorConvertU32ToFloat4(theme.GRAY700);
            colors[(int) ImGuiCol.ResizeGrip] = ColorConvertU32ToFloat4(theme.GRAY400);
            colors[(int) ImGuiCol.ResizeGripHovered] = ColorConvertU32ToFloat4(theme.GRAY600);
            colors[(int) ImGuiCol.ResizeGripActive] = ColorConvertU32ToFloat4(theme.GRAY700);
            colors[(int) ImGuiCol.PlotLines] = ColorConvertU32ToFloat4(theme.BLUE400);
            colors[(int) ImGuiCol.PlotLinesHovered] = ColorConvertU32ToFloat4(theme.BLUE600);
            colors[(int) ImGuiCol.PlotHistogram] = ColorConvertU32ToFloat4(theme.BLUE400);
            colors[(int) ImGuiCol.PlotHistogramHovered] = ColorConvertU32ToFloat4(theme.BLUE600);
            colors[(int) ImGuiCol.TextSelectedBg] = ColorConvertU32ToFloat4((theme.BLUE400 & 0x00FFFFFF) | 0x33000000);
            colors[(int) ImGuiCol.DragDropTarget] = new Vector4(1.00f, 1.00f, 0.00f, 0.90f);
            colors[(int) ImGuiCol.NavHighlight] = ColorConvertU32ToFloat4((theme.GRAY900 & 0x00FFFFFF) | 0x0A000000);
            colors[(int) ImGuiCol.NavWindowingHighlight] = new Vector4(1.00f, 1.00f, 1.00f, 0.70f);
            colors[(int) ImGuiCol.NavWindowingDimBg] = new Vector4(0.80f, 0.80f, 0.80f, 0.20f);
            colors[(int) ImGuiCol.ModalWindowDimBg] = new Vector4(0.20f, 0.20f, 0.20f, 0.35f);
        }
    }
}