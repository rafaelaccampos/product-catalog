using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace ProductCatalog.Infra
{
    public class MongoContext
    {
        private readonly MongoClient _client;
        private readonly IMongoDatabase _database; 

        public MongoContext(IOptions<CatalogSettings> catalogSettings)
        {
            _client = new MongoClient(catalogSettings.Value.ConnectionString);
            _database = _client.GetDatabase(catalogSettings.Value.DatabaseName);
        }

        public IMongoCollection<Product> Products => _database.GetCollection<Product>("products");

        public IMongoCollection<Category> Categories => _database.GetCollection<Category>("categories");

        public IMongoCollection<Catalog> Catalogs => _database.GetCollection<Catalog>("catalogs");

        public async Task CreateCategory(Category category)
        {
            await Categories.InsertOneAsync(category);
        }

        public async Task<IEnumerable<Category>> GetAllCategories()
        {
            return await Categories.Find(c => true).ToListAsync();
        }

        public async Task CreateCatalog(Catalog catalog)
        {
            await Catalogs.InsertOneAsync(catalog);
        }

        public async Task<IEnumerable<Catalog>> GetAllCatalogs()
        {
            return await Catalogs.Find(c => true).ToListAsync();
        }
    }
}
