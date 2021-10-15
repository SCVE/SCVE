using SCVE.Core.Primitives;

namespace SCVE.Core
{
    public class GeometryGenerator
    {
        public static GeometryData GenerateRect(Rect rect)
        {
            float[] vertices = {
                // Top left
                rect.X - rect.Width / 2, rect.Y + rect.Height / 2, 0f, 
                // Top right
                rect.X + rect.Width / 2, rect.Y + rect.Height / 2, 0f, 
                // Bottom right
                rect.X + rect.Width / 2, rect.Y - rect.Height / 2, 0f, 
                // Bottom left
                rect.X - rect.Width / 2, rect.Y - rect.Height / 2, 0f, 
            };

            int[] indices = {
                0, 3, 1,
                1, 3, 2
            };

            return new GeometryData(vertices, indices);
        }
    }
}