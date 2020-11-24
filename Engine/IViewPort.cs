using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Engine
{
    public interface IViewPort
    {
        Single Width { get; }
        Single Height { get; }
        Single ZNear { get; }
        Single ZFar { get; }
        Matrix4x4 ProjectionMatrix { get; }

        void DrawLine(Vector3 start, Vector3 end);
        void DrawTriangle(Triangle triangle);

    }
}
