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
        public static Matrix4x4 Make(Single zNear, Single zFar, Single fieldOfView, Single aspectRatio)
        {
            var cotan = 1 / (Single)Math.Tan(fieldOfView * 0.5f);
            var zdepth = zFar - zNear;
            var matrix = new Matrix4x4()
            {
                M11 = aspectRatio * cotan,
                M22 = cotan,
                M33 = zFar / zdepth,
                M34 = -zFar * zFar / zdepth,
                M43 = 1
            };
            return matrix;
        }
    }
}
