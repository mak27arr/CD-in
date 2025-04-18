namespace CD_in_Core.Domain.Conditions
{
    public interface IValueTransformer<T>
    {
        T Transform(T originalValue);
    }
}
