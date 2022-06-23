using SCVE.Editor.Abstractions;
using SCVE.Editor.Editing.ProjectStructure;

namespace SCVE.Editor.Services
{
    public class DragDropAssetToSequenceService : IService
    {
        public AssetBase DraggedAsset { get; private set; }

        public int Frame { get; private set; } = -1;

        public int Track { get; private set; } = -1;

        public void SetDraggedAsset(AssetBase asset)
        {
            DraggedAsset = asset;
        }

        public void SetDragDestination(int frame, int track)
        {
            Frame = frame;
            Track = track;
        }

        public void Reset()
        {
            DraggedAsset = null;
            Frame = -1;
            Track = -1;
        }
    }
}