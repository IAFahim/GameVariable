namespace Variable.Core
{
    public interface IVariable
    {
        bool IsFull { get; }
        bool IsEmpty { get; }
        double GetRatio();
    }
}