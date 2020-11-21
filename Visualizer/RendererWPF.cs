using Engine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace Visualizer
{
    public class RendererWPF : FrameworkElement
    {
        private readonly SolidColorBrush _brush;

        private readonly SolidColorBrush bgBrush = Brushes.Black;
        private readonly Pen pen;
        private Mesh _Mesh;
        private Matrix4x4 _worldMatrix;
        private readonly Vector3 _viewPosition;
        private Matrix4x4 _viewMatrix;
        private Matrix4x4 _ProjectionMatrix;
        private Matrix4x4 _scaleMatrix;
        private Matrix4x4 _updateMatrix;
        private UInt64 renderpass;

        public RendererWPF()
        {
            _brush = new SolidColorBrush(Colors.Gray) { Opacity = 0.5 };
            _brush.Freeze();
            _Mesh = (Application.Current as App).LoadedMesh;
            _worldMatrix = Matrix4x4.CreateWorld(Vector3.Zero, Vector3.UnitZ, -Vector3.UnitY);
            _viewPosition = Vector3.Transform(new Vector3(-1, 3f, -5f), _worldMatrix);

            _viewMatrix = Matrix4x4.CreateLookAt(_viewPosition, Vector3.Zero, Vector3.UnitY);
            _ProjectionMatrix = Matrix4x4.CreatePerspectiveFieldOfView((Single)(Math.PI / 2), 1f, 0.1f, 30);
            pen = new Pen(Brushes.Black, 1);
        }

        protected override void OnRender(DrawingContext dc)
        {
            try
            {
                //var worldCamera = Vector3.Transform(_viewPosition * 100, _worldMatrix);
                unchecked { renderpass++; }

                var rect = new Rect(RenderSize);
                dc.DrawRectangle(bgBrush, null, rect);

                var lessrenderpass = renderpass * 0.01f;
                var rotationYM = Matrix4x4.CreateRotationY(lessrenderpass, Vector3.Zero);
                var rotationAndWorld = _worldMatrix * rotationYM;

                var toBeRendered = _Mesh.Triangles
                    .Select(x =>
                    {
                        var triangle = x.Transform(rotationAndWorld);
                        var normal = triangle.GetNormal();
                        var dotviewnormal = Vector3.Dot(Vector3.Normalize(triangle.A - _viewPosition), normal);
                        var centroid = Vector3.Divide(triangle.A + triangle.B + triangle.C, 3);
                        return (Triangle: triangle, Normal: normal, DotViewAndNormal: dotviewnormal, Centroid: centroid);
                    })
                    .Where(x => x.DotViewAndNormal < 0)
                    .OrderByDescending(x => x.Triangle.Vectors.Select(v => Vector3.Distance(_viewPosition, v)).Max())
                    .Select(x => (x.Triangle, x.Normal, x.Centroid))
                    .AsParallel();

                foreach (var (tIn, nIn, centroid) in toBeRendered)
                {
                    var tOut = tIn.Transform(_updateMatrix);

                    var normalStartOut = Vector3.Transform(centroid, _updateMatrix);
                    var normalEndIn = centroid + (nIn * 0.1f);
                    var normalEndOut = Vector3.Transform(normalEndIn, _updateMatrix);

                    // Create a StreamGeometry to use to specify myPath.
                    var geometry = new StreamGeometry
                    {
                        FillRule = FillRule.EvenOdd
                    };
                    // Open a StreamGeometryContext that can be used to describe this StreamGeometry
                    // object's contents.
                    using (var ctx = geometry.Open())
                    {
                        // Begin the triangle at the point specified. Notice that the shape is set to
                        // be closed so only two lines need to be specified below to make the triangle.
                        ctx.BeginFigure(RelativeToCenter(new Point(tOut.A.X, tOut.A.Y)), true /* is filled */, true /* is closed */);
                        // Draw a line to the next specified point.
                        ctx.LineTo(RelativeToCenter(new Point(tOut.B.X, tOut.B.Y)), true /* is stroked */, false /* is smooth join */);
                        // Draw another line to the next specified point.
                        ctx.LineTo(RelativeToCenter(new Point(tOut.C.X, tOut.C.Y)), true /* is stroked */, false /* is smooth join */);

                    }
                    // Freeze the geometry (make it unmodifiable)
                    // for additional performance benefits.
                    geometry.Freeze();
                    dc.DrawGeometry(_brush, pen, geometry);
                    dc.DrawLine(new Pen(Brushes.GreenYellow, 1), RelativeToCenter(new Point(normalStartOut.X, normalStartOut.Y)), RelativeToCenter(new Point(normalEndOut.X, normalEndOut.Y)));

                }
                {
                    var vec0 = Vector3.Zero;
                    var vecx = Vector3.Transform(Vector3.Transform(Vector3.UnitX * 2, _worldMatrix), _updateMatrix);
                    var vecy = Vector3.Transform(Vector3.Transform(Vector3.UnitY * 2, _worldMatrix), _updateMatrix);
                    var vecz = Vector3.Transform(Vector3.Transform(Vector3.UnitZ * 2, _worldMatrix), _updateMatrix);

                    dc.DrawLine(new Pen(Brushes.Red, 1), RelativeToCenter(new Point(vec0.X, vec0.Y)), RelativeToCenter(new Point(vecx.X, vecx.Y)));
                    dc.DrawLine(new Pen(Brushes.Green, 1), RelativeToCenter(new Point(vec0.X, vec0.Y)), RelativeToCenter(new Point(vecy.X, vecy.Y)));
                    dc.DrawLine(new Pen(Brushes.Blue, 1), RelativeToCenter(new Point(vec0.X, vec0.Y)), RelativeToCenter(new Point(vecz.X, vecz.Y)));

                    var viewPositionOut = Vector3.Transform(Vector3.Transform(_viewPosition, _worldMatrix), _updateMatrix);
                    dc.DrawLine(new Pen(Brushes.Yellow, 1), RelativeToCenter(new Point(vec0.X, vec0.Y)), RelativeToCenter(new Point(viewPositionOut.X, viewPositionOut.Y)));
                }
            }
            finally
            {
                Task.Delay(25).ContinueWith(x =>
                {
                    if (Dispatcher.HasShutdownStarted) return;
                    Dispatcher.Invoke(() => InvalidateVisual());
                });
            }
        }

        private Point RelativeToCenter(Point p)
        {
            p.X += ActualWidth * 0.5;
            p.Y += ActualHeight * 0.5;
            return p;
        }

        protected override void OnRenderSizeChanged(SizeChangedInfo sizeInfo)
        {
            base.OnRenderSizeChanged(sizeInfo);
            _scaleMatrix = Matrix4x4.CreateScale((Single)Math.Min(sizeInfo.NewSize.Width, sizeInfo.NewSize.Height) * 0.25f);
            _updateMatrix = _scaleMatrix * _ProjectionMatrix * _viewMatrix;
        }
    }
}
