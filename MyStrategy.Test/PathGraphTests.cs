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
            var graph = new PathGraph(100, 100);
            AddWalls(graph, 20);

            graph.FindNums((2, 2));

            Debug.WriteLine(graph.ToNums());
        }

        private void AddWalls(PathGraph graph, int k)
        {
            var kk = graph.Dim.I - k;
            for (var i = k; i <= kk; i++)
            {
                graph[i, k] = true;
                graph[k, i] = true;
                graph[kk, i] = true;
                graph[i, kk] = true;
            }
        }

        [TestMethod]
        public void FindPathTest()
        {
            var graph = new PathGraph(30, 30);
            AddWalls(graph, 5);
            
            graph.FindPath((2, 2));

            Debug.WriteLine(graph.ToPath());
        }
    }
}