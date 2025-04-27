using CD_in_Core.Application.Pool;
using CD_in_Core.Application.Services.Interfaces.Sequences;
using CD_in_Core.Domain.Models.Sequences;
using CD_in_Core.Domain.Select;

namespace CD_in_Core.Application.Services.Sequences
{
    internal class BeneficialReplacementService : IReplacementService
    {
        private readonly ISequencePool _pool;

        public BeneficialReplacementService(ISequencePool pool)
        {
            _pool = pool;
        }

        public ISequence PerformReplacement(ISequence sequence, ValueTransformationRule options)
        {
            var modified = _pool.Get();
            options.Specification.SetSequence(sequence);

            foreach (var element in sequence)
            {
                if (options.Specification.IsSatisfiedBy(element))
                {
                    var newElement = options.ReplacementStrategy.Transform(element);
                    modified.Add(newElement);
                    sequence.Add(newElement.Clone());
                }
                else
                {
                    modified.Add(element.Clone());
                }
            }

            return modified;
        }
    }
}
