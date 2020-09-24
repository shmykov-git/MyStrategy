using MyStrategy.DataModel;

namespace MyStrategy.Acts
{
    public interface IAct
    {
        void Do();
        void Clean();
        Unit Unit { get; }
    }
}