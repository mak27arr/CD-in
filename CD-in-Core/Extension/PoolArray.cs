using Microsoft.Extensions.ObjectPool;
using System.Collections;

namespace CD_in_Core.Extension
{
    public class PoolArray<T> : IEnumerable<T>
    {
        private readonly ObjectPool<T[]> _pool;
        private T[] _array;
        private int _index;

        internal PoolArray(ObjectPool<T[]> pool)
        {
            _pool = pool;
            _array = _pool.Get();
        }

        public T[] Data => _array;

        public int Count => _index;

        public void Release()
        {
            if (_array != null)
            {
                _pool.Return(_array);
                _array = null;
            }
        }

        #region IEnumerable

        public IEnumerator<T> GetEnumerator()
        {
            if (_array == null)
                throw new ObjectDisposedException(nameof(PoolArray<T>));

            for (int i = 0; i < _index; i++)
            {
                yield return _array[i];
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        #endregion

        internal void Copy(byte[] block, int index)
        {
            Array.Copy(block, Data, index);
            _index = index;
        }
    }
}
