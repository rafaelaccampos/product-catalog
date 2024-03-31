using FluentAssertions;
using FluentAssertions.Execution;
using MongoDB.Driver;
using Newtonsoft.Json;
using ProductCatalog.Dtos;
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
            var category = new CategoryInput
            {
                Title = "Calçados esportivos",
                Description = "Para correr com intensidade",
                Owner = "John",
            };

            var response = await _httpClient.PostAsync(URL_BASE, category.ToJsonContent());

            var mongoContext = GetService<MongoContext>();
            var categoryFromDatabase = await mongoContext.Categories.Find(c => true).SingleOrDefaultAsync();

            var categoryOutput = new CategoryOutput
            {
                Title = category.Title,
                Description = category.Description,
                Owner = category.Owner,
            };

            using (new AssertionScope())
            {
                response.Should().HaveStatusCode(HttpStatusCode.Created);
                categoryFromDatabase.Should().BeEquivalentTo(categoryOutput, options
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

            var categoriesFromResponse = JsonConvert.DeserializeObject<IList<CategoryOutput>>(responseContent);

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
        public async Task ShouldBeAbleToReturnEmptyWhenGetAllDoesNotHasValue()
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
            var categoryFromDatabase = await mongoContext.Categories.Find(c => true).FirstOrDefaultAsync();

            var response = await _httpClient.GetAsync($"{URL_BASE}/{categoryFromDatabase.Id}");
            var responseContent = await response.Content.ReadAsStringAsync();

            var categoryFromResponse = JsonConvert.DeserializeObject<CategoryOutput>(responseContent);

            using (new AssertionScope())
            {
                response.Should().HaveStatusCode(HttpStatusCode.OK);
                categoryFromResponse.Should().BeEquivalentTo(categories.First(), options => 
                    options
                    .ExcludingMissingMembers()
                    .Excluding(c => c.Id));
            }
        }

        [Test]
        public async Task ShouldBeAbleToReturnNullWhenCategoryDoesNotExists()
        {
            var response = await _httpClient.GetAsync($"{URL_BASE}/65ecf78759159f2e38c2e514");
            var responseContent = await response.Content.ReadAsStringAsync();

            var category = await _context.Categories.Find(c => true).FirstOrDefaultAsync();

            using (new AssertionScope())
            {
                response.Should().HaveStatusCode(HttpStatusCode.NotFound);
                category.Should().BeNull();
            }
        }

        [Test]
        public async Task ShouldBeAbleToDeleteACategory()
        {
            var category = new Category("Calçados", "Calçados esportivos", "John");

            await _context.Categories.InsertOneAsync(category);

            var mongoContext = GetService<MongoContext>();
            var categoryFromDatabase = await mongoContext.Categories.Find(c => true).FirstOrDefaultAsync();

            var response = await _httpClient.DeleteAsync($"{URL_BASE}/{categoryFromDatabase.Id}");
            var responseContent = await response.Content.ReadAsStringAsync();

            var categoryDeleted = await mongoContext.Categories.Find(c => true).FirstOrDefaultAsync();

            using (new AssertionScope())
            {
                response.Should().HaveStatusCode(HttpStatusCode.NoContent);
                categoryDeleted.Should().BeNull();
            }
        }
    }
}
