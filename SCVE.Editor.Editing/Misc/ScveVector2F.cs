namespace SCVE.Editor.Editing.Misc;

public struct ScveVector2F
{
    public float X { get; set; }
    public float Y { get; set; }

    public ScveVector2F()
    {
        X = 0;
        Y = 0;
    }

    public ScveVector2F(float x, float y)
    {
        X = x;
        Y = y;
    }

    public override string ToString()
    {
        return $"{{ X: {X:F}, Y: {Y:F} }}";
    }
}