using System.Text.Json.Serialization;
using SCVE.Editor.Editing.Effects;
using SCVE.Editor.Editing.ProjectStructure;

namespace SCVE.Editor.Editing.Editing;

/// <summary>
/// Any clip, that references an asset
/// </summary>
public class AssetClip : Clip
{
    public AssetType AssetType { get; private set; }
    public Guid ReferencedAssetId { get; private set; }

    [JsonConstructor]
    private AssetClip()
    {
    }

    public static AssetClip CreateNew(AssetType assetType, Guid referencedAssetId, int startFrame, int frameLength)
    {
        return new AssetClip()
        {
            Guid = Guid.NewGuid(),
            Effects = new List<EffectBase>(),
            AssetType = assetType,
            StartFrame = startFrame,
            FrameLength = frameLength,
            ReferencedAssetId = referencedAssetId
        };
    }

    public override Clip Duplicate()
    {
        return new AssetClip()
        {
            Guid = Guid.NewGuid(),
            Effects = new List<EffectBase>(),
            AssetType = AssetType,
            StartFrame = StartFrame,
            FrameLength = FrameLength,
            ReferencedAssetId = ReferencedAssetId
        };
    }

    public override string ShortName()
    {
        return $"Image Clip";
    }
}