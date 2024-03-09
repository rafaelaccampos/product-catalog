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
            var mongoContext = GetService<MongoContext>();

            var productInDatabase = await mongoContext.Products.Find(product => true).FirstOrDefaultAsync();

            using (new AssertionScope())
            {
                productInDatabase.Should().BeEquivalentTo(product);
            }
        }
    }
}