using MyStrategy.Tools;

namespace Viewer.Uwp.Viewer
{
    class Settings : ISceneManagerSettings
    {
        public string SceneFileName => @"Assets\scene.json";
        public int FpsInterval => 200;
        public int SceneWidth => 100;
        public int SceneHeight => 100;
    }
}