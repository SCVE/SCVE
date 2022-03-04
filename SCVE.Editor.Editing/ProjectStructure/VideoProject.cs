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
    }
}