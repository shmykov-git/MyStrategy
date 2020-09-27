using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MyStrategy.DataModel;
using MyStrategy.Extensions;

namespace MyStrategy.Test
{
    [TestClass]
    public class MathTests
    {
        [TestMethod]
        public void LineTest()
        {
            Line l = ((1, 2), (3, 4));
            var d = l.GetDistanceToLine((4, 3));
            Assert.AreEqual(2f.Sqrt().ToCheckValue(), d.ToCheckValue());

            var d2 = l.GetDistanceToLine((2, 5));
            Assert.AreEqual(-2f.Sqrt().ToCheckValue(), d2.ToCheckValue());
        }

        [TestMethod]
        public void PassCircleObstacleTest()
        {
            //Line line = ((1, 2), (10, 10));
            //var res = line.PassCircleObstacle(((-1, -1), 2)).ToArray();
            //Assert.AreEqual(0, res.Length);

            //var res2 = line.PassCircleObstacle(((5, 5), 2)).ToArray();
            //Assert.AreEqual(2, res2.Length);
        }

        [TestMethod]
        public void FirstMoveToPassCircleObstacleGroupTest()
        {
            //Line line = ((1, 2), (10, 10));
            //var move = line.FirstMoveToPassCircleObstacleGroup(((-1, -1), 2));
            //Assert.AreEqual((10,10), move);

            //var move2 = line.FirstMoveToPassCircleObstacleGroup(((5, 5), 2), ((6, 6), 2));
            //Assert.AreNotEqual((10, 10), move2);
        }
    }
}