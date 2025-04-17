using CD_in_Core.Domain.Models.Specification;

namespace CD_in_Core.Domain.Models.Spec
{
    public class GreaterOrEqualThanCondition : IValueCondition<int>
    {
        private readonly int _threshold;

        public GreaterOrEqualThanCondition(int threshold)
        {
            _threshold = threshold;
        }

        public bool IsSatisfiedBy(int value) => value >= _threshold;
    }
}
