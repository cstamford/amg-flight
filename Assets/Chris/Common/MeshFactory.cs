// ==================================================================== \\
// File   : MeshFactory.cs                                              \\
// Author : Christopher Stamford									    \\
//                                                                      \\
// Static class containing functions to build various primitives.       \\
// ==================================================================== \\

using UnityEngine;

namespace cst.Common
{
    internal class MeshFactory
    {
        public static Mesh buildQuad()
        {
            Mesh mesh = new Mesh();

            Vector3[] vertices =
            {
                new Vector3(1, 1, 0),
                new Vector3(1, -1, 0),
                new Vector3(-1, 1, 0),
                new Vector3(-1, -1, 0),
            };

            Vector2[] texCoords =
            {
                new Vector2(1, 1),
                new Vector2(1, 0),
                new Vector2(0, 1),
                new Vector2(0, 0),
            };

            int[] indexes =
            {
                0, 1, 2,
                2, 1, 3,
            };

            mesh.vertices = vertices;
            mesh.uv = texCoords;
            mesh.triangles = indexes;
            mesh.RecalculateNormals();

            return mesh;
        }
    }
}
