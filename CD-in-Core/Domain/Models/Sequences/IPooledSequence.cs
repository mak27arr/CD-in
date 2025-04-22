using CD_in_Core.Application.Pool;

namespace CD_in_Core.Domain.Models.Sequences
{
    interface IPooledSequence : ISequence
    {
        void Release();

        void SetPool(ISequencePool pool);
    }
}
