using CD_in_Core.Domain.Models.Sequences;
using CD_in_Core.Domain.Models.Specification;

namespace CD_in_Core.Domain.Models.Replacement
{
    public class ReplaceSingleTwosWithOnesSpecification : ISequenceCondition<KeyValuePair<int, int>>
    {
        private Sequence? _sequence;
        private int _searchValue = 2;

        public void SetSequence(Sequence sequence)
        {
            _sequence = sequence;
        }

        public bool IsSatisfiedBy(KeyValuePair<int, int> item)
        {
            if (_sequence == null)
                throw new InvalidOperationException("Sequence is not set");

            var digit = _sequence.Digits[item.Key];
            return digit == _searchValue && IsIsolated(item.Key);
        }

        private bool IsIsolated(int index)
        {
            return (!_sequence.Digits.TryGetValue(index - 1, out var left) || left != _searchValue)
                && (!_sequence.Digits.TryGetValue(index + 1, out var right) || right != _searchValue);
        }
    }
}