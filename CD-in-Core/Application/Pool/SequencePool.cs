using Microsoft.Extensions.ObjectPool;

namespace CD_in_Core.Application.Pool
{
    internal class SequencePool : ObjectPool<PooledSequence>
    {
        private readonly ObjectPool<PooledSequence> _innerPool;

        public SequencePool(ObjectPool<PooledSequence> innerPool)
        {
            _innerPool = innerPool ?? throw new ArgumentNullException(nameof(innerPool));
        }

        public override PooledSequence Get()
        {
            var sequence = _innerPool.Get();
            sequence.SetPool(this);
            return sequence;
        }

        public override void Return(PooledSequence item)
        {
            _innerPool.Return(item);
        }
    }
}
