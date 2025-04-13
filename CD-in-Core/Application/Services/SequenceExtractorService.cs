using CD_in_Core.Application.Services.Interfaces;
using CD_in_Core.Domain.Models.Sequences;

namespace CD_in_Core.Application.Services
{
    internal class SequenceExtractorService : ISequenceExtractorService
    {
        public Sequence ExstractSequence(Sequence sequence, SequenceExtractionOptions options)
        {
            var resultSequence = new Sequence();
            var currentSequence = new Dictionary<int, int>(); 

            foreach(var element in sequence.Digits)
            {
                if (element.Value == options.TargetDigit)
                {
                    currentSequence.Add(element.Key, element.Value);
                }
                else
                {
                    if (currentSequence.Count >= options.MinSequenceLength)
                    {
                        foreach (var kvp in currentSequence)
                        {
                            resultSequence.Digits[kvp.Key] = kvp.Value;
                        }
                    }

                    currentSequence.Clear();
                }
            }

            if (currentSequence.Count >= options.MinSequenceLength)
            {
                foreach (var kvp in currentSequence)
                    resultSequence.Add(kvp.Key, kvp.Value);
            }

            return resultSequence;
        }
    }
}
