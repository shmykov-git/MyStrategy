using MyStrategy.Tools;

namespace MyStrategy.Test
{
    class TestSettings : ISceneManagerSettings
    {
        public string SceneFileName => "scene.json";
        public int FpsInterval => 0;
        public int SceneWidth => 100;
        public int SceneHeight => 100;
    }
}