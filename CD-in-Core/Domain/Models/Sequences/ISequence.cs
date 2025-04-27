
namespace CD_in_Core.Domain.Models.Sequences
{
    public interface ISequence : IEnumerable<IElement>
    {
        int Count { get; }

        void Add(IElement element);

        [Obsolete]
        void Add(int key, int value);

        void Add(int key, int displayKey, int value);

        void Remove(IElement item);

        IElement GetNext(IElement item);

        IElement GetPrevious(IElement item);

        void Clear();

        void ReindexDisplayKeys();

        void SetCapacity(int count);
    }
}