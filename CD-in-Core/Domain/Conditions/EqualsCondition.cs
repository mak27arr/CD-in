using CD_in_Core.Domain.Models.Sequences;

namespace CD_in_Core.Domain.Conditions
{
    public class EqualsCondition : IValueCondition<KeyValuePair<int, int>>
    {
        private readonly int _target;

        public EqualsCondition(int target)
        {
            _target = target;
        }

        public bool IsSatisfiedBy(KeyValuePair<int, int> item)
        {
            return item.Value == _target;
        }
    }
}
