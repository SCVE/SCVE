using SCVE.Editor.Editing;
using SCVE.Editor.Imaging;

namespace SCVE.Editor.Effects
{
    public class EffectApplicationContext
    {
        public Sequence Sequence { get; set; }

        public Clip Clip { get; set; }

        public IImage SourceImageFrame { get; set; }
    }
}