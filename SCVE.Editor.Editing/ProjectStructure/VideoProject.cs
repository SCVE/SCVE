using System.Collections;
using System.Text.Json.Serialization;
using SCVE.Editor.Editing.Editing;

namespace SCVE.Editor.Editing.ProjectStructure
{
    public class VideoProject
    {
        /// <summary>
        /// Title of the project
        /// <remarks>
        /// Not the file name of the project
        /// </remarks>
        /// </summary>
        public string Title { get; set; }

        public ICollection<SequenceAsset> Sequences { get; set; }
        public ICollection<ImageAsset> Images { get; set; }

        public ICollection<FolderAsset> Folders { get; set; }

        public VideoProject()
        {
        }

        public static VideoProject CreateNew(string title)
        {
            return new VideoProject()
            {
                Title = title,
                Sequences = new List<SequenceAsset>(),
                Images = new List<ImageAsset>()
            };
        }

        public void AddSequence(SequenceAsset sequenceAsset)
        {
            Sequences.Add(sequenceAsset);
        }

        public void AddImage(ImageAsset imageAsset)
        {
            Images.Add(imageAsset);
        }
    }
}