namespace CD_in_Core.Domain.Models.Sequences
{
    interface IPooledSequence : ISequence
    {
        void Release();
    }
}
