using CD_in_Core.Application.Services.Interfaces;
using CD_in_Core.Domain.Models.Sequences;
using CD_in_Core.Infrastructure.FileServices.Interfaces;
using CD_in_Core.Infrastructure.FileServices.Reader;

namespace CD_in_Core.Application.Services.IO
{
    internal class InputDispatcherService : IInputDispatcherService
    {
        private readonly IDeltaIndexTextFileReader _fileDeltaReader;

        public InputDispatcherService(IDeltaIndexTextFileReader fileDeltaReader)
        {
            _fileDeltaReader = fileDeltaReader;
        }

        public IAsyncEnumerable<KeyValuePair<int, int>> GetInputDelta(IInputSourceParam inputSourceParam, Action<double> progress, CancellationToken token = default)
        {
            switch (inputSourceParam)
            {
                case TextFileSourceParam fileSourceParam:
                    return _fileDeltaReader.ProcessFile(fileSourceParam, progress, token);
                default:
                    throw new NotImplementedException($"Can`t read for input: {inputSourceParam?.GetType().Name}");
            }
        }
    }
}
