using System;

namespace WebJobs.Extensions.DependencyInjection
{
    public class ServiceProviderAccessor
        : IServiceProviderAccessor
    {
        public ServiceProviderAccessor(
            IServiceProvider serviceProvider)
        {
            ServiceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
        }

        public IServiceProvider ServiceProvider { get; }
    }
}
