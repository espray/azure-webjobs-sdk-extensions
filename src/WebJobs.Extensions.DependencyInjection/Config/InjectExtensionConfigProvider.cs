using System;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Description;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Azure.WebJobs.Host.Config;
using Microsoft.Azure.WebJobs.Logging;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using WebJobs.Extensions.DependencyInjection.Bindings;

namespace WebJobs.Extensions.DependencyInjection.Config
{
    [Extension("Injection")]
    internal class InjectExtensionConfigProvider
        : IExtensionConfigProvider
    {
        private readonly IOptions<InjectOptions> _options;
        private readonly ILoggerFactory _loggerFactory;

        public InjectExtensionConfigProvider(
            IOptions<InjectOptions> options,
            ILoggerFactory loggerFactory)
        {
            _options = options ?? throw new ArgumentNullException(nameof(options));
            _loggerFactory = loggerFactory ?? throw new ArgumentNullException(nameof(loggerFactory));
        }

        public void Initialize(ExtensionConfigContext context)
        {
            var logger = _loggerFactory
                .CreateLogger(LogCategories.CreateTriggerCategory("Inject"));

            context
                .AddBindingRule<InjectAttribute>()
                .Bind(new InjectAttributeBindingProvider(context, _options.Value, logger));
        }
    }
}
