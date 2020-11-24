using System.Linq;
using System.Numerics;
using System.Runtime.CompilerServices;

namespace Engine
{
    public record Triangle
    {
        public static Vector3 GetNormal(Triangle x)
            => Vector3.Normalize(Vector3.Cross(x.B - x.A, x.C - x.A));

        public static Triangle Transform(Triangle x, Matrix4x4 matrix)
        {
            var tranformedVectors = x.Vectors.Select(x => Vector3.Transform(x, matrix)).ToArray();
            return new Triangle(tranformedVectors[0], tranformedVectors[1], tranformedVectors[2]);
        }

        public static Triangle ToScreenSpace(Triangle x, Matrix4x4 projection)
        {
            var tranformedVectors = x.Vectors
                .Select(x => x.ToScreenSpace(projection))
                .ToArray();
            return new Triangle(tranformedVectors[0], tranformedVectors[1], tranformedVectors[2]);
        }

        public Vector3[] Vectors { get; } = new Vector3[3];
        public Vector3 A { get => Vectors[0]; }
        public Vector3 B { get => Vectors[1]; }
        public Vector3 C { get => Vectors[2]; }

        public Triangle(Vector3 a, Vector3 b, Vector3 c)
        {
            Vectors[0] = a;
            Vectors[1] = b;
            Vectors[2] = c;
        }
        public Vector3 GetNormal() => GetNormal(this);

        public Triangle Transform(Matrix4x4 matrix) => Triangle.Transform(this, matrix);
        public Triangle ToScreenSpace(Matrix4x4 matrix) => Triangle.ToScreenSpace(this, matrix);
    }
}
