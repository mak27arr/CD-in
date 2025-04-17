using CD_in_Core.Application.Pool;
using CD_in_Core.Application.Services.Interfaces;
using CD_in_Core.Domain.Models.Sequences;

namespace CD_in_Core.Application.Services.Sequences
{
    internal class SubSequenceExtractorService : ISubSequenceExtractorService
    {
        private readonly List<IElement> _currentSequence = new List<IElement>();
        private readonly SequencePool _pool;

        public SubSequenceExtractorService(SequencePool pool)
        {
            _pool = pool;
        }

        public ISequence ExstractSequence(ISequence sequence, SubSequenceExtractionOptions options)
        {
            var resultSequence = _pool.Get();
            _currentSequence.Clear();

            foreach (var element in sequence)
            {
                if (options.Condition.IsSatisfiedBy(element.Value))
                {
                    _currentSequence.Add(element);
                }
                else
                {
                    CopySubSequenceToResult(options, resultSequence);
                    _currentSequence.Clear();
                }
            }

            CopySubSequenceToResult(options, resultSequence);

            return resultSequence;
        }

        private void CopySubSequenceToResult(SubSequenceExtractionOptions options, Sequence resultSequence)
        {
            if (_currentSequence.Count >= options.MinSequenceLength)
            {
                foreach (var kvp in _currentSequence)
                {
                    resultSequence.Add(kvp);
                }
            }
        }
    }
}
