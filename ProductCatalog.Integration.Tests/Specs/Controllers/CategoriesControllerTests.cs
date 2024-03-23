using FluentAssertions;
using FluentAssertions.Execution;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using Newtonsoft.Json;
using NUnit.Framework;
using ProductCatalog.Entities;
using ProductCatalog.Infra;
using ProductCatalog.Integration.Tests.Extensions;
using ProductCatalog.Integration.Tests.Setup;
using System.Net;

namespace ProductCatalog.Integration.Tests.Specs.Controllers
{
    public class CategoriesControllerTests : ApiBase
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

        [Test]
        public async Task ShouldBeAbleToGetAllCategories()
        {
            var categories = new List<Category>
            {
                new Category("Calçados esportivos", "Para correr com intensidade", "John"),
                new Category("Bebidas alcóolicas", "Para te dar asas", "Levi")
            };

            await _context.Categories.InsertManyAsync(categories);

            var mongoContext = GetService<MongoContext>();
            var categoriesFromDatabase = await mongoContext.Categories.Find(c => true).ToListAsync();

            var response = await _httpClient.GetAsync(URL_BASE);
            var responseContent = await response.Content.ReadAsStringAsync();

            var categoriesFromResponse = JsonConvert.DeserializeObject<IList<Category>>(responseContent);

            using (new AssertionScope())
            {
                response.Should().HaveStatusCode(HttpStatusCode.OK);
                categoriesFromResponse.Should().BeEquivalentTo(categoriesFromDatabase, options
                    => options
                    .ExcludingMissingMembers()
                    .Excluding(c => c.Id));
            }
        }

        [Test]
        public async Task ShouldBeAbleToReturnNullWhenGetAllDoesNotHasValue()
        {
            var response = await _httpClient.GetAsync(URL_BASE);
            var responseContent = await response.Content.ReadAsStringAsync();

            var categories = JsonConvert.DeserializeObject<IList<Category>>(responseContent);

            using (new AssertionScope())
            {
                response.Should().HaveStatusCode(HttpStatusCode.OK);
                categories.Should().BeEmpty();
            }
        }

        [Test]
        public async Task ShouldBeAbleToGetCategoryById()
        {
            var categories = new List<Category>
            {
                new Category("Calçados esportivos", "Para correr com intensidade", "John"),
                new Category("Bebidas alcóolicas", "Para te dar asas", "Levi")
            };

            await _context.Categories.InsertManyAsync(categories);

            var mongoContext = GetService<MongoContext>();
            var categoryFromDatabase = await mongoContext.Catalogs.Find(c => true).FirstOrDefaultAsync();

            var response = await _httpClient.GetAsync($"{URL_BASE}/{categoryFromDatabase.Id}");
            var responseContent = await response.Content.ReadAsStringAsync();

            var categoryFromResponse = JsonConvert.DeserializeObject<Category>(responseContent);

            using (new AssertionScope())
            {
                response.Should().HaveStatusCode(HttpStatusCode.OK);
                categoryFromResponse.Should().BeEquivalentTo(categories.First(), options => 
                    options
                    .ExcludingMissingMembers()
                    .Excluding(c => c.Id));
            }
        }
    }
}
