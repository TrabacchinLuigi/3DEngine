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
        public static Vector3 ToScreenSpace(this Vector3 input, Matrix4x4 projection)
        {
            var v4 = new Vector4(input, 1f);
            v4 = Vector4.Transform(v4, projection);
            var v3 = new Vector3(v4.X, v4.Y, v4.Z);
            if (v4.W != 0) { v3 = Vector3.Divide(v3, v4.W); }
            if(v4.W == 0) { }
            return v3;
        }
    }
}
