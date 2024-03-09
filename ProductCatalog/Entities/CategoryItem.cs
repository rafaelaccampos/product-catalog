namespace ProductCatalog.Entities
{
    public class CategoryItem
    {
        public string CategoryTitle { get; set; } = null!;

        public string CategoryDescription { get; set; } = null!;

        public IList<ProductItem> Items { get; set; } = null!;
    }
}
