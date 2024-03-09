using FluentAssertions;
using FluentAssertions.Execution;
using MongoDB.Driver;
using ProductCatalog.Infra;
using ProductCatalog.Integration.Tests.Setup;

namespace ProductCatalog.Integration.Tests.Specs
{
    public class ProductTests : DatabaseBase
    {

        [Test]
        public async Task ShouldBeAbleToCreateAProduct()
        {
            var product = new Product
            {
                Title = "Água com gás",
                Category = "Comida",
                Description = "Água gaseificada",
                Owner = "John",
                Price = 2.50M
            };

            await _context.CreateProduct(product);
            var mongoContext = GetService<MongoDbContext>();

            var productInDatabase = await mongoContext._products.Find(product => true).FirstOrDefaultAsync();

            using (new AssertionScope())
            {
                productInDatabase.Should().BeEquivalentTo(product);
            }
        }
    }
}