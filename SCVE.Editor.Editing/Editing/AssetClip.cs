using System.Text.Json.Serialization;

namespace SCVE.Editor.Editing.Editing;

/// <summary>
/// Any clip, that references an asset
/// </summary>
public class AssetClip : Clip
{
    public Guid ReferencedAssetId { get; private set; }

    [JsonConstructor]
    private AssetClip()
    {
    }

    private AssetClip(Guid guid, int startFrame, int frameLength, Guid referencedAssetId) : base(guid, startFrame, frameLength)
    {
        ReferencedAssetId = referencedAssetId;
    }

    public static AssetClip CreateNew(Guid referencedAssetId, int startFrame, int frameLength)
    {
        return new(Guid.NewGuid(), startFrame, frameLength, referencedAssetId);
    }

    public override string ShortName()
    {
        return $"Image Clip";
    }
}