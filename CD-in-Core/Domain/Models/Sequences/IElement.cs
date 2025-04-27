namespace CD_in_Core.Domain.Models.Sequences
{
    public interface IElement : ICloneable<IElement>
    {
        int Key { get; }

        int DisplayKey { get; set; }

        int Value { get; }
    }
}