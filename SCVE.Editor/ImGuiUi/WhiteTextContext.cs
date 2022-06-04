using System;
using System.Numerics;
using ImGuiNET;

namespace SCVE.Editor.ImGuiUi
{
    public class WhiteTextContext : IDisposable
    {
        private static readonly WhiteTextContext _instance = new();

        /// <summary>
        /// Only use with using(WhiteTextContext.Instance) {}
        /// </summary>
        public static WhiteTextContext Instance => _instance.Begin();

        private WhiteTextContext()
        {
        }

        private WhiteTextContext Begin()
        {
            ImGui.PushStyleColor(ImGuiCol.Text, Vector4.One);
            return this;
        }

        public void Dispose()
        {
            ImGui.PopStyleColor();
            GC.SuppressFinalize(this);
        }
    }
}