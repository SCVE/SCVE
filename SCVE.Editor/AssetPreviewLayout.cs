using System;
using System.IO;
using System.Numerics;
using ImGuiNET;
using SCVE.Engine.ImageSharpBindings;

namespace SCVE.Editor
{
    public abstract class AssetPreviewLayout
    {
        public abstract void SetFromAsset(ProjectAsset asset);

        public abstract void OnImGuiRender();

        public abstract void DisposeResources();
    }

    public class TextAssetPreviewLayout : AssetPreviewLayout
    {
        private string _content;

        public override void SetFromAsset(ProjectAsset asset)
        {
            _content = File.ReadAllText(asset.FileSystemFullPath);
        }

        public override void OnImGuiRender()
        {
            ImGui.TextUnformatted(_content);
        }

        public override void DisposeResources()
        {
        }
    }

    public class UnknownTypeAssetPreviewLayout : AssetPreviewLayout
    {
        private string _text;

        public override void SetFromAsset(ProjectAsset asset)
        {
            _text = $"Asset of type {asset.Type} is not currently supported for preview\n" +
                    $"{asset.FileSystemFullPath}";
        }

        public override void OnImGuiRender()
        {
            ImGui.TextUnformatted(_text);
        }

        public override void DisposeResources()
        {
        }
    }

    public class ImageAssetPreviewLayout : AssetPreviewLayout
    {
        private Texture _texture;
        private int _imageWidth = 0;
        private int _imageHeight = 0;

        public override void SetFromAsset(ProjectAsset asset)
        {
            var textureFileData = new ImageSharpTextureLoader().Load(asset.FileSystemFullPath, false);

            _imageWidth  = textureFileData.Width;
            _imageHeight = textureFileData.Height;
            _texture     = new Texture(EditorApp.Instance.GL, _imageWidth, _imageHeight, textureFileData.RgbaPixels);
        }

        public override void OnImGuiRender()
        {
            ImGui.Image((IntPtr)_texture.GlTexture, new Vector2(_imageWidth, _imageHeight));
        }

        public override void DisposeResources()
        {
            _texture?.Dispose();
        }
    }

    public class NotFoundAssetPreviewLayout : AssetPreviewLayout
    {
        private string _text;
        public override void SetFromAsset(ProjectAsset asset)
        {
            _text = $"Asset of type {asset.Type} was not found on the disk\n" +
                    $"{asset.FileSystemFullPath}";
        }

        public override void OnImGuiRender()
        {
            ImGui.TextUnformatted(_text);
        }

        public override void DisposeResources()
        {
        }
    }

    public enum LayoutMode
    {
        Text,
        Image
    }
}