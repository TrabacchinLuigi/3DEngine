using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Runtime.CompilerServices;

namespace Engine
{
    public struct Triangle
    {

        public void  Transform( in Matrix4x4 matrix)
        {
            A.Transform(matrix);
            B.Transform(matrix);
            C.Transform(matrix);
        }

        public Vector3 A;
        public Vector3 B;
        public Vector3 C;

        public Triangle(Vector3 a, Vector3 b, Vector3 c)
        {
            A = a;
            B = b;
            C = c;
        }

        public Triangle(Triangle source)
             : this(source.A, source.B, source.C) { }


        public Vector3 GetNormal() 
            => Vector3.Normalize(Vector3.Cross(this.B - this.A, this.C - this.A));

        public void ToScreenSpace(in Matrix4x4 projection)
        {
            A.ToScreenSpace(projection);
            B.ToScreenSpace(projection);
            C.ToScreenSpace(projection);
        }

        public void CopyTo(Span<Vector3> destination)
        {
            destination[0] = A;
            destination[0] = B;
            destination[0] = C;
        }

        public Single MaxDistance(in Vector3 point)
        {
            var distanceA = point.Distance(A);
            var distanceB = point.Distance(B);
            var distanceC = point.Distance(C);
            return distanceA > distanceB
                ? distanceA > distanceC
                    ? distanceA
                    : distanceC
                : distanceB;
        }

        public Single MinDistance(in Vector3 point)
        {
            var distanceA = point.Distance(A);
            var distanceB = point.Distance(B);
            var distanceC = point.Distance(C);
            return distanceA < distanceB
                ? distanceA < distanceC
                    ? distanceA
                    : distanceC
                : distanceB;
        }

        public static Vector3 GetCentroid(in Triangle triangle)
        {
            var sum = triangle.A + triangle.B + triangle.C;
            sum.Divide(3);
            return sum;
        }

        public Vector3 GetCentroid() => Triangle.GetCentroid(this);
    }
}
