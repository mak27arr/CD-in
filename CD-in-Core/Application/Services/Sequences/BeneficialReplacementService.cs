using CD_in_Core.Application.Pool;
using CD_in_Core.Application.Services.Interfaces;
using CD_in_Core.Domain.Models.Replacement;
using CD_in_Core.Domain.Models.Sequences;

namespace CD_in_Core.Application.Services.Sequences
{
    internal class BeneficialReplacementService : IBeneficialReplacementService
    {
        private readonly SequencePool _pool;

        public BeneficialReplacementService(SequencePool pool)
        {
            _pool = pool;
        }

        public ISequence PerformBeneficialReplacement(ISequence sequence, ValueTransformationOptions options)
        {
            var modified = _pool.Get();
            modified.SetPool(_pool);

            options.Specification.SetSequence(sequence);

            foreach (var kvp in sequence)
            {
                if (options.Specification.IsSatisfiedBy(kvp))
                {
                    modified.Add(new Element() 
                    { 
                        Key = kvp.Key, 
                        Value = options.ReplacementStrategy.Transform(kvp.Value) 
                    });
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
