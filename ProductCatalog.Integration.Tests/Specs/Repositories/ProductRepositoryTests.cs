using FluentAssertions;
using MongoDB.Bson;
using MongoDB.Driver;
using ProductCatalog.Entities;
using ProductCatalog.Infra;
using ProductCatalog.Infra.Repositories;
using ProductCatalog.Integration.Tests.Setup;

namespace ProductCatalog.Integration.Tests.Specs.Repositories
{
    public class ProductRepositoryTests : DatabaseBase
    {
        [Test]
        public async Task ShouldBeAbleToCreateAProduct()
        {
            var product = new Product("Água com gás", "Bebida", 2.50M, "Água gaseificada", "John");

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
                new Product("Refrigerante", "Guaraná", 9.00M, "Bebida", "John"),
                new Product("Vinho", "Vinho Argentino", 10.00M, "Bebida Alcoólica", "Jane")
            };

            await _context.Products.InsertManyAsync(products);

            var mongoContext = GetService<MongoContext>();
            var productRepository = new ProductRepository(mongoContext);
            var productsInDatabase = await productRepository.GetAll();

            productsInDatabase.Should().BeEquivalentTo(products);
        }

        [Test]
        public async Task ShouldReturnEmptyWhenGetAllProductsDoesNotHaveValue()
        {
            var mongoContext = GetService<MongoContext>();
            var productRepository = new ProductRepository(mongoContext);

            var productsInDatabase = await productRepository.GetAll();

            productsInDatabase.Should().BeEmpty();
        }

        [Test]
        public async Task ShouldBeAbleToGetProductById()
        {
            var products = new List<Product>
            {
                new Product("Refrigerante", "Guaraná", 9.00M, "Bebida", "John"),
                new Product("Vinho", "Vinho Argentino", 10.00M, "Bebida Alcoólica", "Jane")
            };

            await _context.Products.InsertManyAsync(products);

            var mongoContext = GetService<MongoContext>();
            var productRepository = new ProductRepository(mongoContext);

            var productInDatabase = await productRepository.GetProductById(products.First().Id);

            productInDatabase.Should().BeEquivalentTo(products.First());
        }

        [Test]
        public async Task ShouldReturnNullWhenGetProductByIdDoesNotHaveValue()
        {
            var mongoContext = GetService<MongoContext>();
            var productRepository = new ProductRepository(mongoContext);

            var productInDatabase = await productRepository.GetProductById(new ObjectId("65ecf78759159f2e38c2e514"));

            productInDatabase.Should().BeNull();
        }

        [Test]
        public async Task ShouldBeAbleToUpdateAProduct()
        {
            var product = new Product("Refrigerante", "Guaraná", 9.00M, "Bebida", "John");

            await _context.Products.InsertOneAsync(product);

            var mongoContext = GetService<MongoContext>();
            var productRepository = new ProductRepository(mongoContext);
            await productRepository.Update(product.Id, "Bebida gelada");

            var productUpdated = await mongoContext.Products.Find(p => true).FirstOrDefaultAsync();

            productUpdated.Category.Should().BeEquivalentTo("Bebida gelada");
        }

        [Test]
        public async Task ShouldNotBeABleToUpdateAProductWhenItsDoesNotExists()
        {
            var product = new Product("Refrigerante", "Guaraná", 9.00M, "Bebida", "John");

            var mongoContext = GetService<MongoContext>();
            var productRepository = new ProductRepository(mongoContext);
            await productRepository.Update(new ObjectId("65ecf78759159f2e38c2e514"), "Bebida gelada");

            var productUpdated = await mongoContext.Products.Find(p => true).FirstOrDefaultAsync();

            productUpdated.Should().BeNull();
        }

        [Test]
        public async Task ShouldBeAbleToDeleteAProduct()
        {
            var product = new Product("Refrigerante", "Guaraná", 9.00M, "Bebida", "John");

            await _context.Products.InsertOneAsync(product);

            var mongoContext = GetService<MongoContext>();
            var productRepository = new ProductRepository(mongoContext);
            await productRepository.Delete(product.Id);

            var productInDatabase = await mongoContext.Products.Find(p => true).FirstOrDefaultAsync();

            productInDatabase.Should().BeNull();
        }
    }
}