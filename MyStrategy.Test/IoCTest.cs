using MyStrategy.Logs;
using MyStrategy.Tools;
using Suit;
using Suit.Logs;
using Unity;

namespace MyStrategy.Test
{
    public static class IoCTest
    {
        public static void Register(UnityContainer container)
        {
            container.RegisterType<ILog, LogToConsoleAndDebug>();

            container.RegisterSingleton<TestViewer>();
            container.RegisterFactory<IViewer>(c => IoC.Get<TestViewer>());

            container.RegisterSingleton<TestSettings>();
            container.RegisterFactory<ISceneManagerSettings>(c => IoC.Get<TestSettings>());
        }
    }
}