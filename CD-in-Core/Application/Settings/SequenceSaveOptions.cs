namespace CD_in_Core.Application.Settings
{
    public record SequenceSaveOptions
    {
        public required string FileName { get; set; }

        public required string FilePath { get; set; }
    }
}