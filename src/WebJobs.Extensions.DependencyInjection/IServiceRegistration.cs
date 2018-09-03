using Microsoft.Extensions.DependencyInjection;

namespace WebJobs.Extensions.DependencyInjection
{
    /// <summary>
    /// Service registration interface
    /// </summary>
    public interface IServiceRegistration
    {
        /// <summary>
        /// Configure services for the application. This may be called multiple times.
        /// </summary>
        /// <param name="services"></param>
        void ConfigureServices(IServiceCollection services);
    }
}
