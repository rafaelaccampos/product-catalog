using FluentAssertions;
using FluentAssertions.Execution;
using ProductCatalog.Entities;
using ProductCatalog.Integration.Tests.Extensions;
using ProductCatalog.Integration.Tests.Setup;
using System.Net;
using System.Text.Json;

namespace ProductCatalog.Integration.Tests.Specs.Controllers
{
    public class ProductControllerTests : ApiBase
    {
        private const string URL_BASE = "/products";

        [Test]
        public async Task ShouldBeAbleToCreateAProduct()
        {
            var productExpected = new Product(
                "Tênis de corrida",
                "Um calçado esportivo projetado para corrida, com sola amortecida", 
                10.00M, 
                "Calçados esportivos", 
                "David");

            var response = await _httpClient.PostAsync(URL_BASE, productExpected.ToJsonContent());
            var responseContent = await response.Content.ReadAsStringAsync();
            var product = JsonSerializer.Deserialize<Product>(responseContent);

            using (new AssertionScope())
            {
                response.Should().HaveStatusCode(HttpStatusCode.OK);
                product.Should().BeEquivalentTo(productExpected);
            }
        }
    }
}
