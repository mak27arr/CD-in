using CD_in_Core.Application.Services.Interfaces;
using CD_in_Core.Domain.Models;
using CD_in_Core.Domain.Models.Replacement;
using CD_in_Core.Domain.Models.Sequences;

namespace CD_in_Core.Application.Services
{
    internal class BeneficialReplacementService : IBeneficialReplacementService
    {
        public ReplacementResult PerformBeneficialReplacement(Sequence sequence, ValueTransformationOptions options)
        {
            var modified = new ReplacementResult();

            options.Specification.SetSequence(sequence);

            foreach (var kvp in sequence.Digits)
            {
                if (options.Specification.IsSatisfiedBy(kvp))
                {
                    modified.Add(kvp.Key, options.ReplacementStrategy.Transform(kvp.Value));
                }
                else
                {
                    modified.Add(kvp.Key, kvp.Value);
                }
            }

            return modified;
        }
    }
}
