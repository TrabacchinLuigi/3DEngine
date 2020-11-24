using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Engine
{
    class Line : IRenderable
    {
        private readonly Vector3 start;
        private readonly Vector3 end;

        public Line(Vector3 start, Vector3 end)
        {
            this.start = start;
            this.end = end;
        }

        public IRenderable Transform(Matrix4x4 matrix)
        {
            throw new NotImplementedException();
        }
    }
}
