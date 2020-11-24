using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Engine
{
    class PerspectiveCamera : ICamera
    {
        public PerspectiveCamera()
        {

        }

        public Vector3 Position => throw new NotImplementedException();

        public Vector3 Target => throw new NotImplementedException();

        public Vector3 Up => throw new NotImplementedException();

        public Matrix4x4 ViewMatrix => throw new NotImplementedException();

        public Matrix4x4 TranslationMatrix => throw new NotImplementedException();
    }
}
