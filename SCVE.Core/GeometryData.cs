namespace SCVE.Core
{
    public class GeometryData
    {
        public float[] Vertices { get; set; }
        
        public int[] Indices { get; set; }

        public GeometryData(float[] vertices, int[] indices)
        {
            Vertices = vertices;
            Indices = indices;
        }
    }
}