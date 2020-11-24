using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Engine
{
    public class ProjectionMatrix
    {
        /// <param name="fieldOfView">Expressed in rad</param>
        public static Matrix4x4 Make(Single fieldOfView, Single aspectRatio, Single zNear, Single zFar)
        {
            var f = 1 / (Single)Math.Tan(fieldOfView * 0.5f);
            var zdepth = zFar - zNear;
            var q = zFar / zdepth;
            var matrix = new Matrix4x4()
            {
                M11 = aspectRatio * f,
                M22 = f,
                M33 = q,
                M34 = 1,
                M43 = -zNear * q,
            };
            return matrix;
        }
    }
}
