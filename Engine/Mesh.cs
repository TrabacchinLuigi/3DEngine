using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Engine
{
    public struct Mesh : IRenderable
    {
        public static ref Mesh Transform(ref Mesh input, in Matrix4x4 matrix)
        {
            for (var i = 0; i < input.Triangles.Length; i++)
            {
                ref var triangle = ref input.Triangles[i];
                triangle.Transform(matrix);
            }
            return ref input;
        }

        public Triangle[] Triangles { get; }
        public Vector3[] Normals { get; private set; }
        public Vector3[] Centroids { get; private set; }

        public Mesh(Triangle[] triangles, Vector3[] normals = null, Vector3[] centroids = null)
        {
            Triangles = triangles;
            if (normals == null) Normals = Array.Empty<Vector3>();
            else { Normals = normals; }

            if (centroids == null) Centroids = Array.Empty<Vector3>();
            else { Centroids = centroids; }
        }

        public Mesh(in Mesh source)
        {
            Triangles = new Triangle[source.Triangles.Length];
            Normals = new Vector3[source.Normals.Length];
            Centroids = new Vector3[source.Centroids.Length];
            source.Triangles.CopyTo((Span<Triangle>)Triangles);
            source.Normals.CopyTo((Span<Vector3>)Normals);
            source.Centroids.CopyTo((Span<Vector3>)Centroids);
        }

        public Mesh Transform(in Matrix4x4 matrix) => Mesh.Transform(ref this, matrix);

        IRenderable IRenderable.Transform(Matrix4x4 matrix)
            => Transform(matrix);

        public void CalculateNormals()
        {
            if (Normals.Length != Triangles.Length)
            {
                Normals = new Vector3[Triangles.Length];
            }
            for (var i = 0; i < Triangles.Length; i++)
            {
                Normals[i] = Triangles[i].GetNormal();
            }
        }

        public void CalculateCentroids()
        {
            if (Centroids.Length != Centroids.Length)
            {
                Centroids = new Vector3[Triangles.Length];
            }
            for (var i = 0; i < Triangles.Length; i++)
            {
                Centroids[i] = Triangles[i].GetCentroid();
            }
        }
    }
}
