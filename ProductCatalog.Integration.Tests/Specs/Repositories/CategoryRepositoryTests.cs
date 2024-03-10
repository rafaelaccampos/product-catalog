using FluentAssertions;
using MongoDB.Bson;
using MongoDB.Driver;
using ProductCatalog.Entities;
using ProductCatalog.Infra;
using ProductCatalog.Integration.Tests.Setup;

namespace ProductCatalog.Integration.Tests.Specs.Repositories
{
    public class CategoryRepositoryTests : DatabaseBase
    {
        [Test]
        public async Task ShouldBeAbleToCreateACategory()
        {
            var category = new Category("Tecnologia", "Eletrônicos diversos", "John");

            var categoryRepository = new CategoryRepository(_context);
            await categoryRepository.Create(category);

            var mongoContext = GetService<MongoContext>();
            var categoryInDatabase = await mongoContext.Categories.Find(category => true).FirstOrDefaultAsync();

            categoryInDatabase.Should().BeEquivalentTo(category);
        }

        [Test]
        public async Task ShouldBeAbleToGetAllCategories()
        {
            var categories = new List<Category>()
            {
                new Category("Tecnologia", "Eletrônicos diversos", "John"),
                new Category("Livros", "Obras literárias", "Jane"),
            };
            await _context.Categories.InsertManyAsync(categories);

            var mongoContext = GetService<MongoContext>();
            var categoryRepository = new CategoryRepository(mongoContext);
            var categoriesInDatabase = await categoryRepository.GetAll();

            categoriesInDatabase.Should().BeEquivalentTo(categories);
        }

        [Test]
        public async Task ShouldReturnEmptyWhenCategoriesDoesNotHaveValue()
        {
            var mongoContext = GetService<MongoContext>();
            var categoryRepository = new CategoryRepository(mongoContext);
            var categoriesInDatabase = await categoryRepository.GetAll();

            categoriesInDatabase.Should().BeEmpty();
        }

        [Test]
        public async Task ShouldBeAbleToGetCategoryById()
        {
            var categories = new List<Category>()
            {
                new Category("Tecnologia", "Eletrônicos diversos", "John"),
                new Category("Livros", "Obras literárias", "Jane"),
            };

            await _context.Categories.InsertManyAsync(categories);

            var mongoContext = GetService<MongoContext>();
            var categoryRepository = new CategoryRepository(mongoContext);
            var categoryInDatabase = await categoryRepository.GetCategoryById(categories.First().Id);

            categoryInDatabase.Should().BeEquivalentTo(categories.First());
        }

        [Test]
        public async Task ShouldReturnNullWhenGetCategoryByIdDoesNotHaveValue()
        {
            var categoryRepository = new CategoryRepository(_context);
            var categoryInDatabase = await categoryRepository.GetCategoryById(new ObjectId("65ecf78759159f2e38c2e514"));

            categoryInDatabase.Should().BeNull();
        }

        [Test]
        public async Task ShouldBeAbleToDeleteACategory()
        {
            var category = new Category("Tecnologia", "Eletrônicos diversos", "John");

            await _context.Categories.InsertOneAsync(category);

            var categoryRepository = new CategoryRepository(_context);
            await categoryRepository.Delete(category.Id);

            var mongoContext = GetService<MongoContext>();
            var categoryInDatabase = await mongoContext.Categories.Find(category => true).FirstOrDefaultAsync();

            categoryInDatabase.Should().BeNull();
        }
    }
}
