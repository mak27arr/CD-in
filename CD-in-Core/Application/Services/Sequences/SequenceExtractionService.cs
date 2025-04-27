using CD_in_Core.Application.Pool;
using CD_in_Core.Application.Services.Interfaces.Sequences;
using CD_in_Core.Domain.Models.Sequences;
using CD_in_Core.Domain.Select;
using CD_in_Core.Domain.ValueObjects;

namespace CD_in_Core.Application.Services.Sequences
{
    internal class SequenceExtractionService : BaseProcessingService, ISequenceExtractionService
    {
        private readonly ISequencePool _pool;

        public SequenceExtractionService(ISequencePool pool)
        {
            _pool = pool;
        }

        public ISequence Extract(ISequence sequence, SelectNumberRule options)
        {
            var resultSequence = _pool.Get();

            foreach (var element in sequence)
            {
                if (options.Condition.IsSatisfiedBy(element))
                    resultSequence.Add(element.Clone());
            }

            UpdateSource(sequence, resultSequence, options.RetentionPolicy);
            return resultSequence;
        }

        public ISequence CloneSequence(ISequence sequence, RawSequenceExtractionRules extractionRules)
        {
            if (extractionRules.RetentionPolicy == ExtractionRetentionPolicy.Remove)
                throw new NotImplementedException("Can't clear source currently");

            var resultSequence = _pool.Get();

            foreach (var element in sequence)
                resultSequence.Add(element.Clone());

            return resultSequence;
        }
    }
}
