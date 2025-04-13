using CD_in_Core.Domain.Models.Sequences;

namespace CD_in_Core.Domain.Models.Specification
{
    public interface ISequenceCondition<T> : IValueCondition<T>
    {
        void SetSequence(Sequence sequence);
    }
}
