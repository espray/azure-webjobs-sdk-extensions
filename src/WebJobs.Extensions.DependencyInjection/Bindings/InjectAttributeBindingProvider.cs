using Microsoft.Azure.WebJobs.Host.Bindings;
using Microsoft.Azure.WebJobs.Host.Config;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using WebJobs.Extensions.DependencyInjection.Config;

namespace WebJobs.Extensions.DependencyInjection.Bindings
{
    internal class InjectAttributeBindingProvider
        : IBindingProvider
    {
        private readonly ExtensionConfigContext _extensionConfigContext;
        private readonly InjectOptions _options;
        private readonly ILogger _logger;
        private IServiceProvider _serviceProvider;

        public InjectAttributeBindingProvider(
            ExtensionConfigContext context,
            InjectOptions options,
            ILogger logger)
        {
            _extensionConfigContext = context ?? throw new ArgumentNullException(nameof(context));
            _options = options ?? throw new ArgumentNullException(nameof(options));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public static readonly ConcurrentDictionary<Guid, IServiceScope> Scopes =
            new ConcurrentDictionary<Guid, IServiceScope>();

        public Task<IBinding> TryCreateAsync(
            BindingProviderContext context)
        {
            context = context ?? throw new ArgumentNullException(nameof(context));

            if (_serviceProvider == null)
            {
                var services = new ServiceCollection();

                var parameter = context.Parameter;
                var attribute = parameter.GetCustomAttribute<InjectAttribute>(inherit: false);

                Type serviceRegistrationType = null;

                if (serviceRegistrationType == null)
                {
                    var assembly = context.Parameter.Member.DeclaringType.Assembly;

                    var serviceRegistrationTypeList = assembly
                        .GetExportedTypes()
                        .Where(
                            t => typeof(IServiceRegistration).IsAssignableFrom(t) &&
                            !t.IsInterface &&
                            !t.IsAbstract);

                    serviceRegistrationType = serviceRegistrationTypeList.FirstOrDefault();

                    if (serviceRegistrationTypeList.Take(2).Count() == 2)
                    {
                        serviceRegistrationType = null;
                        _logger.LogWarning($"Found 2 or more service registrations.");
                    }
                }

                if (serviceRegistrationType != null)
                {
                    if (typeof(IServiceRegistration).IsAssignableFrom(serviceRegistrationType))
                    {
                        var serviceRegistration = (IServiceRegistration)Activator
                        .CreateInstance(serviceRegistrationType);

                        serviceRegistration.ConfigureServices(services);
                    }
                    else
                    {
                        _logger.LogWarning($"Type '{serviceRegistrationType}' must be an implementation of {typeof(IServiceRegistration).AssemblyQualifiedName}.");
                    }
                }
                else
                {
                    _logger.LogWarning($"Did not find a service registration.");
                }

                services.AddSingleton<IServiceProviderAccessor>(sp => new ServiceProviderAccessor(sp));

                _serviceProvider = services.BuildServiceProvider();
            }

            var binding = new InjectBinding(_serviceProvider, context.Parameter.ParameterType) as IBinding;
            return Task.FromResult(binding);
        }
    }
}
