﻿using MongoDB.Bson;
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

        public async Task<Product?> Update(ObjectId id, string category)
        {
            var filter = Builders<Product>
                .Filter.Eq(p => p.Id, id);

            var updatedCategory = Builders<Product>
                .Update
                .Set(p => p.Category, category);

          return await _context.Products.FindOneAndUpdateAsync(filter, updatedCategory);
        }

        public async Task<Product?> Delete(ObjectId id)
        {
            var filter = Builders<Product>
                .Filter
                .Eq(p => p.Id, id);

            return await _context.Products.FindOneAndDeleteAsync(filter);
        }
    }
}
