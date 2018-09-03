using Xunit;

namespace WebJobs.Extensions.DependencyInjection.Tests
{
    public class InjectAttributeTests
    {
        [Fact]
        public void CreateInstance_works()
        {
            var attribute = new InjectAttribute();

            Assert.IsType<InjectAttribute>(attribute);
        }
    }
}
