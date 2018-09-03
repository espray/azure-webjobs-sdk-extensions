using Microsoft.Extensions.DependencyInjection;

namespace WebJobs.Extensions.DependencyInjection
{
    public interface IServiceRegistration
    {
        void ConfigureServices(IServiceCollection services);
    }
}
