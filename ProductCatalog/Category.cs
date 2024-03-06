using MongoDB.Bson;

namespace ProductCatalog
{
    public class Category
    {
        public ObjectId Id { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public string Owner { get; set; }
    }
}
