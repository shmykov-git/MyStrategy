namespace MyStrategy.DataModel.Acts
{
    public interface IPairAct
    {
        void Do(Unit main, Unit enemy);
        int Key { get; set; }
    }
}