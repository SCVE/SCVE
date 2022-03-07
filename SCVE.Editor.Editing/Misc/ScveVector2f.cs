namespace SCVE.Editor.Editing.Misc;

public class ScveVector2f
{
    public float X { get; set; }
    public float Y { get; set; }

    public ScveVector2f()
    {
    }

    public ScveVector2f(float x, float y)
    {
        X = x;
        Y = y;
    }

    public override string ToString()
    {
        return $"{{ X: {X:F}, Y: {Y:F} }}";
    }
}