using MongoDB.Driver;
using ProductCatalog.Entities;
using ProductCatalog.Infra;

namespace ProductCatalog.Infra.Repositories
{
    public class ProductRepository
    {
        private readonly MongoContext _context;

        public ProductRepository(MongoContext context)
        {
            _context = context;
        }

        public async Task CreateProduct(Product product)
        {
            await _context.Products.InsertOneAsync(product);
        }

        public async Task<IEnumerable<Product>> GetAllProducts()
        {
            return await _context.Products.Find(p => true).ToListAsync();
        }
    }
}
