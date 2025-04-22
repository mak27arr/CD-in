using CD_in_Core.Domain.Models.Sequences;
using Microsoft.Extensions.ObjectPool;

namespace CD_in_Core.Application.Pool
{
    internal class SequencePooledObjectPolicy : IPooledObjectPolicy<IPooledSequence>
    {
        private readonly int _size;

        public SequencePooledObjectPolicy(int size = 10)
        {
            _size = size;
        }

        public IPooledSequence Create()
        {
            return new PooledSequence(_size, null!);
        }

        public bool Return(IPooledSequence obj)
        {
            if (obj == null)
                return false;
            obj.Clear();
            return true;
        }
    }
}
