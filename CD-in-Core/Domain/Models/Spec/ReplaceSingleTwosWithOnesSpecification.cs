using CD_in_Core.Domain.Models.Sequences;
using CD_in_Core.Domain.Models.Specification;

namespace CD_in_Core.Domain.Models.Replacement
{
    public class ReplaceSingleTwosWithOnesSpecification : ISequenceCondition<KeyValuePair<int, int>>
    {
        private ISequence? _sequence;
        private int _searchValue = 2;

        public void SetSequence(ISequence sequence)
        {
            _sequence = sequence;
        }

        public bool IsSatisfiedBy(KeyValuePair<int, int> item)
        {
            if (_sequence == null)
                throw new InvalidOperationException("Sequence is not set");

            return item.Value == _searchValue && IsIsolated(item);
        }

        private bool IsIsolated(KeyValuePair<int, int> element)
        {
            return (_sequence.GetPrevious(element.Key) != _searchValue)
                && (_sequence.GetNext(element.Key) != _searchValue);
        }
    }
}