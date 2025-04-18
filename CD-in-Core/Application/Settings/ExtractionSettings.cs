using CD_in_Core.Domain.Select;

namespace CD_in_Core.Application.Settings
{
    public class ExtractionSettings
    {
        public int ExecutionOrder { get; init; }

        public required IExtraction SelectOption { get; set; }

        public required SequenceSaveOptions SaveOptions {get; set;}
    }
}