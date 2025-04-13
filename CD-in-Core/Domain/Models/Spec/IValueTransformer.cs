namespace CD_in_Core.Domain.Models.Specification
{
    public interface IValueTransformer<T>
    {
        T Transform(T originalValue);
    }
}
