using CD_in_Core.Application.Pool;
using CD_in_Core.Application.Services.Interfaces.Sequences;
using CD_in_Core.Domain.Models.Sequences;
using CD_in_Core.Domain.Select;

namespace CD_in_Core.Application.Services.Sequences
{
    internal class SequenceExtractionService : ISequenceExtractionService
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
                    resultSequence.Add(element);
            }

            return resultSequence;
        }

        public ISequence CloneSequence(ISequence sequence)
        {
            var resultSequence = _pool.Get();

            foreach (var element in sequence)
                resultSequence.Add(element);

            return resultSequence;
        }
    }
}
