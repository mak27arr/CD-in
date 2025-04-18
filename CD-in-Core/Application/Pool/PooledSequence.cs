using CD_in_Core.Domain.Models.Sequences;

namespace CD_in_Core.Application.Pool
{
    internal class PooledSequence : Sequence, IPooledSequence
    {
        private ISequencePool _pool;

        public PooledSequence(int size, ISequencePool pool) : base(size)
        {
            _pool = pool;
        }

        public void SetPool(ISequencePool pool)
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
