namespace CD_in_Core.Domain.Conditions
{
    public interface IValueCondition<T>
    {
        bool IsSatisfiedBy(T item);
    }
}
