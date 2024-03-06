using MongoDB.Driver;

namespace ProductCatalog.Infra
{
    public class MongoDbContext
    {
        private readonly IMongoCollection<Product> _products;
        private readonly IMongoCollection<Category> _categories;
        private readonly IMongoCollection<Catalog> _catalogItems;

        public MongoDbContext(string connectionString, string databaseName)
        {
            var client = new MongoClient(connectionString);
            var database = client.GetDatabase(databaseName);

            _products = database.GetCollection<Product>("products");
            _categories = database.GetCollection<Category>("categories");
            _catalogItems = database.GetCollection<Catalog>("catalogs");
        }

        public async Task CreateProduct(Product product)
        {
            await _products.InsertOneAsync(product);
        }

        public IEnumerable<Product> GetAllProducts()
        {
            return _products.Find(p => true).ToList();
        }

        public async Task CreateCategory(Category category)
        {
            await _categories.InsertOneAsync(category);
        }

        public IEnumerable<Category> GetAllCategories()
        {
            return _categories.Find(c => true).ToList();
        }

        public async Task CreateCatalog(Catalog catalog)
        {
            await _catalogItems.InsertOneAsync(catalog);
        }

        public IList<Catalog> GetAllCatalogs()
        {
            return _catalogItems.Find(c => true).ToList();
        }
    }
}
