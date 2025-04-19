using CD_in_Core.Application.Pool;
using CD_in_Core.Application.Services.Interfaces.Sequences;
using CD_in_Core.Domain.Models.Sequences;
using CD_in_Core.Domain.Select;

namespace CD_in_Core.Application.Services.Sequences
{
    internal class BeneficialReplacementService : IBeneficialReplacementService
    {
        private readonly ISequencePool _pool;

        public BeneficialReplacementService(ISequencePool pool)
        {
            _pool = pool;
        }

        public ISequence PerformBeneficialReplacement(ISequence sequence, ValueTransformationRule options)
        {
            var modified = _pool.Get();
            modified.SetPool(_pool);

            options.Specification.SetSequence(sequence);

            foreach (var element in sequence)
            {
                if (options.Specification.IsSatisfiedBy(element))
                {
                    modified.Add(options.ReplacementStrategy.Transform(element));
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
