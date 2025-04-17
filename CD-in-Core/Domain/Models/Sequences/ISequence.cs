
namespace CD_in_Core.Domain.Models.Sequences
{
    public interface ISequence : IEnumerable<IElement>
    {
        int Count { get; }

        IElement GetNext(IElement item);

        IElement GetPrevious(IElement item);
    }
}