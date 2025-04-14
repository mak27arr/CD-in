using CD_in_Core.Domain.Models.Sequences;

namespace CD_in_Core.Domain.Models
{
    public class ReplacementResult : Sequence
    {
        internal ReplacementResult(int size = 1)
        {
            Digits = new Dictionary<int, int>(size);
        }
    }
}
