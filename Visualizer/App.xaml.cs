using Engine;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Threading.Tasks;
using System.Windows;

namespace Visualizer
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public Mesh LoadedMesh { get; private set; }
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            var filePath = e.Args.SingleOrDefault(x => x.StartsWith("file=", StringComparison.OrdinalIgnoreCase))?.Split("=", 2, StringSplitOptions.RemoveEmptyEntries).Last();

            using var fileStream = File.OpenRead(filePath);
            var sr = new StreamReader(fileStream);

            var triangles = new List<Triangle>();
            var vectors = new List<Vector3>();
            while (!sr.EndOfStream)
            {
                var line = sr.ReadLine();
                if (line.StartsWith('v'))
                {
                    var points = line.Split(' ').Skip(1).Select(x => Single.Parse(x, System.Globalization.CultureInfo.InvariantCulture)).ToArray();
                    var vector = new Vector3(points[0], points[1], points[2]);
                    vectors.Add(vector);
                }
                if (line.StartsWith('f'))
                {
                    var indices = line.Split(' ').Skip(1).Select(x => Int32.Parse(x)-1).ToArray();
                    var triangle = new Triangle(vectors[indices[0]], vectors[indices[1]], vectors[indices[2]]);
                    triangles.Add(triangle);
                }
            }
            LoadedMesh = new Mesh(triangles.ToArray());
        }
    }
}
