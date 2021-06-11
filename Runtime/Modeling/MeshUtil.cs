using UnityEngine;

namespace UsefulThings.Modeling
{
    public static class MeshUtil
    {
        public static readonly Mesh BasicQuad = CreateQuadMesh();
        
        public static Mesh CreateQuadMesh(float width = 1f, float height = 1f)
        {
            Mesh mesh = new Mesh();

            mesh.vertices = new []
            {
                new Vector3(0, 0, 0),
                new Vector3(width, 0, 0),
                new Vector3(width, height, 0),
                new Vector3(0, height, 0),
            };

            mesh.triangles = new []
            {
                0, 1, 2,
                0, 2, 3,
            };

            mesh.uv = new[]
            {
                new Vector2(0, 0),
                new Vector2(1, 0),
                new Vector2(1, 1),
                new Vector2(0, 1),
            };
        
            mesh.RecalculateNormals();
            return mesh;
        }
    }
}