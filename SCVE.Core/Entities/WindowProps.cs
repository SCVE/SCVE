namespace SCVE.Core.Entities
{
    public class WindowProps
    {
        public string Title { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public bool IsMain { get; set; }

        public WindowProps(string title = "SCVE", int width = 1600, int height = 900, bool isMain = true)
        {
            IsMain = isMain;
            Title = title;
            Width = width;
            Height = height;
        }

        public override string ToString()
        {
            return $"WindowProps: {{{nameof(Title)}: {Title}, {nameof(Width)}: {Width}, {nameof(Height)}: {Height}, {nameof(IsMain)}: {IsMain}}}";
        }
    }
}