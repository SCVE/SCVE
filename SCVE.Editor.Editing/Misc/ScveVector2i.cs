namespace SCVE.Editor.Editing.Misc;

public struct ScveVector2I
{
    public int X { get; set; }
    public int Y { get; set; }

    public ScveVector2I()
    {
        X = 0;
        Y = 0;
    }

    public ScveVector2I(int x, int y)
    {
        X = x;
        Y = y;
    }

    public override string ToString()
    {
        return $"{{ X: {X}, Y: {Y} }}";
    }
}