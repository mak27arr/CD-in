using CD_in_Core.Application.Settings;
using CD_in_Core.Infrastructure;
using CD_in_Core.Infrastructure.FileServices.Interfaces;

namespace CD_in_Core.Application.Services
{
    internal class OutputDispatcherService : IOutputDispatcherService
    {
        private readonly ISequenceWriter _fileSequenceWriter;

        public OutputDispatcherService(ISequenceWriter fileSequenceWriter)
        {
            _fileSequenceWriter = fileSequenceWriter;
        }

        public async Task AppendSequenceAsync(WriteRequest writeRequest, CancellationToken token)
        {
            switch (writeRequest.SaveTo)
            {
                case SaveToTextFileParam:
                    await _fileSequenceWriter.AppendSequenceAsync(writeRequest, token);
                    break;
                default:
                    throw new NotImplementedException($"Not Implement for {writeRequest.SaveTo.GetType().Name}");
            }
        }

        public async Task WaitToFinishAsync()
        {
            await _fileSequenceWriter.WaitToFinishAsync();
        }
    }
}
