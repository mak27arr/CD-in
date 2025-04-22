using CD_in_Core.Domain.Models.Sequences;
using Microsoft.Extensions.ObjectPool;

namespace CD_in_Core.Application.Pool
{
    internal class SequencePool : ObjectPool<IPooledSequence>, ISequencePool
    {
        private readonly ObjectPool<IPooledSequence> _innerPool;

        public SequencePool(ObjectPool<IPooledSequence> innerPool)
        {
            _innerPool = innerPool ?? throw new ArgumentNullException(nameof(innerPool));
        }

        public override IPooledSequence Get()
        {
            var sequence = _innerPool.Get();
            sequence.SetPool(this);
            return sequence;
        }

        public override void Return(IPooledSequence item)
        {
            _innerPool.Return(item);
        }
    }
}
