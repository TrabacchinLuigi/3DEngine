using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Engine
{
    public class Mesh : IRenderable
    {
        public static Mesh Transform(Mesh input, Matrix4x4 matrix)
              => new Mesh(input.Triangles.Select(x => x.Transform(matrix)));

        public IEnumerable<Triangle> Triangles { get; }

        public Mesh(IEnumerable<Triangle> triangles)
        {
            Triangles = triangles;
        }

        public Mesh Transform(Matrix4x4 matrix) => Mesh.Transform(this, matrix);

        IRenderable IRenderable.Transform(Matrix4x4 matrix)
            => Transform(matrix);
    }
}
