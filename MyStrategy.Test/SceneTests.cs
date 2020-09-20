using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MyStrategy.Extensions;
using MyStrategy.Tools;
using Suit;
using Suit.Logs;

namespace MyStrategy.Test
{
    [TestClass]
    public class SceneTests
    {
        [TestInitialize]
        public void Initialize()
        {
            IoC.Configure(IoCTest.Register);
        }

        [TestMethod]
        public void TestStart()
        {
            var sceneManager = IoC.Get<SceneManager>();
            var log = IoC.Get<ILog>();
            var viewer = (TestViewer)IoC.Get<IViewer>();

            sceneManager.Start();
            viewer.IsActive = true;
            var scene = sceneManager.Scene;

            for (var round = 1; round < 20; round++)
            {
                scene.Round = round;

                log.Debug($"===== round {round} =====");
                foreach (var unit in sceneManager.Scene.Units.ToArray())
                {
                    foreach (var opponent in unit.GetUnderSightOpponents())
                    foreach (var act in unit.PairActs)
                    {
                        act.Do(unit, opponent);
                    }

                    foreach (var act in unit.SelfActs)
                    {
                        act.Do(unit);
                    }
                }
            }

            foreach (var clan in scene.Clans.Where(c=>c.Units.Count>0))
            {
                log.Debug($"Win {clan}");

                foreach (var unit in clan.Units)
                {
                    log.Debug($"unit {unit.Id} hp:{unit.Hp}");
                }
            }
        }
    }
}
