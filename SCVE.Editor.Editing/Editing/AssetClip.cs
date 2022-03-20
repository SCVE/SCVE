using System.Text.Json.Serialization;
using SCVE.Editor.Editing.Effects;

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

    public static AssetClip CreateNew(Guid referencedAssetId, int startFrame, int frameLength)
    {
        return new AssetClip()
        {
            Guid = Guid.NewGuid(),
            Effects = new List<EffectBase>(),
            StartFrame = startFrame,
            FrameLength = frameLength,
            ReferencedAssetId = referencedAssetId
        };
    }

    public override string ShortName()
    {
        return $"Image Clip";
    }
}