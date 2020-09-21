namespace MyStrategy.Tools
{
    public interface ISceneManagerSettings
    {
        string SceneFileName { get; }
        int FpsInterval { get; }
        int SceneWidth { get; }
        int SceneHeight { get; }
        int RoundCount { get; }
    }
}