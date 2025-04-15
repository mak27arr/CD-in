
namespace CD_in_Core.Domain.Models.Sequences
{
    public interface ISequence : IEnumerable<Element>
    {
        int Count { get; }

        Element GetNext(Element item);

        Element GetPrevious(Element item);
    }
}