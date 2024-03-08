using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace ProductCatalog.Infra
{
    public class MongoDbContext
    {
        public readonly IMongoCollection<Product> _products;
        private readonly IMongoCollection<Category> _categories;
        private readonly IMongoCollection<Catalog> _catalogItems;

        public MongoDbContext(IOptions<CatalogSettings> catalogSettings)
        {
            var client = new MongoClient(catalogSettings.Value.ConnectionString);
            var database = client.GetDatabase(catalogSettings.Value.Database);

            _products = database.GetCollection<Product>("products");
            _categories = database.GetCollection<Category>("categories");
            _catalogItems = database.GetCollection<Catalog>("catalogs");
        }

        public async Task CreateProduct(Product product)
        {
            await _products.InsertOneAsync(product);
        }

        public async Task<IEnumerable<Product>> GetAllProducts()
        {
            return await _products.Find(p => true).ToListAsync();
        }

        public async Task CreateCategory(Category category)
        {
            await _categories.InsertOneAsync(category);
        }

        public async Task<IEnumerable<Category>> GetAllCategories()
        {
            return await _categories.Find(c => true).ToListAsync();
        }

        public async Task CreateCatalog(Catalog catalog)
        {
            await _catalogItems.InsertOneAsync(catalog);
        }

        public async Task<IEnumerable<Catalog>> GetAllCatalogs()
        {
            return await _catalogItems.Find(c => true).ToListAsync();
        }
    }
}
