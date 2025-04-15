using CD_in_Core.Application.Services.Interfaces;
using CD_in_Core.Domain.Models;
using CD_in_Core.Domain.Models.Sequences;

namespace CD_in_Core.Application.Services.Sequences
{
    internal class LargeNumberExtractionService : ILargeNumberExtractionService
    {
        public ISequence ExtractLargeNumbers(ISequence sequence, LargeNumberExtractionOptions options)
        {
            var resultSequence = new Sequence();

            foreach (var kvp in sequence)
            {
                if (options.Condition.IsSatisfiedBy(kvp.Value))
                    resultSequence.Add(kvp);
            }

            return resultSequence;
        }
    }
}
