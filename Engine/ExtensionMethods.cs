using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Engine
{
    public static class ExtensionMethods
    {
        public static ref Vector4 Transform(this ref Vector4 position, in Matrix4x4 matrix)
        {
            if (!matrix.IsIdentity)
            {
                var x = (position.X * matrix.M11) + (position.Y * matrix.M21) + (position.Z * matrix.M31) + matrix.M41;
                var y = (position.X * matrix.M12) + (position.Y * matrix.M22) + (position.Z * matrix.M32) + matrix.M42;
                var z = (position.X * matrix.M13) + (position.Y * matrix.M23) + (position.Z * matrix.M33) + matrix.M43;
                var w = position.W = (position.X * matrix.M14) + (position.Y * matrix.M24) + (position.Z * matrix.M34) + matrix.M44;
                position.X = x;
                position.Y = y;
                position.Z = z;
                position.W = w;
            }
            return ref position;
        }

        public static ref Vector3 Transform(this ref Vector3 position, in Matrix4x4 matrix)
        {
            if (!matrix.IsIdentity)
            {
                var x = (position.X * matrix.M11) + (position.Y * matrix.M21) + (position.Z * matrix.M31) + matrix.M41;
                var y = (position.X * matrix.M12) + (position.Y * matrix.M22) + (position.Z * matrix.M32) + matrix.M42;
                var z = (position.X * matrix.M13) + (position.Y * matrix.M23) + (position.Z * matrix.M33) + matrix.M43;

                position.X = x;
                position.Y = y;
                position.Z = z;
            }
            return ref position;
        }

        public static ref Vector3 Divide(this ref Vector3 left, in Single right)
        {
            left.X /= right;
            left.Y /= right;
            left.Z /= right;
            return ref left;
        }

        public static ref Vector3 Multiply(this ref Vector3 left, in Single right)
        {
            left.X *= right;
            left.Y *= right;
            left.Z *= right;
            return ref left;
        }

        public static ref Vector3 Divide(this ref Vector3 left, in Vector3 right)
        {
            left.X /= right.X;
            left.Y /= right.Y;
            left.Z /= right.Z;
            return ref left;
        }

        public static ref Vector3 Multiply(this ref Vector3 left, in Vector3 right)
        {
            left.X *= right.X;
            left.Y *= right.Y;
            left.Z *= right.Z;
            return ref left;
        }

        public static ref Vector3 ToScreenSpace(this ref Vector3 input, in Matrix4x4 projection)
        {
            var v4 = new Vector4(input, 1f);
            v4.Transform(projection);
            input.X = v4.X;
            input.Y = v4.Y;
            input.Z = v4.Z;

            if (v4.W != 0) { input.Divide(v4.W); }
            return ref input;
        }

        public static Single Dot(this in Vector3 vector1, in Vector3 vector2)
        {
            return (vector1.X * vector2.X)
                 + (vector1.Y * vector2.Y)
                 + (vector1.Z * vector2.Z);
        }
        public static Single DistanceSquared(this in Vector3 value1, in Vector3 value2)
        {
            var difference = value1 - value2;
            return Dot(difference, difference);
        }

        public static Single Distance(this in Vector3 value1, in Vector3 value2)
        {
            var distanceSquared = DistanceSquared(value1, value2);
            return MathF.Sqrt(distanceSquared);
        }
    }
}
