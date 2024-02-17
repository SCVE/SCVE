using System.Numerics;
using ImGuiNET;

namespace SCVE.Editor.ImGuiUi.Panels
{
    public class FilePickerModalPanel : ImGuiFileBrowserPanelBase, IDisposable
    {
        private string _selectedFilePath;

        private string _title = $"File Selector";
        
        protected override void Initialize()
        {
            // var imageSharpTextureLoader = new ImageSharpTextureLoader();
            // var fileIconTextureData = imageSharpTextureLoader.Load("assets/FileIcon.png");
            // var directoryIconTextureData = imageSharpTextureLoader.Load("assets/DirectoryIcon.png");
            //
            // FileIcon = new ThreeWayImage(new CpuImage(fileIconTextureData.RgbaPixels, fileIconTextureData.Width, fileIconTextureData.Height), "FileIcon");
            // DirectoryIcon = new ThreeWayImage(new CpuImage(directoryIconTextureData.RgbaPixels, directoryIconTextureData.Width, directoryIconTextureData.Height), "DirectoryIcon");

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
            _selectedFilePath = location;
            _title = title;
            ImGui.OpenPopup(_title);
        }

        /// <summary>
        /// Returns true if the file was selected in this frame
        /// </summary>
        public bool OnImGuiRender(ref string path)
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

                path = _selectedFilePath;

                if (selected)
                {
                    ImGui.CloseCurrentPopup();
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
                    Content = CurrentDirectory.EnumerateFileSystemInfos();
                }

                ImGui.SameLine();
            }

            ImGui.Text(CurrentDirectory.FullName);

            if (ImGui.BeginChild("FilePickerTree", ImGui.GetContentRegionAvail() - new Vector2(0, 40)))
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
                                if (!entry.IsDirectory())
                                {
                                    _selectedFilePath = entry.FullName;
                                    selected = true;
                                }
                                else
                                {
                                    CurrentDirectory = new DirectoryInfo(path);
                                    Content = CurrentDirectory.EnumerateFileSystemInfos();
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
                    Content = CurrentDirectory.EnumerateFileSystemInfos();
                }

                ImGui.SameLine();
            }

            ImGui.Text(CurrentDirectory.FullName);

            if (ImGui.BeginChild("FilePickerCells", ImGui.GetContentRegionAvail() - new Vector2(0, 40)))
            {
                int columnCount = 6;
                ImGui.Columns(columnCount, "file_picker_columns", false);

                // magic 20, don't ask, it just works
                var columnWidth = ImGui.GetColumnWidth() - 20;

                foreach (var entry in Content)
                {
                    var path = entry.FullName;

                    string filename = entry.Name;

                    ImGui.PushID(filename);
                    var icon = path.IsDirectoryPath() ? DirectoryIcon : FileIcon;
                    // ImGui.PushStyleColor(ImGuiCol.Button, new Vector4(0, 0, 0, 0));

                    ImGui.ImageButton("filename", (IntPtr) icon.GpuImage.GpuId, new Vector2(columnWidth, columnWidth), new Vector2(0, 1), new Vector2(1, 0));

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
                            if (!entry.IsDirectory())
                            {
                                _selectedFilePath = entry.FullName;
                                selected = true;
                            }
                            else
                            {
                                CurrentDirectory = new DirectoryInfo(path);
                                Content = CurrentDirectory.EnumerateFileSystemInfos();
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