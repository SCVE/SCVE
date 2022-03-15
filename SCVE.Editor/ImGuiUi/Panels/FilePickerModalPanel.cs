using System;
using System.Collections.Generic;
using System.IO;
using System.Numerics;
using ImGuiNET;
using SCVE.Editor.Imaging;
using SCVE.Engine.ImageSharpBindings;

namespace SCVE.Editor.ImGuiUi.Panels
{
    public class FilePickerModalPanel : ImGuiModalPanel
    {
        private DirectoryInfo _currentDirectory;

        private IEnumerable<FileSystemInfo> _content;

        private ThreeWayImage _directoryIcon;
        private ThreeWayImage _fileIcon;

        public string SelectedPath => _selectedFilePath;
        private string _selectedFilePath;

        private bool _mode;


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

            ImGui.SetNextWindowSize(new Vector2(600, 400));
            
            if (ImGui.BeginPopupModal(Name, ref IsOpen, ImGuiWindowFlags.NoResize | ImGuiWindowFlags.MenuBar))
            {
                if (ImGui.BeginMenuBar())
                {
                    if (ImGui.BeginMenu("Mode"))
                    {
                        if (ImGui.MenuItem("Tree", _mode))
                        {
                            _mode = !_mode;
                        }
                        if (ImGui.MenuItem("Cells", !_mode))
                        {
                            _mode = !_mode;
                        }
                        ImGui.EndMenu();
                    }
                    ImGui.EndMenuBar();
                }

                if (_mode)
                {
                    DrawCells();
                }
                else
                {
                    DrawTree();
                }

                if (ImGui.Button("Close"))
                {
                    ImGui.CloseCurrentPopup();
                    Dismiss();
                }

                ImGui.EndPopup();
            }
        }

        private void DrawTree()
        {
            if (_currentDirectory.Parent is not null)
            {
                if (ImGui.Button("<-"))
                {
                    _currentDirectory = _currentDirectory.Parent;
                    _content = _currentDirectory.EnumerateFileSystemInfos();
                }
                
                ImGui.SameLine();
            }
            
            ImGui.Text(_currentDirectory.FullName);

            if (ImGui.BeginChild("FilePickerTree", ImGui.GetContentRegionAvail() - new Vector2(0, 40)))
            {
                foreach (var entry in _content)
                {
                    var path = entry.FullName;
                    var name = entry.Name;
                    if (entry.IsDirectory())
                    {
                        ImGui.PushStyleColor(ImGuiCol.Text, 0xFF0000FF);
                    }

                    if (ImGui.TreeNodeEx(name, ImGuiTreeNodeFlags.SpanFullWidth | ImGuiTreeNodeFlags.Leaf))
                    {
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

                        ImGui.TreePop();
                    }

                    if (entry.IsDirectory())
                    {
                        ImGui.PopStyleColor();
                    }
                }
                
                ImGui.EndChild();
            }
        }

        private void DrawCells()
        {
            if (_currentDirectory.Parent is not null)
            {
                if (ImGui.Button("<-"))
                {
                    _currentDirectory = _currentDirectory.Parent;
                    _content = _currentDirectory.EnumerateFileSystemInfos();
                }
                
                ImGui.SameLine();
            }
            
            ImGui.Text(_currentDirectory.FullName);

            if (ImGui.BeginChild("FilePickerTree", ImGui.GetContentRegionAvail() - new Vector2(0, 40)))
            {
                int columnCount = 6;
                ImGui.Columns(columnCount, "file_picker_columns", false);
                
                // magic 20, don't ask, it just works
                var columnWidth = ImGui.GetColumnWidth() - 20;
                
                foreach (var entry in _content)
                {
                    var path = entry.FullName;

                    string filename = entry.Name;

                    ImGui.PushID(filename);
                    var icon = path.IsDirectoryPath() ? _directoryIcon : _fileIcon;
                    // ImGui.PushStyleColor(ImGuiCol.Button, new Vector4(0, 0, 0, 0));
                    
                    ImGui.ImageButton((IntPtr) icon.GpuImage.GpuId, new Vector2(columnWidth, columnWidth), new Vector2(0, 1), new Vector2(1, 0));

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
                ImGui.EndChild();
            }

            ImGui.Columns(1);
        }
    }
}