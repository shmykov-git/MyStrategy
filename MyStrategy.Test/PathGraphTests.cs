using System.Diagnostics;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MyStrategy.DataModel;
using Suit;

namespace MyStrategy.Test
{
    [TestClass]
    public class PathGraphTests
    {
        [TestInitialize]
        public void Initialize()
        {
            IoC.Configure(IoCTest.Register);
        }

        [TestMethod]
        public void FindNumsTest()
        {
            SquarePolygon centerWall = ((50, 50), 30);

            var graph = new PathGraph(100, 100);
            graph.AddWalls(centerWall);

            graph.FindNums((2, 2));

            Debug.WriteLine(graph.ToNums());
        }

        [TestMethod]
        public void FindPathTest()
        {
            SquarePolygon pointToGo = ((11, 5), 2);
            SquarePolygon wall1 = ((19, 8), 4);
            SquarePolygon wall2 = ((11, 19), 4);
            SquarePolygon wall3 = ((23, 17), 4);

            var graph = new PathGraph(30, 30);
            graph.AddWalls(wall1, wall2, wall3);

            graph.FindPath(pointToGo);

            Debug.WriteLine(graph.ToPath());
        }

        [TestMethod]
        public void FindPathFirstTest()
        {
            SquarePolygon pointToGo = ((11, 5), 2);
            Index pointA = (28, 28);
            SquarePolygon wall1 = ((19, 8), 4);
            SquarePolygon wall2 = ((11, 17), 4);
            SquarePolygon wall3 = ((23, 17), 4);

            var graph = new PathGraph(30, 30);
            graph.AddWalls(wall1, wall2, wall3);

            graph.FindPathFirst(pointToGo, pointA);
            graph.FindPathFirst(pointToGo, pointA);
            graph.FindPathFirst(pointToGo, pointA);

            Debug.WriteLine(graph.ToPath());
        }

        [TestMethod]
        public void FindPathATest()
        {
            PointPolygon pointToGo = (11, 5);
            //SquarePolygon pointToGo = ((11, 5), 2);
            Index pointA = (28, 10);
            SquarePolygon wall1 = ((19, 8), 4);
            SquarePolygon wall2 = ((11, 17), 4);
            SquarePolygon wall3 = ((23, 17), 4);

            var graph = new PathGraph(30, 30);
            graph.AddWalls(wall1, wall2, wall3);

            graph.FindPath1(pointToGo, pointA);

            Debug.WriteLine(graph.ToPath());
        }
    }
}