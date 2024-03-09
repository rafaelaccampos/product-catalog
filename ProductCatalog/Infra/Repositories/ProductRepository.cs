using MongoDB.Bson;
using MongoDB.Driver;
using ProductCatalog.Entities;

namespace ProductCatalog.Infra.Repositories
{
    public class ProductRepository
    {
        private readonly MongoContext _context;

        public ProductRepository(MongoContext context)
        {
            _context = context;
        }

        public async Task Create(Product product)
        {
            await _context.Products.InsertOneAsync(product);
        }

        public async Task<IEnumerable<Product?>> GetAll()
        {
            return await _context.Products.Find(p => true).ToListAsync();
        }

        public async Task<Product?> GetProductById(ObjectId id)
        {
            var filter = Builders<Product>
                .Filter
                .Eq(p => p.Id, id);

            return await _context.Products.Find(filter).SingleOrDefaultAsync();
        }

        public async Task Update(ObjectId id, string category)
        {
            var filter = Builders<Product>
                .Filter.Eq(p => p.Id, id);

            var updatedCategory = Builders<Product>
                .Update
                .Set(p => p.Category, category);

            await _context.Products.UpdateOneAsync(filter, updatedCategory);
        }

        public async Task Delete(ObjectId id)
        {
            var filter = Builders<Product>
                .Filter
                .Eq(p => p.Id, id);

            await _context.Products.DeleteOneAsync(filter);
        }
    }
}
