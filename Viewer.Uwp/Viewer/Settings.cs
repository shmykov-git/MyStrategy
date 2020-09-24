using MyStrategy.Tools;

namespace Viewer.Uwp.Viewer
{
    class Settings : ISceneManagerSettings
    {
        public string SceneFileName => @"Scenes\scene.json";
        public int FpsInterval => 50;
        public int SceneWidth => 500;
        public int SceneHeight => 500;
        public int RoundCount => -1;
    }
}