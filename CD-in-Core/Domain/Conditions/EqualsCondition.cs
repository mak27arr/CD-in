using CD_in_Core.Domain.Models.Sequences;

namespace CD_in_Core.Domain.Conditions
{
    public class EqualsCondition : IValueCondition<IElement>
    {
        private readonly int _target;

        public EqualsCondition(int target)
        {
            _target = target;
        }

        public bool IsSatisfiedBy(IElement item)
        {
            return item.Value == _target;
        }
    }
}
