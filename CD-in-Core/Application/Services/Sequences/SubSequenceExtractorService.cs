using CD_in_Core.Application.Services.Interfaces;
using CD_in_Core.Domain.Models.Sequences;

namespace CD_in_Core.Application.Services.Sequences
{
    internal class SubSequenceExtractorService : ISubSequenceExtractorService
    {
        private readonly List<Element> _currentSequence = new List<Element>();

        public ISequence ExstractSequence(ISequence sequence, SubSequenceExtractionOptions options)
        {
            var resultSequence = new Sequence(sequence.Count);
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
