using FluentAssertions;
using FluentAssertions.Execution;
using MongoDB.Driver;
using ProductCatalog.Entities;
using ProductCatalog.Infra;
using ProductCatalog.Integration.Tests.Extensions;
using ProductCatalog.Integration.Tests.Setup;
using System.Net;

namespace ProductCatalog.Integration.Tests.Specs.Controllers
{
    public class CategoryControllerTests : ApiBase
    {
        private const string URL_BASE = "/categories";

        [Test]
        public async Task ShouldBeAbleToCreateCategory()
        {
            var category = new Category(
                "Calçados esportivos",
                "Para correr com intensidade", 
                "John");

            var response = await _httpClient.PostAsync(URL_BASE, category.ToJsonContent());

            var mongoContext = GetService<MongoContext>();
            var categoryFromDatabase = await mongoContext.Categories.Find(c => true).SingleOrDefaultAsync();

            using (new AssertionScope())
            {
                response.Should().HaveStatusCode(HttpStatusCode.OK);
                categoryFromDatabase.Should().BeEquivalentTo(category, options
                    => options
                    .ExcludingMissingMembers()
                    .Excluding(c => c.Id));
            }
        }
    }
}
