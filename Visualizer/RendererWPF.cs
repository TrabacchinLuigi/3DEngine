using Engine;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

namespace Visualizer
{
    class Pens
    {
        public readonly Pen Normal = new(Brushes.GreenYellow, 1);
        public readonly Pen Edge = new(Brushes.Black, 1);
        public readonly Pen Camera = new(Brushes.Yellow, 1);
        public readonly Pen XAxis = new(Brushes.Red, 1);
        public readonly Pen YAxis = new(Brushes.Green, 1);
        public readonly Pen ZAxis = new(Brushes.Blue, 1);
    }

    public class RendererWPF : FrameworkElement
    {
        static readonly Single Onedegree = (Single)Math.PI / 180;

        public static readonly DependencyProperty ShowNormalsProperty =
            DependencyProperty.Register(nameof(ShowNormals), typeof(Boolean), typeof(RendererWPF), new PropertyMetadata(false));

        public Boolean ShowNormals
        {
            get { return (Boolean)GetValue(ShowNormalsProperty); }
            set { SetValue(ShowNormalsProperty, value); }
        }

        public UInt16 FPS
        {
            get { return (UInt16)GetValue(FPSPropertyKey.DependencyProperty); }
            private set { SetValue(FPSPropertyKey, value); }
        }

        // Using a DependencyProperty as the backing store for FPS.  This enables animation, styling, binding, etc...
        public static readonly DependencyPropertyKey FPSPropertyKey =
            DependencyProperty.RegisterReadOnly(nameof(FPS), typeof(UInt16), typeof(RendererWPF), new PropertyMetadata((UInt16)0));

        UInt16 _fpsAccumulator = 0;
        Stopwatch _fpsStopwatch = new();

        private readonly SolidColorBrush _brush;

        private readonly SolidColorBrush bgBrush = Brushes.Black;
        private readonly Pens Pens = new();

        private Mesh _Mesh;
        private readonly Object _DrawOrWriteMeshLock = new Object();
        private Matrix4x4 _worldMatrix;
        //private Vector3 _cameraPosition;
        //private Vector3 _cameraTarget;
        //private Vector3 _cameraUp = Vector3.UnitY;
        private Single _zNear = 0.1f;
        private Single _zFar = 1000f;
        //private Matrix4x4 _viewMatrix;
        private Matrix4x4? _ProjectionMatrix;
        private Matrix4x4 _scaleMatrix;
        private Matrix4x4 _flipYMatrix;
        private Matrix4x4 _scaleFlipMatrix;
        private Matrix4x4 _cameraMovementMatrix;

        //private Matrix4x4 _flipYMatrix;
        private Single _fov = (Single)(Math.PI / 3);
        private UInt64 renderpass;
        private Mesh _DrawnMesh;

        public RendererWPF()
        {
            _brush = new SolidColorBrush(Colors.Gray) { Opacity = 0.5 };
            _brush.Freeze();
            _Mesh = (Application.Current as App).LoadedMesh.Transform(Matrix4x4.CreateTranslation(Vector3.UnitZ * 5));
            _worldMatrix = Matrix4x4.CreateWorld(Vector3.Zero, -Vector3.UnitZ, Vector3.UnitY);
            _flipYMatrix = Matrix4x4.CreateReflection(new Plane(Vector3.UnitY, 0));
            //_cameraTarget = Vector3.Zero;
            //_cameraPosition = Vector3.Transform(new Vector3(0, 0, -10), _worldMatrix);
            //UpdateCameraMovementMatrix();
            //_viewMatrix = Matrix4x4.CreateLookAt(_cameraPosition, _cameraTarget, _cameraUp);

            //_flipYMatrix = Matrix4x4.CreateRotationZ((Single)Math.PI);
            Focusable = true;
            Loaded += RendererWPF_Loaded;

            _engineTask = Task.Run(SpinTheEngine);

        }

