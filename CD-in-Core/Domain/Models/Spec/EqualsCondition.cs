using CD_in_Core.Domain.Models.Specification;

namespace CD_in_Core.Domain.Models.Replacement
{
    public class EqualsCondition : IValueCondition<int>
    {
        private readonly int _target;

        public EqualsCondition(int target)
        {
            _target = target;
        }

        public bool IsSatisfiedBy(int item)
        {
            return item == _target;
        }
    }
}
