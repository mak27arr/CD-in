using CD_in_Core.Domain.Models.Sequences;

namespace CD_in_Core.Domain.Conditions
{
    public class GreaterOrEqualThanCondition : IValueCondition<IElement>
    {
        private readonly int _threshold;

        public GreaterOrEqualThanCondition(int threshold)
        {
            _threshold = threshold;
        }

        public bool IsSatisfiedBy(IElement value) => value.Value >= _threshold;
    }
}
