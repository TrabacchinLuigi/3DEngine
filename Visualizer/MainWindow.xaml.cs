using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Visualizer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
    //    [System.Runtime.InteropServices.DllImport("gdi32.dll")]
    //    public static extern bool DeleteObject(IntPtr hObject);

        //private Bitmap _bmp;
        //private readonly CancellationTokenSource _cts;
        //private readonly ManualResetEventSlim _mre;
        //private readonly Thread _renderThread;
        //private Int32 w;
        //private Int32 h;
        public MainWindow()
        {
            InitializeComponent();
            
            
            
            //_cts = new CancellationTokenSource();
            //_mre = new ManualResetEventSlim(true);
           // _renderThread = new Thread((Object state) =>
           //{
           //    var ct = (CancellationToken)state;
           //    while (!ct.IsCancellationRequested)
           //    {
           //        Class1.DrawShit(_bmp, w, h);
           //        Thread.Sleep(100);
           //        _mre.Wait();
           //    }
           //});

        }

        private void Window_MouseDown(Object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
                this.DragMove();
        }

        private void Window_GotFocus(Object sender, RoutedEventArgs e)
        {
            //_mre.Set();
        }

        private void Window_LostFocus(Object sender, RoutedEventArgs e)
        {
           // _mre.Reset();
        }

        private void Window_Closed(Object sender, EventArgs e)
        {
            //_cts.Cancel();
            //_mre.Set();
            //_renderThread.Join();
            //_cts.Dispose();
        }

        private void Window_Resized(Object sender, SizeChangedEventArgs e)
        {
            UpdateWH();
        }

        private void UpdateWH()
        {
            //var parent = Image.Parent as FrameworkElement;

            //if (Double.IsNaN(parent.ActualWidth) || Double.IsNaN(parent.ActualHeight)) return;
            //w = (Int32)parent.ActualWidth;
            //h = (Int32)parent.ActualHeight;
        }

        private void Window_ContentRendered(Object sender, EventArgs e)
        {
            //UpdateWH();

            //_bmp = new System.Drawing.Bitmap(w, h);

            ////Convert the bitmap to BitmapSource for use with WPF controls
            //var hBmp = _bmp.GetHbitmap();
            //Image.Source = System.Windows.Interop.Imaging.CreateBitmapSourceFromHBitmap(
            //          hBmp,
            //          IntPtr.Zero,
            //          Int32Rect.Empty,
            //          System.Windows.Media.Imaging.BitmapSizeOptions.FromEmptyOptions());
            //Image.Source.Freeze();
            //DeleteObject(hBmp); //Clean up original bitmap

            //_renderThread.Start(_cts.Token);
        }
    }
}
