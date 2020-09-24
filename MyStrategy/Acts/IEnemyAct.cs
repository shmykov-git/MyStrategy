using MyStrategy.DataModel;

namespace MyStrategy.Acts
{
    public interface IEnemyAct : IAct
    {
        Unit Enemy { get; }
    }
}