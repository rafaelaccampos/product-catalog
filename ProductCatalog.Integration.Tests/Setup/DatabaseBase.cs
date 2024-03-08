using Microsoft.Extensions.DependencyInjection;
using ProductCatalog.Infra;

namespace ProductCatalog.Integration.Tests.Setup
{
    public class DatabaseBase
    {
        protected static IServiceScope _scope;
        protected static MongoDbContext _context;

        [SetUp]
        public void SetUpScope()
        {
            _scope = TestEnvironment.Factory.Services.CreateScope();
            _context = TestEnvironment.Factory.Services.CreateScope().ServiceProvider.GetRequiredService<MongoDbContext>();
        }

        public static T GetService<T>()
        {
            return _scope.ServiceProvider.GetService<T>()!;
        }
    }
}
