using System;
using MyStrategy.Logs;
using MyStrategy.Tools;
using Suit;
using Suit.Logs;
using Unity;

namespace Viewer.Uwp.Viewer
{
    public static class IoCViewerUwp
    {
        public static void Register(UnityContainer container)
        {
            //container.RegisterType<ILog, LogToConsoleAndDebug>();
            container.RegisterType<ILog, NoLog>();

            container.RegisterSingleton<UwpViewer>();
            container.RegisterFactory<IViewer>(c => IoC.Get<UwpViewer>());

            container.RegisterSingleton<Settings>();
            container.RegisterFactory<ISceneManagerSettings>(c => IoC.Get<Settings>());

            container.RegisterFactory<Func<IPage, Controller>>(c => (Func<IPage, Controller>) (page =>
                new Controller(
                    IoC.Get<ILog>(),
                    IoC.Get<Settings>(),
                    IoC.Get<UwpViewer>(), 
                    IoC.Get<SceneManager>())
                {
                    Page = page
                }));
        }
    }
}