namespace SCVE.Core.Entities
{
    public class WindowProps
    {
        public string Title { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }

        public WindowProps(string title = "SCVE", int width = 800, int height = 600)
        {
            Title = title;
            Width = width;
            Height = height;
        }

        public override string ToString()
        {
            return $"WindowProps: {{{nameof(Title)}: {Title}, {nameof(Width)}: {Width}, {nameof(Height)}: {Height}}}";
        }
    }
}