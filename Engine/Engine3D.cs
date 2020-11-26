using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine
{
    class Engine3D
    {
        private readonly Middleware[] middlewares;

        public Engine3D(Middleware[] middlewares ) 
        {
            this.middlewares = middlewares;
            
        }
        public void RenderFrame(Scene scene)
        {
            foreach(var middleware in middlewares)
            {
                
            }
            //var toWorldSpace = _scene.Renderables
            //     .Select(x => x.Transform(_scene.WorldMatrix));
            //var withNormals = toWorldSpace
            //    .Select(x => {
                    
            //    });

           
            //var toBeRendered = _Mesh.Triangles
            //    .Select(x =>
            //    {
            //        var triangle = x.Transform(rotationAndWorld);
            //        var normal = triangle.GetNormal();
            //        var dotviewnormal = Vector3.Dot(triangle.A - Vector3.Zero, normal);

            //        return (Triangle: triangle, Normal: normal, DotViewAndNormal: dotviewnormal);
            //    })
            //    .Where(x => x.DotViewAndNormal <= 0)
            //    .OrderByDescending(x => x.Triangle.Vectors.Select(v => Vector3.Distance(_cameraPosition, v)).Max())
            //    .Select(x => (x.Triangle, x.Normal))
            //    .AsParallel();

            //foreach (var (tIn, nIn) in toBeRendered)
            //{
            //    var tOut = tIn.ToScreenSpace(_updateMatrix);

            //    // Create a StreamGeometry to use to specify myPath.
            //    var geometry = new StreamGeometry
            //    {
            //        FillRule = FillRule.EvenOdd
            //    };
            //    // Open a StreamGeometryContext that can be used to describe this StreamGeometry
            //    // object's contents.
            //    using (var ctx = geometry.Open())
            //    {
            //        // Begin the triangle at the point specified. Notice that the shape is set to
            //        // be closed so only two lines need to be specified below to make the triangle.
            //        ctx.BeginFigure(RelativeToCenter(new Point(tOut.A.X, tOut.A.Y)), true /* is filled */, true /* is closed */);
            //        // Draw a line to the next specified point.
            //        ctx.LineTo(RelativeToCenter(new Point(tOut.B.X, tOut.B.Y)), true /* is stroked */, false /* is smooth join */);
            //        // Draw another line to the next specified point.
            //        ctx.LineTo(RelativeToCenter(new Point(tOut.C.X, tOut.C.Y)), true /* is stroked */, false /* is smooth join */);

            //    }
            //    // Freeze the geometry (make it unmodifiable)
            //    // for additional performance benefits.
            //    geometry.Freeze();

            //    dc.DrawGeometry(_brush, Pens.Edge, geometry);
            //    if (ShowNormals)
            //    {
            //        var centroid = Vector3.Divide(tIn.A + tIn.B + tIn.C, 3);
            //        var normalStartOut = Vector3.Transform(centroid, _updateMatrix);
            //        var normalEndIn = centroid + (nIn * 0.1f);
            //        var normalEndOut = Vector3.Transform(normalEndIn, _updateMatrix);
            //        dc.DrawLine(Pens.Normal, RelativeToCenter(new Point(normalStartOut.X, normalStartOut.Y)), RelativeToCenter(new Point(normalEndOut.X, normalEndOut.Y)));
            //    }
            //}
        }
    }
}
