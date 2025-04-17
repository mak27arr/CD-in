using CD_in_Core.Domain.Models.Sequences;
using Microsoft.Extensions.ObjectPool;

namespace CD_in_Core.Application.Pool
{
    internal class PooledSequence : Sequence, IPoolSequence
    {
        private ObjectPool<PooledSequence> _pool;

        public PooledSequence(int size, ObjectPool<PooledSequence> pool) : base(size)
        {
            _pool = pool;
        }

        internal void SetPool(ObjectPool<PooledSequence> pool)
        {
            if (_pool == pool)
                return;

            if (_pool != null)
                throw new InvalidOperationException("Pool already set.");

            _pool = pool;
        }

        public void Release()
        {
            Clear();
            _pool.Return(this);
        }
    }
}
