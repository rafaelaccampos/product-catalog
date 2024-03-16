using FluentAssertions;
using FluentAssertions.Execution;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using Newtonsoft.Json;
using ProductCatalog.Entities;
using ProductCatalog.Infra;
using ProductCatalog.Integration.Tests.Extensions;
using ProductCatalog.Integration.Tests.Setup;
using System.Net;

namespace ProductCatalog.Integration.Tests.Specs.Controllers
{
    public class ProductControllerTests : ApiBase
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
                response.Should().HaveStatusCode(HttpStatusCode.OK);
                productFromDatabase.Should().BeEquivalentTo(product, options => options
                .ExcludingMissingMembers()
                .Excluding(p => p.Id));
            }
        }
    }
}