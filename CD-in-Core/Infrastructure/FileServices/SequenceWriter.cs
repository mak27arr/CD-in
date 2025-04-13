using CD_in_Core.Domain.Models.Sequences;

namespace CD_in_Core.Infrastructure.FileServices
{
    public class SequenceWriter : ISequenceWriter
    {
        public async Task AppendSequenceAsync(Sequence sequence, string sourceFileName, SequenceSaveOptions options, CancellationToken cancellationToken = default)
        {
            var fileName = $"{options.FileName}-{sourceFileName}.txt";
            var fullName = Path.Combine(options.FilePath, fileName);

            await using var stream = new FileStream(fullName, FileMode.Append, FileAccess.Write, FileShare.None, 4096, true);
            await using var writer = new StreamWriter(stream);

            foreach (var digit in sequence.Digits)
            {
                if (cancellationToken.IsCancellationRequested)
                    break;

                await writer.WriteLineAsync($"{digit.Key}:{digit.Value}");
            }
        }
    }
}
