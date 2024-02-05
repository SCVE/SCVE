using System;
using System.Collections.Generic;
using System.IO;
using System.Numerics;
using ImGuiNET;
using SCVE.Editor.Imaging;
using SCVE.Engine.ImageSharpBindings;

namespace SCVE.Editor.ImGuiUi.Panels
{
    public class DirectoryPickerModalPanel : ImGuiFileBrowserPanelBase
    {
        private string _selectedDirectoryPath;

        private string _title = "Directory Selector";

        protected override void Initialize()
        {
            var imageSharpTextureLoader = new ImageSharpTextureLoader();
            var fileIconTextureData = imageSharpTextureLoader.Load("assets/FileIcon.png");
            var directoryIconTextureData = imageSharpTextureLoader.Load("assets/DirectoryIcon.png");

            FileIcon = new ThreeWayImage(new CpuImage(fileIconTextureData.RgbaPixels, fileIconTextureData.Width, fileIconTextureData.Height), "FileIcon");
            DirectoryIcon = new ThreeWayImage(new CpuImage(directoryIconTextureData.RgbaPixels, directoryIconTextureData.Width, directoryIconTextureData.Height), "DirectoryIcon");

            FileIcon.ToGpu();
            DirectoryIcon.ToGpu();
            Initialized = true;
        }

        public override void Open(string location, string title)
        {
            if (!Initialized)
            {
                Initialize();
            }

            CurrentDirectory = new DirectoryInfo(location);
            Content = CurrentDirectory.EnumerateFileSystemInfos();
            _selectedDirectoryPath = location;
            _title = title;
            ImGui.OpenPopup(_title);
        }

        /// <summary>
        /// Returns true if the file was selected in this frame
        /// </summary>
        public bool OnImGuiRender(ref string location)
        {
            ImGui.SetNextWindowSize(new Vector2(600, 400));

            bool selected = false;

            // doesn't affect anything, it's just a dummy variable for ImGui.Net
            bool dummyOpened = true;

            if (ImGui.BeginPopupModal(_title, ref dummyOpened, ImGuiWindowFlags.NoResize | ImGuiWindowFlags.MenuBar))
            {
                DrawMenuBar();

                if (Mode)
                {
                    selected = DrawCells();
                }
                else
                {
                    selected = DrawTree();
                }

                ImGui.TextDisabled("Current location: ");
                ImGui.SameLine();

                ImGui.TextDisabled(_selectedDirectoryPath);

                location = _selectedDirectoryPath;

                if (ImGui.Button("Select folder"))
                {
                    ImGui.CloseCurrentPopup();

                    selected = true;
                }

                ImGui.EndPopup();
            }

            return selected;
        }

        private bool DrawTree()
        {
            bool selected = false;
            if (CurrentDirectory.Parent is not null)
            {
                if (ImGui.Button("<-"))
                {
                    CurrentDirectory = CurrentDirectory.Parent;
                    _selectedDirectoryPath = CurrentDirectory.FullName;
                    Content = CurrentDirectory.EnumerateFileSystemInfos();
                }

                ImGui.SameLine();
            }

            ImGui.Text(CurrentDirectory.FullName);

            if (ImGui.BeginChild("DirectoryPickerTree", ImGui.GetContentRegionAvail() - new Vector2(0, 60)))
            {
                foreach (var entry in Content)
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
                                    CurrentDirectory = new DirectoryInfo(path);
                                    Content = CurrentDirectory.EnumerateFileSystemInfos();
                                    _selectedDirectoryPath = entry.FullName;
                                    selected = true;
                                }
                            }
                            else if (ImGui.IsMouseClicked(ImGuiMouseButton.Left))
                            {
                                if (entry.IsDirectory())
                                {
                                    _selectedDirectoryPath = entry.FullName;
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

            return selected;
        }

        private bool DrawCells()
        {
            bool selected = false;
            if (CurrentDirectory.Parent is not null)
            {
                if (ImGui.Button("<-"))
                {
                    CurrentDirectory = CurrentDirectory.Parent;
                    _selectedDirectoryPath = CurrentDirectory.FullName;
                    Content = CurrentDirectory.EnumerateFileSystemInfos();
                }

                ImGui.SameLine();
            }

            ImGui.Text(CurrentDirectory.FullName);

            if (ImGui.BeginChild("DirectoryPickerCells", ImGui.GetContentRegionAvail() - new Vector2(0, 60)))
            {
                int columnCount = 6;
                ImGui.Columns(columnCount, "directory_picker_columns", false);

                // magic 20, don't ask, it just works
                var columnWidth = ImGui.GetColumnWidth() - 20;

                foreach (var entry in Content)
                {
                    var path = entry.FullName;

                    string filename = entry.Name;

                    ImGui.PushID(filename);
                    var icon = path.IsDirectoryPath() ? DirectoryIcon : FileIcon;
                    // ImGui.PushStyleColor(ImGuiCol.Button, new Vector4(0, 0, 0, 0));

                    ImGui.ImageButton(filename, (IntPtr) icon.GpuImage.GpuId, new Vector2(columnWidth, columnWidth));

                    ImGui.PopID();

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
                                _selectedDirectoryPath = entry.FullName;
                                CurrentDirectory = new DirectoryInfo(path);
                                Content = CurrentDirectory.EnumerateFileSystemInfos();
                                selected = true;
                            }
                        }
                        else if (ImGui.IsMouseClicked(ImGuiMouseButton.Left))
                        {
                            if (entry.IsDirectory())
                            {
                                _selectedDirectoryPath = entry.FullName;
                            }
                        }
                    }

                    ImGui.TextWrapped(filename);

                    ImGui.NextColumn();
                }

                ImGui.Columns(1);
                ImGui.EndChild();
            }

            return selected;
        }
    }
}