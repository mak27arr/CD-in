using CD_in_Core.Domain.Models.Sequences;

namespace CD_in_Core.Domain.Conditions
{
    public class ReplaceSingleTwosWithOnesSpecification : ISequenceCondition<IElement>
    {
        private ISequence? _sequence;
        private int _searchValue = 2;

        public void SetSequence(ISequence sequence)
        {
            _sequence = sequence;
        }

        public bool IsSatisfiedBy(IElement item)
        {
            if (_sequence == null)
                throw new InvalidOperationException("Sequence is not set");

            return item.Value == _searchValue && IsIsolated(item);
        }

        private bool IsIsolated(IElement element)
        {
            return _sequence?.GetPrevious(element).Value != _searchValue
                && _sequence?.GetNext(element).Value != _searchValue;
        }
    }
}