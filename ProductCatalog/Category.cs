using MongoDB.Bson;

namespace ProductCatalog
{
    public class Category
    {
        public ObjectId Id { get; set; }

        public string Title { get; set; } = null!;

        public string Description { get; set; } = null!;

        public string Owner { get; set; } = null!;
    }
}
