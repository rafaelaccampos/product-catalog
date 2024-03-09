namespace ProductCatalog.Entities
{
    public class ProductItem
    {
        public string Title { get; set; } = null!;

        public string Description { get; set; } = null!;

        public decimal Price { get; set; }
    }
}
