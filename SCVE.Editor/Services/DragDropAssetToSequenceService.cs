using SCVE.Editor.Abstractions;
using SCVE.Editor.Editing.ProjectStructure;

namespace SCVE.Editor.Services
{
    public class DragDropAssetToSequenceService : IService
    {
        public AssetBase DraggedAsset { get; private set; }

        public int Frame { get; private set; }

        public int Track { get; private set; }

        public void SetDraggedAsset(AssetBase asset)
        {
            DraggedAsset = asset;
        }

        public void SetDragDestination(int frame, int track)
        {
            Frame = frame;
            Track = track;
        }
    }
}