using System;
using Microsoft.Azure.WebJobs.Description;

namespace WebJobs.Extensions.DependencyInjection
{
    [AttributeUsage(AttributeTargets.Parameter)]
    [Binding]
    public class InjectAttribute
        : Attribute
    {
        public InjectAttribute()
        {
        }
    }
}
