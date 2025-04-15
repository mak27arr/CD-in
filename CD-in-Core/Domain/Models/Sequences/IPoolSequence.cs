namespace CD_in_Core.Domain.Models.Sequences
{
    interface IPoolSequence : ISequence
    {
        void Release();
    }
}
