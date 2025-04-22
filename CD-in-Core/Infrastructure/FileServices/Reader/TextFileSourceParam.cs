using CD_in_Core.Infrastructure.FileServices.Interfaces;

namespace CD_in_Core.Infrastructure.FileServices.Reader
{
    public class TextFileSourceParam : IInputSourceParam
    {
        public required string Path { get; init; }

        public int BlockSize { get; init; }
    }
}