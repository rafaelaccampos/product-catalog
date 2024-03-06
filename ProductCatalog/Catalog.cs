using MongoDB.Bson;

namespace ProductCatalog
{
    public class Catalog
    {
        public ObjectId Id { get; set; }

        public string Owner { get; set; }

        public IList<CategoryItem> CatalogItems { get; set; }
    }
}
