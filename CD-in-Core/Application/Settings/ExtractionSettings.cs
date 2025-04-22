using CD_in_Core.Domain.Select;

namespace CD_in_Core.Application.Settings
{
    public class ExtractionSettings
    {
        public int ExecutionOrder { get; init; }

        public required IExtractionRule SelectOption { get; set; }

        public required SaveToTextFileParam SaveOptions {get; set;}
    }
}