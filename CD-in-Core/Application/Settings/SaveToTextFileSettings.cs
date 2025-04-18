namespace CD_in_Core.Application.Settings
{
    public record SaveToTextFileSettings : ISequenceSaveSettings
    {
        public required string FileName { get; set; }

        public required string FilePath { get; set; }
    }
}