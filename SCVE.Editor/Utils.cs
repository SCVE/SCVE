using System;
using System.IO;
using System.Numerics;
using SCVE.Editor.Editing.Editing;
using SCVE.Editor.Imaging;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Drawing.Processing;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;
using FontCollection = SixLabors.Fonts.FontCollection;

namespace SCVE.Editor
{
    public static class Utils
    {
        public static bool IsDirectory(this FileSystemInfo info)
        {
            // get the file attributes for file or directory
            FileAttributes attr = info.Attributes;

            //detect whether its a directory or file
            if ((attr & FileAttributes.Directory) == FileAttributes.Directory)
                return true;
            else
                return false;
        }

        public static bool IsDirectoryPath(this string path)
        {
            return Directory.Exists(path);
        }

        public static ThreeWayImage CreateNoPreviewImage(int width, int height)
        {
            var previewImage = new CpuImage(new byte[width * height * 4], width, height);

            using var image = Image.WrapMemory<Rgba32>(previewImage.ToByteArray(), width, height);
            
            FontCollection fontCollection = new FontCollection();
            fontCollection.Add("assets/Font/arial.ttf");
            var font = fontCollection.Get("arial").CreateFont(72);
            image.Mutate(i => i.DrawText($"NO PREVIEW", font, Color.Red, new PointF(10, 0)));

            return new ThreeWayImage(previewImage, "NO PREVIEW");
        }

        public static Sequence CreateTestingSequence()
        {
            var sequence = Sequence.CreateNew(30, new Vector2(1920, 1080));
            sequence.FrameLength = 100;
            sequence.AddTrack(Track.CreateNew());
            sequence.AddTrack(Track.CreateNew());
            sequence.AddTrack(Track.CreateNew());

            sequence.Tracks[0].AddClip(EmptyClip.CreateNew(0, 10));
            sequence.Tracks[0].AddClip(EmptyClip.CreateNew(30, 30));
            sequence.Tracks[0].AddClip(EmptyClip.CreateNew(60, 30));

            sequence.Tracks[1].AddClip(EmptyClip.CreateNew(10, 10));
            sequence.Tracks[1].AddClip(EmptyClip.CreateNew(20, 10));
            sequence.Tracks[1].AddClip(EmptyClip.CreateNew(40, 15));

            sequence.Tracks[2].AddClip(ImageClip.CreateNew(10, 30, Guid.Parse("53d08676-4b40-4efe-bab7-2588dc697e25")));

            return sequence;
        }
    }
}