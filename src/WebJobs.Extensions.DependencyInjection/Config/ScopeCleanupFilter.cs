using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs.Host;
using WebJobs.Extensions.DependencyInjection.Bindings;

namespace WebJobs.Extensions.DependencyInjection.Config
{
    internal class ScopeCleanupFilter
        : IFunctionInvocationFilter, IFunctionExceptionFilter
    {
        public Task OnExceptionAsync(FunctionExceptionContext exceptionContext, CancellationToken cancellationToken)
        {
            RemoveScope(exceptionContext.FunctionInstanceId);
            return Task.CompletedTask;
        }

        public Task OnExecutedAsync(FunctionExecutedContext executedContext, CancellationToken cancellationToken)
        {
            RemoveScope(executedContext.FunctionInstanceId);
            return Task.CompletedTask;
        }

        public Task OnExecutingAsync(FunctionExecutingContext executingContext, CancellationToken cancellationToken) =>
            Task.CompletedTask;

        private void RemoveScope(Guid id)
        {
            if (InjectAttributeBindingProvider.Scopes.TryRemove(id, out var scope))
            {
                scope.Dispose();
            }
        }
    }
}
