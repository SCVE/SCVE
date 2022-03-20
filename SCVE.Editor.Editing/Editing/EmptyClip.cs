using System.Text.Json.Serialization;
using SCVE.Editor.Editing.Effects;

namespace SCVE.Editor.Editing.Editing
{
    public class EmptyClip : Clip
    {
        [JsonConstructor]
        private EmptyClip()
        {
        }

        public static EmptyClip CreateNew(int startFrame, int frameLength)
        {
            return new EmptyClip()
            {
                Guid = Guid.NewGuid(),
                Effects = new List<EffectBase>(),
                FrameLength = frameLength,
                StartFrame = startFrame
            };
        }

        public override string ShortName()
        {
            return "Empty Clip";
        }
    }
}