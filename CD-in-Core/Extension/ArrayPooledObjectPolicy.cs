using Microsoft.Extensions.ObjectPool;

namespace CD_in_Core.Extension
{
    public class ArrayPooledObjectPolicy<T> : IPooledObjectPolicy<T[]>
    {
        private readonly int _size;

        public ArrayPooledObjectPolicy(int size)
        {
            _size = size;
        }

        public T[] Create()
        {
            return new T[_size];
        }

        public bool Return(T[] obj)
        {
            return obj != null;
        }
    }
}
