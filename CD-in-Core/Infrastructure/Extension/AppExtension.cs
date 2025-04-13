using CD_in_Core.Infrastructure.FileServices;
using Microsoft.Extensions.DependencyInjection;

namespace CD_in_Core.Infrastructure.Extension
{
    public static class AppExtension
    {
        public static IServiceCollection RegisterAppInfrastructureServices(this IServiceCollection service)
        {
            service.AddTransient<ISequenceWriter, SequenceWriter>();
            service.AddTransient<IFileReader, FileReader>();

            return service;
        }
    }
}
