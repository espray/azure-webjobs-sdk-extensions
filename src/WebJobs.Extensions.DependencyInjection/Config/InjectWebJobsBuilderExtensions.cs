using System;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.DependencyInjection;
using WebJobs.Extensions.DependencyInjection.Config;

namespace Microsoft.Extensions.Hosting
{
    /// <summary>
    /// Extension methods for ServiceProvider integration
    /// </summary>
    public static class InjectWebJobsBuilderExtensions
    {
        /// <summary>
        /// Adds the Injection extension to the provided <see cref="IWebJobsBuilder"/>.
        /// </summary>
        /// <param name="builder">The <see cref="IWebJobsBuilder"/> to configure.</param>
        public static IWebJobsBuilder AddInject(
            this IWebJobsBuilder builder)
        {
            builder = builder ?? throw new ArgumentNullException(nameof(builder));

            //Register the extension
            builder.AddExtension<InjectExtensionConfigProvider>()
                .BindOptions<InjectOptions>();

            //Register the cleanup a filter
            builder.Services.AddSingleton<IFunctionFilter, ScopeCleanupFilter>();

            return builder;
        }

        /// <summary>
        /// Adds the Injection extension to the provided <see cref="IWebJobsBuilder"/>.
        /// </summary>
        /// <param name="builder">The <see cref="IWebJobsBuilder"/> to configure.</param>
        /// <param name="configure">An <see cref="Action{InjectOptions}"/> to configure the provided <see cref="InjectOptions"/>.</param>
        public static IWebJobsBuilder AddInject(
            this IWebJobsBuilder builder,
            Action<InjectOptions> configure)
        {
            builder = builder ?? throw new ArgumentNullException(nameof(builder));
            configure = configure ?? throw new ArgumentNullException(nameof(configure));

            builder.AddInject();
            builder.Services.Configure(configure);

            return builder;
        }
    }
}
