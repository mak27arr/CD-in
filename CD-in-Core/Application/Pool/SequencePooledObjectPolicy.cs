using Microsoft.Extensions.ObjectPool;

namespace CD_in_Core.Application.Pool
{
    internal class SequencePooledObjectPolicy : IPooledObjectPolicy<PooledSequence>
    {
        private readonly int _size;

        public SequencePooledObjectPolicy(int size = 10)
        {
            _size = size;
        }

        public PooledSequence Create()
        {
            return new PooledSequence(_size, null!);
        }

        public bool Return(PooledSequence obj)
        {
            if (obj == null)
                return false;
            obj.Clear();
            return true;
        }
    }
}
