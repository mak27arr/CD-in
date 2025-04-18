using CD_in_Core.Application.Pool;
using CD_in_Core.Application.Services.Interfaces;
using CD_in_Core.Domain.Models.Replacement;
using CD_in_Core.Domain.Models.Sequences;

namespace CD_in_Core.Application.Services.Sequences
{
    internal class BeneficialReplacementService : IBeneficialReplacementService
    {
        private readonly ISequencePool _pool;

        public BeneficialReplacementService(ISequencePool pool)
        {
            _pool = pool;
        }

        public ISequence PerformBeneficialReplacement(ISequence sequence, ValueTransformationOptions options)
        {
            var modified = _pool.Get();
            modified.SetPool(_pool);

            options.Specification.SetSequence(sequence);

            foreach (var element in sequence)
            {
                if (options.Specification.IsSatisfiedBy(element))
                {
                    modified.Add(element.Key, options.ReplacementStrategy.Transform(element.Value));
                }
                else
                {
                    modified.Add(element);
                }
            }

            return modified;
        }
    }
}
