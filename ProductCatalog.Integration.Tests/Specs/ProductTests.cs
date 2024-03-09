using FluentAssertions;
using MongoDB.Driver;
using ProductCatalog.Entities;
using ProductCatalog.Infra;
using ProductCatalog.Infra.Repositories;
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
                Title = "�gua com g�s",
                Category = "Bebida",
                Description = "�gua gaseificada",
                Owner = "John",
                Price = 2.50M
            };

            var productRepository = new ProductRepository(_context);
            await productRepository.Create(product);

            var mongoContext = GetService<MongoContext>();
            var productInDatabase = await mongoContext.Products.Find(product => true).FirstOrDefaultAsync();

           productInDatabase.Should().BeEquivalentTo(product);
        }

        [Test]
        public async Task ShouldBeAbleToGetAllProducts()
        {
            var products = new List<Product>
            {
                new Product
                {
                    Title = "Refrigerante",
                    Category = "Bebida",
                    Description = "Guaran�",
                    Owner = "John",
                    Price = 9.00M
                },
                new Product
                {
                    Title = "Vinho",
                    Category = "Bebida Alco�lica",
                    Description = "Vinho Argentino",
                    Owner = "Jane",
                    Price = 10.00M
                }
            };

            await _context.Products.InsertManyAsync(products);
            
            var mongoContext = GetService<MongoContext>();
            var productRepository = new ProductRepository(mongoContext);
            var productsInDatabase = await productRepository.GetAll();

            productsInDatabase.Should().BeEquivalentTo(products);
        }

        [Test]
        public async Task ShouldBeAbleToGetProductById()
        {
            var products = new List<Product>
            {
                new Product
                {
                    Title = "Refrigerante",
                    Category = "Bebida",
                    Description = "Guaran�",
                    Owner = "John",
                    Price = 9.00M
                },
                new Product
                {
                    Title = "Vinho",
                    Category = "Bebida Alco�lica",
                    Description = "Vinho Argentino",
                    Owner = "Jane",
                    Price = 10.00M
                }
            };

            await _context.Products.InsertManyAsync(products);

            var mongoContext = GetService<MongoContext>();
            var productRepository = new ProductRepository(mongoContext);

            var productInDatabase = await productRepository.GetProductById(products.First().Id);

            productInDatabase.Should().BeEquivalentTo(products.First());
        }
    }
}