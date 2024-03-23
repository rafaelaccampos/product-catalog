using FluentAssertions;
using FluentAssertions.Execution;
using MongoDB.Driver;
using Newtonsoft.Json;
using ProductCatalog.Entities;
using ProductCatalog.Infra;
using ProductCatalog.Integration.Tests.Extensions;
using ProductCatalog.Integration.Tests.Setup;
using System.Net;

namespace ProductCatalog.Integration.Tests.Specs.Controllers
{
    public class ProductsControllerTests : ApiBase
    {
        private const string URL_BASE = "/products";

        [Test]
        public async Task ShouldBeAbleToCreateAProduct()
        {
            var product = new Product(
                "Tênis de corrida",
                "Um calçado esportivo projetado para corrida, com sola amortecida",
                10.00M,
                "Calçados esportivos",
                "David");

            var response = await _httpClient.PostAsync(URL_BASE, product.ToJsonContent());

            var context = GetService<MongoContext>();
            var productFromDatabase = await context.Products.Find(product => true).FirstOrDefaultAsync();

            using (new AssertionScope())
            {
                response.Should().HaveStatusCode(HttpStatusCode.Created);
                productFromDatabase.Should().BeEquivalentTo(product, options => options
                .ExcludingMissingMembers()
                .Excluding(p => p.Id));
            }
        }

        [Test]
        public async Task ShouldBeAbleToGetProductById()
        {
            var product = new Product(
                "Tênis de corrida",
                "Um calçado esportivo projetado para corrida, com sola amortecida",
                10.00M,
                "Calçados esportivos",
                "David");

           await _context.Products.InsertOneAsync(product);

           var context = GetService<MongoContext>();
           var productFromDatabase = await context.Products.Find(product => true).FirstOrDefaultAsync();

           var response = await _httpClient.GetAsync($"{URL_BASE}/{productFromDatabase.Id}");
           var responseContent = await response.Content.ReadAsStringAsync();

           var productFromResponse = JsonConvert.DeserializeObject<Product>(responseContent);
           
           using (new AssertionScope())
           {
                response.Should().HaveStatusCode(HttpStatusCode.OK);
                productFromResponse.Should().BeEquivalentTo(product, options 
                    => options.ExcludingMissingMembers()
                    .Excluding(p => p.Id));
           }
        }

        [Test]
        public async Task ShouldBeAbleToReturnNullAndNotFoundWhenProductDoesNotExists()
        {
            var productFromDatabase = await _context.Products.Find(p => true).SingleOrDefaultAsync();

            var response = await _httpClient.GetAsync($"{URL_BASE}/65ecf78759159f2e38c2e514");

            using (new AssertionScope())
            {
                response.Should().HaveStatusCode(HttpStatusCode.NotFound);
                productFromDatabase.Should().BeNull();
            }
        }

        [Test]
        public async Task ShouldBeAbleToGetAListOfAllProducts()
        {
            var products = new List<Product>
            {
                new Product("Refrigerante", "Guaraná", 9.00M, "Bebida", "John"),
                new Product("Vinho", "Vinho Argentino", 10.00M, "Bebida Alcoólica", "Jane")
            };

            await _context.Products.InsertManyAsync(products);

            var response = await _httpClient.GetAsync(URL_BASE);
            var responseContent = await response.Content.ReadAsStringAsync();

            var productsFromDatabase = JsonConvert.DeserializeObject<IList<Product>>(responseContent);

            using (new AssertionScope())
            {
                response.Should().HaveStatusCode(HttpStatusCode.OK);
                productsFromDatabase.Should().BeEquivalentTo(products, options 
                    => options
                    .ExcludingMissingMembers()
                    .Excluding(p => p.Id));
            }
        }

        [Test]
        public async Task ShouldNotBeAbleToReturnEmptyWhenProductsDoesNotExists()
        {
            var response = await _httpClient.GetAsync(URL_BASE);
            var responseContent = await response.Content.ReadAsStringAsync();

            var productsFromDatabase = JsonConvert.DeserializeObject<IList<Product>>(responseContent);

            using (new AssertionScope())
            {
                response.Should().HaveStatusCode(HttpStatusCode.OK);
                productsFromDatabase.Should().BeEmpty();
            }
        }

        [Test]
        public async Task ShouldBeAbleToUpdateCategoryFromAProduct()
        {
            var product = new Product(
                "Tênis de corrida",
                "Um calçado esportivo projetado para corrida, com sola amortecida",
                10.00M,
                "Calçados esportivos",
                "David");

            await _context.Products.InsertOneAsync(product);

            var context = GetService<MongoContext>();
            var productFromDatabase = await context.Products.Find(product => true).SingleOrDefaultAsync();

            var categoryUpdated = "Calçados";

            var response = await _httpClient.PutAsync($"{URL_BASE}/{productFromDatabase.Id}", categoryUpdated.ToJsonContent());

            var productUpdated = await context.Products.Find(product => true).FirstOrDefaultAsync();

            using (new AssertionScope())
            {
                response.Should().HaveStatusCode(HttpStatusCode.OK);
                productUpdated.Category.Should().BeEquivalentTo(categoryUpdated);
            }
        }

        [Test]
        public async Task ShouldBeAbleToDeleteAProduct()
        {
            var product = new Product(
                "Tênis de corrida",
                "Um calçado esportivo projetado para corrida, com sola amortecida",
                10.00M,
                "Calçados esportivos",
                "David");

            await _context.Products.InsertOneAsync(product);

            var context = GetService<MongoContext>();
            var productFromDatabase = await context.Products.Find(product => true).SingleOrDefaultAsync();

            var response = await _httpClient.DeleteAsync($"{URL_BASE}/{productFromDatabase.Id}");

            var productDeleted = await context.Products.Find(p => true).SingleOrDefaultAsync();

            using (new AssertionScope())
            {
                response.Should().HaveStatusCode(HttpStatusCode.NoContent);
                productDeleted.Should().BeNull();
            }
        }
    }
}