        private void SpinTheEngine()
        {
            while (!Dispatcher.HasShutdownStarted)
            {
                try
                {

                    if (_ProjectionMatrix == null) continue;
                    var mesh = new Mesh(_Mesh);
                    unchecked { renderpass++; }



                    var lessrenderpass = renderpass * 0.01f;
                    var rotationYM = /*Matrix4x4.Identity;*/ Matrix4x4.CreateRotationY(lessrenderpass, Vector3.Zero + Vector3.UnitZ * 5);
                    var rotationAndWorld = rotationYM * /* _cameraMovementMatrix **/ _worldMatrix;

#if DEBUG
                    _frameStopWatch.Start();
#endif
                    var validTriangles = new List<(Triangle Triangle, Vector3 Normal)>();
                    for (var i = 0; i < mesh.Triangles.Length; i++)
                    {
                        mesh.Triangles[i].Transform(rotationAndWorld);
                    }
                    mesh.CalculateNormals();
                    for (var i = 0; i < mesh.Triangles.Length; i++)
                    {
                        ref var triangle = ref mesh.Triangles[i];
                        ref var normal = ref mesh.Normals[i];
                        var dotviewnormal = (triangle.A - Vector3.Zero).Dot(normal);
                        if (dotviewnormal <= 0) validTriangles.Add((triangle, normal));
                    }
                    var tuples = validTriangles
                        .Select(x => (x.Triangle, x.Normal, minDistance: x.Triangle.MinDistance(Vector3.Zero)))
                        .OrderByDescending(x => x.minDistance)
                        .Select(x => (x.Triangle, x.Normal));
                    mesh = new Mesh(tuples.Select(x => x.Triangle).ToArray(), tuples.Select(x => x.Normal).ToArray());

                    //if (ShowNormals) { mesh.CalculateCentroids(); }
                    for (var i = 0; i < mesh.Triangles.Length; i++)
                    {
                        ref var triangle = ref mesh.Triangles[i];
                        triangle.ToScreenSpace(_ProjectionMatrix.Value);
                        triangle.Transform(_scaleFlipMatrix);
                    }

#if DEBUG
                    _frameStopWatch.Stop();
                    Debug.WriteLine($"calculating took\t{_frameStopWatch.Elapsed}ms");
                    _frameStopWatch.Restart();
#endif
                    Thread.Sleep(50);
                    lock (_DrawOrWriteMeshLock)
                    {
                        _DrawnMesh = mesh;
                    }
                    // _mre.WaitOne();
                    Dispatcher.Invoke(() => InvalidateVisual());

                }
                catch (Exception ex) { }
                finally { }
            }
        }

        private void UpdateCameraMovementMatrix()
        {
            //_cameraMovementMatrix = Matrix4x4.CreateTranslation(-_cameraPosition);
        }

        private void RendererWPF_Loaded(Object sender, RoutedEventArgs e)
        {
            this.Focus();
        }

        protected override void OnMouseDown(MouseButtonEventArgs e)
        {
            this.Focus();
            base.OnMouseDown(e);
        }


