namespace MyStrategy.DataModel.Acts
{
    public interface ISelfAct
    {
        void Do(Unit unit);
        int Key { get; set; }
    }
}