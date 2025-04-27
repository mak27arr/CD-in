using CD_in_Core.Domain.Models.Sequences;
using CD_in_Core.Domain.Select;

namespace CD_in_Core.Application.Services.Sequences
{
    internal class BaseProcessingService
    {
        protected void UpdateSource(ISequence source, IPooledSequence satisfiedElements, ExtractionRetentionPolicy retentionPolicy)
        {
            if (retentionPolicy == ExtractionRetentionPolicy.Remove)
            {
                foreach (var item in satisfiedElements)
                    source.Remove(item);

                source.ReindexDisplayKeys();
            }
        }
    }
}