        protected override void OnKeyDown(KeyEventArgs e)
        {
            e.Handled = true;
            switch (e.Key)
            {
                //case Key.W:
                //{
                //    var movement = -Vector3.Normalize(_cameraPosition - _cameraTarget) * 0.1f;
                //    _cameraPosition += movement;
                //    _cameraTarget += movement;
                //    UpdateCameraMovementMatrix();
                //    UpdateScreenMatrixs();
                //    break;
                //}
                //case Key.S:
                //{
                //    var movement = Vector3.Normalize(_cameraPosition - _cameraTarget) * 0.1f;
                //    _cameraPosition += movement;
                //    _cameraTarget += movement;
                //    UpdateCameraMovementMatrix();
                //    UpdateScreenMatrixs();
                //    break;
                //}
                //case Key.Space:
                //{
                //    var movement = _cameraUp * 0.1f;
                //    _cameraPosition += movement;
                //    _cameraTarget += movement;
                //    UpdateCameraMovementMatrix();

                //    break;
                //}
                //case Key.LeftCtrl:
                //{
                //    var movement = -_cameraUp * 0.1f;
                //    _cameraPosition += movement;
                //    _cameraTarget += movement;
                //    UpdateCameraMovementMatrix();

                //    break;
                //}
                //case Key.A:
                //{
                //    var cameraDirection = Vector3.Normalize(_cameraTarget - _cameraPosition);
                //    var direction = Vector3.Cross(cameraDirection, _cameraUp);
                //    var movement = direction * 0.1f;
                //    _cameraPosition += movement;
                //    _cameraTarget += movement;
                //    UpdateCameraMovementMatrix();
                //    UpdateScreenMatrixs();
                //    break;
                //}
                //case Key.D:
                //{
                //    var cameraDirection = Vector3.Normalize(_cameraTarget - _cameraPosition);
                //    var direction = -Vector3.Cross(cameraDirection, _cameraUp);
                //    var movement = direction * 0.1f;
                //    _cameraPosition += movement;
                //    _cameraTarget += movement;
                //    UpdateCameraMovementMatrix();
                //    UpdateScreenMatrixs();
                //    break;
                //}
                //case Key.Left:
                //    _cameraTarget = Vector3.Transform(_cameraTarget, Matrix4x4.CreateRotationY(-Onedegree, _cameraPosition));

                //    UpdateCameraMovementMatrix();
                //    UpdateScreenMatrixs();
                //    break;
                //case Key.Right:
                //    _cameraTarget = Vector3.Transform(_cameraTarget, Matrix4x4.CreateRotationY(Onedegree, _cameraPosition));

                //    UpdateCameraMovementMatrix();
                //    UpdateScreenMatrixs();
                //    break;
                //case Key.Up:
                //    _cameraTarget = Vector3.Transform(_cameraTarget, Matrix4x4.CreateRotationX(-Onedegree, _cameraPosition));

                //    UpdateCameraMovementMatrix();
                //    UpdateScreenMatrixs();
                //    break;
                //case Key.Down:
                //    _cameraTarget = Vector3.Transform(_cameraTarget, Matrix4x4.CreateRotationX(Onedegree, _cameraPosition));

                //    UpdateCameraMovementMatrix();
                //    UpdateScreenMatrixs();
                //    break;
                default:
                    break;
            }
        }

        private readonly Stopwatch _frameStopWatch = new Stopwatch();
        private readonly Object _engineTask;

