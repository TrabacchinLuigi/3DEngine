using System.Numerics;

namespace Engine
{
    public interface ICamera
    {
        Vector3 Position { get; }
        Vector3 Target { get; }
        Vector3 Up { get; }
        Matrix4x4 ViewMatrix { get; }
        Matrix4x4 TranslationMatrix { get; }


    }
}