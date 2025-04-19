using CD_in_Core.Infrastructure.FileServices.Interfaces;
using CD_in_Core.Infrastructure.FileServices.Reader;
using CD_in_Core.Infrastructure.FileServices.Writer;
using Microsoft.Extensions.DependencyInjection;

namespace CD_in_Core.Infrastructure.Extension
{
    public static class AppExtension
    {
        public static IServiceCollection RegisterAppInfrastructureServices(this IServiceCollection service)
        {
            service.AddTransient<ISequenceWriter, SequenceWriter>();
            service.AddTransient<IFileReader, FileReader>();
            service.AddTransient<ILineCountEstimator, BinaryLineCountEstimator>();
            service.AddTransient<IFileReadProgressTracker, FileReadProgressTracker>();
            return service;
        }
    }
}