        protected override void OnRender(DrawingContext dc)
        {
            try
            {
                lock (_DrawOrWriteMeshLock)
                {
                    //if (!_fpsStopwatch.IsRunning)
                    //{
                    //    System.GC.TryStartNoGCRegion(100 * 1024*1024);
                    //}
                    _fpsStopwatch.Start();

                    var rect = new Rect(RenderSize);
                    dc.DrawRectangle(bgBrush, null, rect);
                    {
                        var vec0 = Vector3.Zero;
                        var vecx = Vector3.UnitX;
                        var vecy = Vector3.UnitY;
                        var vecz = Vector3.UnitZ;
                        vec0.Transform(_worldMatrix).ToScreenSpace(_ProjectionMatrix.Value).Transform(_scaleFlipMatrix);
                        vecx.Multiply(2).Transform(_worldMatrix).ToScreenSpace(_ProjectionMatrix.Value).Transform(_scaleFlipMatrix);
                        vecy.Multiply(2).Transform(_worldMatrix).ToScreenSpace(_ProjectionMatrix.Value).Transform(_scaleFlipMatrix);
                        vecz.Multiply(2).Transform(_worldMatrix).ToScreenSpace(_ProjectionMatrix.Value).Transform(_scaleFlipMatrix);

                        dc.DrawLine(Pens.XAxis, RelativeToCenter(new Point(vec0.X, vec0.Y)), RelativeToCenter(new Point(vecx.X, vecx.Y)));
                        dc.DrawLine(Pens.YAxis, RelativeToCenter(new Point(vec0.X, vec0.Y)), RelativeToCenter(new Point(vecy.X, vecy.Y)));
                        dc.DrawLine(Pens.ZAxis, RelativeToCenter(new Point(vec0.X, vec0.Y)), RelativeToCenter(new Point(vecz.X, vecz.Y)));

                        //var viewPositionOut = Vector3.Transform(Vector3.Transform(_cameraPosition, _cameraMovementMatrix * _worldMatrix), _updateMatrix);
                        //dc.DrawLine(Pens.Camera, RelativeToCenter(new Point(vec0.X, vec0.Y)), RelativeToCenter(new Point(viewPositionOut.X, viewPositionOut.Y)));
                    }

                    for (var i = 0; i < _DrawnMesh.Triangles.Length; i++)
                    {
                        ref var triangle = ref _DrawnMesh.Triangles[i];
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
                            ctx.BeginFigure(RelativeToCenter(new Point(triangle.A.X, triangle.A.Y)), true /* is filled */, true /* is closed */);
                            // Draw a line to the next specified point.
                            ctx.LineTo(RelativeToCenter(new Point(triangle.B.X, triangle.B.Y)), true /* is stroked */, false /* is smooth join */);
                            // Draw another line to the next specified point.
                            ctx.LineTo(RelativeToCenter(new Point(triangle.C.X, triangle.C.Y)), true /* is stroked */, false /* is smooth join */);
                        }
                        // Freeze the geometry (make it unmodifiable)
                        // for additional performance benefits.
                        geometry.Freeze();

                        dc.DrawGeometry(_brush, Pens.Edge, geometry);
                        if (ShowNormals)
                        {
                            ref var nIn = ref _DrawnMesh.Normals[i];
                            ref var centroid = ref _DrawnMesh.Centroids[i];
                            var normalEnd = centroid + (nIn * 0.1f);

                            centroid.ToScreenSpace(_ProjectionMatrix.Value).Transform(_scaleFlipMatrix);
                            normalEnd.ToScreenSpace(_ProjectionMatrix.Value).Transform(_scaleFlipMatrix);
                            dc.DrawLine(Pens.Normal, RelativeToCenter(new Point(centroid.X, centroid.Y)), RelativeToCenter(new Point(normalEnd.X, normalEnd.Y)));
                        }
                    }
#if DEBUG
                    _frameStopWatch.Stop();
                    Debug.WriteLine($"drwing took\t\t\t{_frameStopWatch.Elapsed}ms");
#endif
                }
            }
            catch (Exception ex) { }
            finally
            {
                //_mre.Set();
                _fpsAccumulator++;
                if (_fpsStopwatch.ElapsedMilliseconds >= 1000)
                {
                    _fpsStopwatch.Stop();
                    _fpsStopwatch.Reset();
                    this.FPS = _fpsAccumulator;
                    _fpsAccumulator = 0;
                    //GC.EndNoGCRegion();
                    //GC.Collect(0);
                }
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
            _scaleMatrix = Matrix4x4.CreateScale((Single)(sizeInfo.NewSize.Height) * 0.5f, (Single)(sizeInfo.NewSize.Width) * 0.5f, 1f);
            var ratio = (Single)(sizeInfo.NewSize.Width / sizeInfo.NewSize.Height);
            _ProjectionMatrix = ProjectionMatrix.Make(_fov, ratio, _zNear, _zFar); // Matrix4x4.CreatePerspectiveFieldOfView(_fov, ratio, _zNear, _zFar);
            _scaleFlipMatrix = _flipYMatrix * _scaleMatrix;

        }

        public void DrawLine(Vector3 start, Vector3 end)
        {
            throw new NotImplementedException();
        }

        public void DrawTriangle(Triangle triangle)
        {
            throw new NotImplementedException();
        }
    }
}
