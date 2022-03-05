namespace SCVE.Editor.Editing.Misc;

public class ScveVector2i
{
    public int X { get; set; }
    public int Y { get; set; }

    public ScveVector2i()
    {
    }

    public ScveVector2i(int x, int y)
    {
        X = x;
        Y = y;
    }

    public override string ToString()
    {
        return $"{{ X: {X}, Y: {Y} }}";
    }
}