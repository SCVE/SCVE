using System;
using System.IO;
using System.Numerics;
using ImGuiNET;
using SCVE.Editor.Editing.ProjectStructure;
using SCVE.Engine.ImageSharpBindings;
using Silk.NET.OpenGL;

namespace SCVE.Editor.ImGuiUi
{
    public abstract class AssetPreviewLayout : IImGuiRenderable
    {
        public abstract void SetFromAsset(AssetBase asset);

        public abstract void OnImGuiRender();
        public abstract void DisposeResources();
    }

    public class UnknownTypeAssetPreviewLayout : AssetPreviewLayout
    {
        private string _text;

        public override void SetFromAsset(AssetBase asset)
        {
            _text = $"Asset \"{asset.Name}\" is not currently supported for preview\n";
        }

        public override void OnImGuiRender()
        {
            ImGui.TextUnformatted(_text);
        }

        public override void DisposeResources()
        {
        }
    }
}