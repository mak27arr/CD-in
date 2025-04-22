using CD_in_Core.Domain.Models.Sequences;

namespace CD_in_Core.Application.Pool
{
    internal interface ISequencePool
    {
        IPooledSequence Get();
        void Return(IPooledSequence item);
    }
}