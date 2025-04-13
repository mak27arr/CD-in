using CD_in_Core.Application.Services.Interfaces;
using CD_in_Core.Domain.Models;
using CD_in_Core.Domain.Models.Sequences;

namespace CD_in_Core.Application.Services
{
    internal class LargeNumberExtractionService : ILargeNumberExtractionService
    {
        public Sequence ExtractLargeNumbers(Sequence sequence, LargeNumberExtractionOptions options)
        {
            var resultSequence = new Sequence();

            foreach (var kvp in sequence.Digits)
            {
                if (options.Condition.IsSatisfiedBy(kvp.Value))
                    resultSequence.Add(kvp.Key, kvp.Value);
            }

            return resultSequence;
        }
    }
}
