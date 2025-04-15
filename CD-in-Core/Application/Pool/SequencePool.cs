using Microsoft.Extensions.ObjectPool;

namespace CD_in_Core.Application.Pool
{
    internal class SequencePool : ObjectPool<PoolSequence>
    {
        private readonly ObjectPool<PoolSequence> _innerPool;

        public SequencePool(ObjectPool<PoolSequence> innerPool)
        {
            _innerPool = innerPool ?? throw new ArgumentNullException(nameof(innerPool));
        }

        public override PoolSequence Get()
        {
            var sequence = _innerPool.Get();
            sequence.SetPool(this);
            return sequence;
        }

        public override void Return(PoolSequence item)
        {
            _innerPool.Return(item);
        }
    }
}
