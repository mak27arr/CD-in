namespace CD_in_Core.Application.Settings.Input
{
    public class DirectoryInputSourceSettings : IInputSourceSettings
    {
        public required string FolderPath { get; set; }

        public required string InputFilesType { get; set; }
    }
}
