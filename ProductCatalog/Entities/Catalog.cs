using MongoDB.Bson;

namespace ProductCatalog.Entities
{
    public class Catalog
    {
        public ObjectId Id { get; set; }

        public string Owner { get; set; } = null!;

        public IList<CategoryItem> CatalogItems { get; set; } = null!;
    }
}
