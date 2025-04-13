namespace CD_in_Core.Domain.Models.Specification
{
    public interface IValueCondition<T>
    {
        bool IsSatisfiedBy(T item);
    }
}
