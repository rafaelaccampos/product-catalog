using MongoDB.Driver;
using ProductCatalog.Entities;

namespace ProductCatalog.Infra.Repositories
{
    public class CatalogRepository
    {
        private readonly MongoContext _context;

        public CatalogRepository(MongoContext context)
        {
            _context = context;
        }
        public async Task CreateCatalog(Catalog catalog)
        {
            await _context.Catalogs.InsertOneAsync(catalog);
        }

        public async Task<IEnumerable<Catalog>> GetAllCatalogs()
        {
            return await _context.Catalogs.Find(c => true).ToListAsync();
        }
    }
}
