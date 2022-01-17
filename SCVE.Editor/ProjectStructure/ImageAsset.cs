using System;

namespace SCVE.Editor.ProjectStructure
{
    public class ImageAsset : ProjectAsset
    {
        public int Width { get; set; }
        public int Height { get; set; }
        
        public ImageAsset(Guid guid, string internalName, string internalFullPath, string fileSystemFullPath, string type) : base(guid, internalName, internalFullPath, fileSystemFullPath, type)
        {
        }
    }
}