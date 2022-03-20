using System;
using System.Collections.Generic;
using System.IO;
using ImGuiNET;
using SCVE.Editor.Imaging;

namespace SCVE.Editor.ImGuiUi.Panels
{
    public abstract class ImGuiFileBrowserPanelBase : IDisposable
    {
        protected DirectoryInfo CurrentDirectory;

        protected IEnumerable<FileSystemInfo> Content;

        protected ThreeWayImage DirectoryIcon;
        protected ThreeWayImage FileIcon;

        protected bool Mode;

        protected bool Initialized = false;

        protected void DrawMenuBar()
        {
            if (ImGui.BeginMenuBar())
            {
                if (ImGui.BeginMenu("Mode"))
                {
                    if (ImGui.MenuItem("Tree", Mode))
                    {
                        Mode = !Mode;
                    }

                    if (ImGui.MenuItem("Cells", !Mode))
                    {
                        Mode = !Mode;
                    }

                    ImGui.EndMenu();
                }

                ImGui.EndMenuBar();
            }
        }

        protected abstract void Initialize();

        /// <summary>
        /// DON'T CALL FROM MenuItem
        /// </summary>
        public abstract void Open(string location, string title);

        public void Dispose()
        {
            DirectoryIcon?.Dispose();
            FileIcon?.Dispose();
        }
    }
}