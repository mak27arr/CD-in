namespace CD_in_Core.Application.Pool
{
    internal interface ISequencePool
    {
        PooledSequence Get();
        void Return(PooledSequence item);
    }
}