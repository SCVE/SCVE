using System;
using System.IO;
using System.Numerics;
using ImGuiNET;
using SCVE.Editor.Imaging;
using SCVE.Engine.ImageSharpBindings;

namespace SCVE.Editor.ImGuiUi
{
    public class FilePickerModalPanel : ImGuiModalPanel
    {
        private DirectoryInfo _currentDirectory;

        private ThreeWayImage _directoryIcon;
        private ThreeWayImage _fileIcon;
        
        private float _padding = 16.0f;
        private float _thumbnailSize = 96.0f;

        public FilePickerModalPanel()
        {
            Name = "Open Project";

            var imageSharpTextureLoader = new ImageSharpTextureLoader();

            var fileIconTextureData = imageSharpTextureLoader.Load("assets/FileIcon.png");
            var directoryIconTextureData = imageSharpTextureLoader.Load("assets/DirectoryIcon.png");

            _fileIcon = new ThreeWayImage(new CpuImage(fileIconTextureData.RgbaPixels, fileIconTextureData.Width, fileIconTextureData.Height), "FileIcon");
            _directoryIcon = new ThreeWayImage(new CpuImage(directoryIconTextureData.RgbaPixels, directoryIconTextureData.Width, directoryIconTextureData.Height), "DirectoryIcon");
            
            _fileIcon.ToGpu();
            _directoryIcon.ToGpu();
        }

        public void Open(string location)
        {
            _currentDirectory = new DirectoryInfo(location);
            base.Open();
        }

        public override void OnImGuiRender()
        {
            if (IsOpen)
            {
                ImGui.OpenPopup(Name);
            }

            if (ImGui.BeginPopupModal(Name, ref IsOpen))
            {
                ImGui.TextDisabled($"Open Project");

                if (_currentDirectory.Parent is not null)
                {
                    if (ImGui.Button("<-"))
                    {
                        _currentDirectory = _currentDirectory.Parent;
                    }
                }

                float cellSize = _thumbnailSize + _padding;

                float panelWidth = ImGui.GetContentRegionAvail().X;
                int columnCount = (int) (panelWidth / cellSize);
                if (columnCount < 1)
                    columnCount = 1;

                ImGui.Columns(columnCount, "file_picker_columns", false);

                foreach (var entry in _currentDirectory.EnumerateFileSystemInfos())
                {
                    var path = entry.FullName;

                    string filename = entry.Name;

                    ImGui.PushID(filename);
                    var icon = path.IsDirectoryPath() ? _directoryIcon : _fileIcon;
                    ImGui.PushStyleColor(ImGuiCol.Button, new Vector4(0, 0, 0, 0));
                    ImGui.ImageButton((IntPtr) icon.GpuImage.GpuId, new Vector2(_thumbnailSize, _thumbnailSize), new Vector2(0, 1), new Vector2(1, 0));

                    if (ImGui.BeginDragDropSource())
                    {
                        // const wchar_t* itemPath = relativePath.c_str();
                        // ImGui.SetDragDropPayload("CONTENT_BROWSER_ITEM", itemPath, (wcslen(itemPath) + 1) * sizeof(wchar_t));
                        // ImGui.EndDragDropSource();
                    }

                    ImGui.PopStyleColor();
                    if (ImGui.IsItemHovered() && ImGui.IsMouseDoubleClicked(ImGuiMouseButton.Left))
                    {
                        if (entry.IsDirectory())
                            _currentDirectory = new DirectoryInfo(path);
                    }

                    ImGui.TextWrapped(filename);

                    ImGui.NextColumn();

                    ImGui.PopID();
                }

                ImGui.Columns(1);

                ImGui.SliderFloat("Thumbnail Size", ref _thumbnailSize, 32, 512);
                ImGui.SliderFloat("Padding", ref _padding, 0, 32);

                if (ImGui.Button("Close"))
                {
                    ImGui.CloseCurrentPopup();
                    Close();
                }

                ImGui.EndPopup();
            }
        }
    }
}