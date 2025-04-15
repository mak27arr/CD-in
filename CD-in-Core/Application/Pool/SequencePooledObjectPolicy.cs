using Microsoft.Extensions.ObjectPool;

namespace CD_in_Core.Application.Pool
{
    internal class SequencePooledObjectPolicy : IPooledObjectPolicy<PoolSequence>
    {
        private readonly int _size;

        public SequencePooledObjectPolicy(int size = 10)
        {
            _size = size;
        }

        public PoolSequence Create()
        {
            return new PoolSequence(_size, null!);
        }

        public bool Return(PoolSequence obj)
        {
            if (obj == null)
                return false;
            obj.Clear();
            return true;
        }
    }
}
