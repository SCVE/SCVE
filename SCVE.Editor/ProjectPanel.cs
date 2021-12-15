using ImGuiNET;

namespace SCVE.Editor
{
    public class ProjectPanel
    {
        private Project _project;

        public void LoadProject(string path)
        {
            if (Project.PathIsProject(path))
            {
                _project = Project.LoadFrom(path);
            }
        }

        private void PushImGuiAssetTreeFolder(ProjectAssetFolder folder)
        {
            foreach (var subfolder in folder.Subfolders)
            {
                if (ImGui.TreeNodeEx(subfolder.Name))
                {
                    PushImGuiAssetTreeFolder(subfolder);
                    ImGui.TreePop();
                }
            }
            foreach (var asset in folder.Assets)
            {
                if (ImGui.TreeNodeEx(asset.Name, ImGuiTreeNodeFlags.Leaf))
                {
                    ImGui.TreePop();
                }
            }
        }
        
        public void OnImGuiRender()
        {
            if (ImGui.Begin("Project Panel"))
            {
                if (_project is not null)
                {
                    ImGui.Text(_project.Name);
                }
                else
                {
                    ImGui.Text("Project is not loaded");
                }

                PushImGuiAssetTreeFolder(_project);
                
                ImGui.End();
            }
        }
    }
}