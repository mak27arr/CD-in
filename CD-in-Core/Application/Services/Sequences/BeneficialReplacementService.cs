using CD_in_Core.Application.Services.Interfaces;
using CD_in_Core.Domain.Models;
using CD_in_Core.Domain.Models.Replacement;
using CD_in_Core.Domain.Models.Sequences;

namespace CD_in_Core.Application.Services.Sequences
{
    internal class BeneficialReplacementService : IBeneficialReplacementService
    {
        public ReplacementResult PerformBeneficialReplacement(ISequence sequence, ValueTransformationOptions options)
        {
            var modified = new ReplacementResult(sequence.Count);

            options.Specification.SetSequence(sequence);

            foreach (var kvp in sequence)
            {
                if (options.Specification.IsSatisfiedBy(kvp))
                {
                    modified.Add(new Element(kvp.Key, options.ReplacementStrategy.Transform(kvp.Value)));
                }
                else
                {
                    modified.Add(kvp);
                }
            }

            return modified;
        }
    }
}
