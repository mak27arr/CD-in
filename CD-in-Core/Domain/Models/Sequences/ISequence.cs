
namespace CD_in_Core.Domain.Models.Sequences
{
    public interface ISequence : IEnumerable<IElement>
    {
        int Count { get; }

        void Add(IElement element);

        void Add(int key, int value);

        void Clear();

        IElement GetNext(IElement item);

        IElement GetPrevious(IElement item);
    }
}