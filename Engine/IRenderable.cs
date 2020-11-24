using System.Numerics;

namespace Engine
{
    public  interface IRenderable
    {
        IRenderable Transform(Matrix4x4 matrix);
    }
}