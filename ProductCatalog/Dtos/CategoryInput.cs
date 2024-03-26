using MongoDB.Bson;

namespace ProductCatalog.Dtos
{
    public class CategoryInput
    {
        public string Id { get; private set; } = null!;

        public string Title { get; private set; } = null!;

        public string Description { get; private set; } = null!;

        public string Owner { get; private set; } = null!;
    }
}
