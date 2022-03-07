using System;
using System.Collections.Generic;
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

        private IEnumerable<FileSystemInfo> _content;

        private ThreeWayImage _directoryIcon;
        private ThreeWayImage _fileIcon;
        
        private float _thumbnailSize = 96.0f;
        private string _selectedFilePath;

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

        public string SelectedPath => _selectedFilePath;

        public void Open(string location, Action closed, Action dismissed)
        {
            _currentDirectory = new DirectoryInfo(location);
            _content = _currentDirectory.EnumerateFileSystemInfos();
            base.Open(closed, dismissed);
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
                        _content = _currentDirectory.EnumerateFileSystemInfos();
                    }
                }

                float cellSize = _thumbnailSize;

                float panelWidth = ImGui.GetContentRegionAvail().X;
                int columnCount = Math.Clamp((int) (panelWidth / cellSize), 1, 8);

                ImGui.Columns(columnCount, "file_picker_columns", false);

                foreach (var entry in _content)
                {
                    var path = entry.FullName;

                    string filename = entry.Name;

                    ImGui.PushID(filename);
                    var icon = path.IsDirectoryPath() ? _directoryIcon : _fileIcon;
                    // ImGui.PushStyleColor(ImGuiCol.Button, new Vector4(0, 0, 0, 0));
                    ImGui.ImageButton((IntPtr) icon.GpuImage.GpuId, new Vector2(_thumbnailSize, _thumbnailSize), new Vector2(0, 1), new Vector2(1, 0));

                    if (ImGui.BeginDragDropSource())
                    {
                        ImGui.Text(filename);
                        // const wchar_t* itemPath = relativePath.c_str();
                        // ImGui.SetDragDropPayload("CONTENT_BROWSER_ITEM", itemPath, (wcslen(itemPath) + 1) * sizeof(wchar_t));
                        ImGui.EndDragDropSource();
                    }

                    // ImGui.PopStyleColor();
                    if (ImGui.IsItemHovered())
                    {
                        if (ImGui.IsMouseDoubleClicked(ImGuiMouseButton.Left))
                        {
                            if (entry.IsDirectory())
                            {
                                _currentDirectory = new DirectoryInfo(path);
                                _content = _currentDirectory.EnumerateFileSystemInfos();
                            }
                            else
                            {
                                _selectedFilePath = entry.FullName;
                                ImGui.CloseCurrentPopup();
                                Close();
                            }
                        }
                    }
                    
                    ImGui.TextWrapped(filename);

                    ImGui.NextColumn();

                    ImGui.PopID();
                }

                ImGui.Columns(1);

                ImGui.SliderFloat("Thumbnail Size", ref _thumbnailSize, 32, 512);

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