using MongoDB.Bson;

namespace ProductCatalog.Integration.Tests.Specs.Controllers
{
    public class ProductOutput
    {
        public string Id { get; private set; }

        public string Title { get; private set; } = null!;

        public string Description { get; private set; } = null!;

        public decimal Price { get; private set; }

        public string Category { get; private set; } = null!;

        public string Owner { get; private set; } = null!;
    }
}