using MongoDB.Bson;
using MongoDB.Driver;
using ProductCatalog.Entities;
using ProductCatalog.Infra;

namespace ProductCatalog
{
    public class CategoryRepository
    {
        private readonly MongoContext _context;

        public CategoryRepository(MongoContext context)
        {
            _context = context;
        }

        public async Task Create(Category category)
        {
           await _context.Categories.InsertOneAsync(category);
        }

        public async Task<IEnumerable<Category?>> GetAll()
        {
            return await _context.Categories.Find(c => true).ToListAsync();
        }

        public async Task<Category?> GetCategoryById(ObjectId id)
        {
            var filter = Builders<Category>
                .Filter
                .Eq(p => p.Id, id);

            return await _context.Categories.Find(filter).SingleOrDefaultAsync();
        }

        public async Task Delete(ObjectId id)
        {
            var filter = Builders<Category>
                .Filter
                .Eq(p => p.Id, id);

            await _context.Categories.DeleteOneAsync(filter);
        }
    }
}
