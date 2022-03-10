using System.Collections;
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

        public void AddImage(string fileName, string relativePath)
        {
            var imageAsset = new ImageAsset()
            {
                Guid  = Guid.NewGuid(),
                Name = fileName,
                Location = "/",
                Content = new Image()
                {
                    Guid = Guid.NewGuid(),
                    RelativePath = relativePath
                }
            };
            
            Images.Add(imageAsset);
        }
    }
}