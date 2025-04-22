using CD_in_Core.Domain.Models.Sequences;

namespace CD_in_Core.Domain.Conditions
{
    public interface ISequenceCondition<T> : IValueCondition<T>
    {
        void SetSequence(ISequence sequence);
    }
}
