
namespace CD_in_Core.Domain.Models.Sequences
{
    public interface ISequence : IEnumerable<IElement>
    {
        int Count { get; }

        void Add(IElement element);

        void Add(int key, int value);

        IElement GetNext(IElement item);

        IElement GetPrevious(IElement item);

        void Clear();

        void SetCapacity(int count);
    }
}