using CD_in_Core.Infrastructure.FileServices.Interfaces;

namespace CD_in_Core.Application.Settings
{
    public record SaveToTextFileParam : ISequenceSaveParam
    {
        public required string FileName { get; set; }

        public required string FilePath { get; set; }
    }
}