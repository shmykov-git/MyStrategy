namespace MyStrategy.DataModel.Acts
{
    public interface IAct
    {
        void Do();
        void Clean();
        Unit Unit { get; }
    }
}