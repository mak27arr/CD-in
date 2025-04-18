using CD_in_Core.Application.Pool;
using CD_in_Core.Application.Services.Interfaces;
using CD_in_Core.Domain.Models.Sequences;
using CD_in_Core.Domain.Select;

namespace CD_in_Core.Application.Services.Sequences
{
    internal class LargeNumberExtractionService : ILargeNumberExtractionService
    {
        private readonly ISequencePool _pool;

        public LargeNumberExtractionService(ISequencePool pool)
        {
            _pool = pool;
        }

        public ISequence ExtractLargeNumbers(ISequence sequence, LargeNumberExtraction options)
        {
            var resultSequence = _pool.Get();

            foreach (var element in sequence)
            {
                if (options.Condition.IsSatisfiedBy(element))
                    resultSequence.Add(element);
            }

            return resultSequence;
        }
    }
}
