using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using CCT.NUI.Core;

namespace CCT.NUI.Tests.Core
{
    [TestClass]
    public class PointFunctionsTests
    {
        [TestMethod]
        public void Calculates_Correct_Center()
        {
            var point1 = new Point(1, 1, 1);
            var point2 = new Point(0, 0, 0);

            var center = Point.Center(point1, point2);
            Assert.AreEqual(0.5, center.X);
            Assert.AreEqual(0.5, center.Y);
            Assert.AreEqual(0.5, center.Z);
        }

        [TestMethod]
        public void Calculates_Correct_Center_Of_Point_List()
        {
            var point1 = new Point(1, 1, 1);
            var point2 = new Point(0, 0, 0);
            var point3 = new Point(-1, -1, -1);

            var center = Point.Center(new[] { point1, point2, point3 });
            Assert.AreEqual(0, center.X);
            Assert.AreEqual(0, center.Y);
            Assert.AreEqual(0, center.Z);
        }

        
        [TestMethod]
        public void Calculates_Correct_Distance()
        {
            var point1 = new Point(2, 0, 0);
            var point2 = new Point(0, 0, 0);

            var distance = Point.Distance(point1, point2);
            Assert.AreEqual(2, distance);
        }

        [TestMethod]
        public void Calculates_Correct_2D_Distance()
        {
            var point1 = new Point(2, 0, 1);
            var point2 = new Point(0, 0, 5);

            var distance = Point.Distance2D(point1, point2);
            Assert.AreEqual(2, distance);
        }

        [TestMethod]
        public void Calculates_Correct_Subtraction()
        {
            var point1 = new Point(2, 2, 2);
            var point2 = new Point(1, 1, 1);

            var vector = Point.Subtract(point1, point2);
            Assert.AreEqual(1, vector.X);
            Assert.AreEqual(1, vector.Y);
            Assert.AreEqual(1, vector.Z);
        }

        [TestMethod]
        public void Calculates_Correct_Nearest_Point()
        {
            var point1 = new Point(2, 0, 1);
            var point2 = new Point(0, 0, 5);

            var targetPoint = new Point(0, 0, 0);

            var nearestPoint = Point.FindNearestPoint(targetPoint, new[] { point1, point2 });
            Assert.AreEqual(point2, nearestPoint);
        }

        [TestMethod]
        public void Calculates_Correct_Nearest_Point_Index()
        {
            var point1 = new Point(2, 0, 1);
            var point2 = new Point(0, 0, 5);

            var targetPoint = new Point(0, 0, 0);

            var index = Point.FindIndexOfNearestPoint(targetPoint, new[] { point1, point2 });
            Assert.AreEqual(1, index);
        }
    }
}
