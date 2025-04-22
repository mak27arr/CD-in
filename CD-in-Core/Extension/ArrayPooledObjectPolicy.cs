using Microsoft.Extensions.ObjectPool;

namespace CD_in_Core.Extension
{
    public class ArrayPooledObjectPolicy<T> : IPooledObjectPolicy<T[]>
    {
        public int Size { get;  private set; }

        public ArrayPooledObjectPolicy(int size)
        {
            Size = size;
        }

        public T[] Create()
        {
            return new T[Size];
        }

        public bool Return(T[] obj)
        {
            return obj != null;
        }
    }
}
