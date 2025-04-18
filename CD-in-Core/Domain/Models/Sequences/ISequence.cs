
namespace CD_in_Core.Domain.Models.Sequences
{
    public interface ISequence : IEnumerable<KeyValuePair<int, int>>
    {
        int Count { get; }

        void Add(int index, int value);

        void Add(KeyValuePair<int, int> element);

        void Clear();

        int GetNext(int index);

        int GetPrevious(int index);
    }
}