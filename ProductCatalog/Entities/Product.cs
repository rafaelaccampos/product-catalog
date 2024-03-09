using MongoDB.Bson;

namespace ProductCatalog.Entities
{
    public class Product
    {
        public ObjectId Id { get; set; }

        public string Title { get; set; } = null!;

        public string Description { get; set; } = null!;

        public decimal Price { get; set; }

        public string Category { get; set; } = null!;

        public string Owner { get; set; } = null!;
    }
}
