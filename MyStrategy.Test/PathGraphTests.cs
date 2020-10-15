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
        public void FindAllTest()
        {
            var graph = new PathGraph(100, 100);

            for (var i = 20; i <= 80; i++)
            {
                graph[i, 20] = true;
                graph[20, i] = true;
                graph[80, i] = true;
                graph[i, 80] = true;
            }
            graph.FindAll((2,2));
            
            Debug.WriteLine(graph);
        }
    }
}