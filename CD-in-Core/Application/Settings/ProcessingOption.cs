using CD_in_Core.Application.Settings.DeltaIndex;
using CD_in_Core.Application.Settings.Input;

namespace CD_in_Core.Application.Settings
{
    public class ProcessingOption
    {
        public required IInputSourceSettings InputSource { get; set; }

        public required DeltaIndexParams DeltaParam { get; set; }

        public required List<ExtractionSettings> ExtractionOptions { get; set; }
    }
}