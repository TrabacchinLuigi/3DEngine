using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Engine
{
    public class Mesh
    {
        //public static Mesh MakeUnitCube()
        //{
        //    var u000 = Vector3.Zero;
        //    var u001 = Vector3.UnitZ;
        //    var u010 = Vector3.UnitY;
        //    var u011 = Vector3.UnitY + Vector3.UnitZ;
        //    var u100 = Vector3.UnitX;
        //    var u101 = Vector3.UnitX + Vector3.UnitZ;
        //    var u110 = Vector3.UnitX + Vector3.UnitY;
        //    var u111 = Vector3.One;

        //    return new Mesh(
        //        new Triangle[]
        //        {
        //            // sud
        //            new Triangle(u000, u010, u110),
        //            new Triangle(u000, u110, u100),
        //            //bottom 
        //            new Triangle(u001, u000, u100),
        //            new Triangle(u001, u100, u101),
        //            //nord
        //            new Triangle(u101, u111, u011),
        //            new Triangle(u101, u011, u001),
        //            //top
        //            new Triangle(u010, u011, u111),
        //            new Triangle(u010, u111, u110),
        //            // west
        //            new Triangle(u001, u011, u010),
        //            new Triangle(u001, u010, u000),
        //            // east
        //            new Triangle(u100, u110, u111),
        //            new Triangle(u100, u111, u101),
        //        });
        //}
        public Triangle[] Triangles { get; }

        public Mesh(Triangle[] triangles)
        {
            Triangles = triangles;
        }
    }
}
