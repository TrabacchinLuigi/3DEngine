using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Engine
{
    public class Scene
    {
        public static Scene CreateLeftHanded(params ICamera[] cameras)
            => new Scene(Matrix4x4.CreateWorld(Vector3.Zero, -Vector3.UnitZ, Vector3.UnitY), cameras);

        private readonly List<IRenderable> _renderables = new();
        private readonly ICamera[] Cameras;
        public Matrix4x4 WorldMatrix { get; }

        public Scene(Matrix4x4 worldMatrix, params ICamera[] cameras)
        {
            Cameras = cameras;
            this.WorldMatrix = worldMatrix;
        }
        public IEnumerable<IRenderable> Renderables => _renderables;

        void Add(IRenderable renderable)
        {
            _renderables.Add(renderable);
        }


    }
}
