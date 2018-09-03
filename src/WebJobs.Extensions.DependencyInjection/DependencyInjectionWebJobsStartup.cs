using System;
using Microsoft.Azure.WebJobs.Extensions;
using Microsoft.Azure.WebJobs.Hosting;
using Microsoft.Extensions.Hosting;

[assembly: WebJobsStartup(typeof(DependencyInjectionWebJobsStartup), "Dependency Injection")]

namespace Microsoft.Azure.WebJobs.Extensions
{
    public class DependencyInjectionWebJobsStartup
        : IWebJobsStartup
    {
        public void Configure(
            IWebJobsBuilder builder)
        {
            builder = builder ?? throw new ArgumentNullException(nameof(builder));

            builder.AddInject();
        }
    }
}
