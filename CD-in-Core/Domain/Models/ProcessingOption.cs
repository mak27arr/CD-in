using CD_in_Core.Domain.Models.DeltaIndex;

namespace CD_in_Core.Domain.Models
{
    public class ProcessingOption
    {
        public string? FolderPath { get; set; }

        public string? InputFilesType { get; set; }

        public required DeltaIndexParams DeltaParam { get; set; }

        public required List<ExtractionOptions> ExtractionOptions { get; set; }
    }
}