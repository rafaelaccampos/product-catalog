using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;
using ProductCatalog.Infra;

namespace ProductCatalog.Integration.Tests.Setup
{
    public class DatabaseBase
    {
        protected static IServiceScope _scope;
        protected static MongoContext _context;

        [SetUp]
        public async Task SetUpScope()
        {
            _scope = TestEnvironment.Factory.Services.CreateScope();
            _context = TestEnvironment.Factory.Services.CreateScope().ServiceProvider.GetRequiredService<MongoContext>();

            await ClearCollections();
        }

        private async Task ClearCollections()
        {
            await _context.Products.DeleteManyAsync(Builders<Product>.Filter.Empty);
        }

        public static T GetService<T>()
        {
            return _scope.ServiceProvider.GetService<T>()!;
        }
    }
}
