namespace Variable.Core
{
    public interface IBoundedInfo
    {
        bool IsFull { get; }
        bool IsEmpty { get; }
        double GetRatio();
    }
}