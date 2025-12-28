namespace Variable.Core
{
    public interface IBoundedInfo
    {
        bool IsFull();
        bool IsEmpty();
        double GetRatio();
    }
}