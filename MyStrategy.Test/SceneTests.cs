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
            var viewer = IoC.Get<TestViewer>();

            sceneManager.InitScene();

            viewer.IsActive = true;

            sceneManager.PlayScene().Wait();
        }
    }
}
