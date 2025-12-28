namespace Variable.Core
{
    public interface IVariable
    {
        double GetRatio();
        bool IsFull { get; }
        bool IsEmpty { get; }
    }
}
