using MongoDB.Bson;

namespace ProductCatalog.Dtos
{
    public class CategoryInput
    {
        public string Title { get; set; } = null!;

        public string Description { get; set; } = null!;

        public string Owner { get; set; } = null!;
    }
}
