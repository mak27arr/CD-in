using Microsoft.Extensions.ObjectPool;

namespace CD_in_Core.Extension
{
    public sealed class SinglePoolManager
    {
        private ArrayPooledObjectPolicy<byte>? _policy;
        private static ObjectPool<byte[]>? _pool;

        public ObjectPool<byte[]> GetOrCreateArrayPool(int blockSize)
        {
            if (_pool != null)
            {
                if (_policy?.Size == blockSize)
                    return _pool;

                _pool = null;
            }

            var poolProvider = new DefaultObjectPoolProvider();
            _policy = new ArrayPooledObjectPolicy<byte>(blockSize);
            _pool = poolProvider.Create(_policy);

            return _pool;
        }
    }
}
