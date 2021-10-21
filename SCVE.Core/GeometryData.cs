using SCVE.Core.Utilities;

namespace SCVE.Core
{
    public class GeometryData
    {
        public float[] Vertices { get; set; }
        
        public int[] Indices { get; set; }

        public GeometryData(float[] vertices, int[] indices)
        {
            Logger.Construct(nameof(GeometryData));
            Vertices = vertices;
            Indices = indices;
        }
    }
}