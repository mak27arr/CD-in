using Microsoft.Extensions.ObjectPool;

namespace CD_in_Core.Extension
{
    public class PoolArray<T>
    {
        private readonly ObjectPool<T[]> _pool;
        private T[] _array;
        private int _index;

        public T[] Data => _array;

        public int LastElementIndex => _index;

        public T this[int index]
        {
            set
            {
                _array[index] = value;
                _index = index;
            }
        }

        internal PoolArray(ObjectPool<T[]> pool)
        {
            _pool = pool;
            _array = _pool.Get() ?? throw new ApplicationException("Error create array in pool");
        }

        public void Release()
        {
            if (_array != null)
            {
                _pool.Return(_array);
                _array = null;
            }
        }

        internal void Copy(byte[] block, int length)
        {
            Array.Copy(block, Data, length);
            _index = length;
        }
    }
}
