using CD_in_Core.Application.Pool;
using CD_in_Core.Application.Services.Interfaces;
using CD_in_Core.Domain.Models;
using CD_in_Core.Domain.Models.Sequences;

namespace CD_in_Core.Application.Services.Sequences
{
    internal class LargeNumberExtractionService : ILargeNumberExtractionService
    {
        private readonly ISequencePool _pool;

        public LargeNumberExtractionService(ISequencePool pool)
        {
            _pool = pool;
        }

        public ISequence ExtractLargeNumbers(ISequence sequence, LargeNumberExtractionOptions options)
        {
            var resultSequence = _pool.Get();

            foreach (var kvp in sequence)
            {
                if (options.Condition.IsSatisfiedBy(kvp.Value))
                    resultSequence.Add(kvp);
            }

            return resultSequence;
        }
    }
}
