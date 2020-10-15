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
            graph.AddWall(centerWall);

            graph.FindNums((2, 2));

            Debug.WriteLine(graph.ToNums());
        }

        [TestMethod]
        public void FindPathTest()
        {
            SquarePolygon pointToGo = ((11, 5), 2);
            SquarePolygon centerWall = ((15, 15), 5);

            var graph = new PathGraph(30, 30);
            graph.AddWall(centerWall);

            graph.FindPath(pointToGo);

            Debug.WriteLine(graph.ToPath());
        }
    }
}