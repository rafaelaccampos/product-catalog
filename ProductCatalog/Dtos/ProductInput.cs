namespace ProductCatalog.Dtos
{
    public class ProductInput
    {
        public string Title { get; set; } = null!;

        public string Description { get; set; } = null!;

        public decimal Price { get; set; }

        public string Category { get; set; } = null!;

        public string Owner { get; set; } = null!;
    }
}
