using MongoDB.Driver;
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

        public async Task CreateCategory(Category category)
        {
            await _context.Categories.InsertOneAsync(category);
        }

        public async Task<IEnumerable<Category>> GetAllCategories()
        {
            return await _context.Categories.Find(c => true).ToListAsync();
        }
    }
}
