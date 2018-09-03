using System;

namespace WebJobs.Extensions.DependencyInjection
{
    public interface IServiceProviderAccessor
    {
        IServiceProvider ServiceProvider { get; }
    }
}
