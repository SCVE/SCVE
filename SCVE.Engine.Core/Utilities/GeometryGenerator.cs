using SCVE.Engine.Core.Entities;
using SCVE.Engine.Core.Primitives;

namespace SCVE.Engine.Core.Utilities
{
    public static class GeometryGenerator
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

        public static GeometryData GenerateUnitSquare()
        {
            float[] vertices = {
                // Top left
                -0.5f, 0.5f, 0f, 
                // Top right
                0.5f, 0.5f, 0f, 
                // Bottom right
                0.5f, -0.5f, 0f, 
                // Bottom left
                -0.5f, -0.5f, 0f, 
            };

            int[] indices = {
                0, 3, 1,
                1, 3, 2
            };

            return new GeometryData(vertices, indices);
        }

        public static GeometryData GeneratePositiveUnitSquare()
        {
            float[] vertices = {
                // Top left
                0, 0, 0f, 
                // Top right
                1, 0, 0f, 
                // Bottom right
                1, 1, 0f, 
                // Bottom left
                0, 1, 0f, 
            };

            int[] indices = {
                0, 3, 1,
                1, 3, 2
            };

            return new GeometryData(vertices, indices);
        }
    }
}