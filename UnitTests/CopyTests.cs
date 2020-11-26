using Engine;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Numerics;

namespace UnitTests
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void Copied_triangles_should_be_independent()
        {
            var triangle = new Triangle(Vector3.UnitX, Vector3.UnitY, Vector3.UnitZ);

            var copy = new Triangle(triangle);
            copy.A = Vector3.One;
            Assert.AreNotEqual(triangle.A, copy.A);

        }
    }
}
