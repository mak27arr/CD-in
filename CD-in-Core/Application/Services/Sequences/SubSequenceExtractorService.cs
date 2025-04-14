using CD_in_Core.Application.Services.Interfaces;
using CD_in_Core.Domain.Models.Sequences;

namespace CD_in_Core.Application.Services.Sequences
{
    internal class SubSequenceExtractorService : ISubSequenceExtractorService
    {
        private readonly Dictionary<int, int> _currentSequence = new Dictionary<int, int>();

        public Sequence ExstractSequence(Sequence sequence, SubSequenceExtractionOptions options)
        {
            var resultSequence = new Sequence(sequence.Count);
            _currentSequence.Clear();

            foreach (var element in sequence.Digits)
            {
                if (options.Condition.IsSatisfiedBy(element.Value))
                {
                    _currentSequence.Add(element.Key, element.Value);
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
                    resultSequence.Add(kvp.Key, kvp.Value);
                }
            }
        }
    }
}
