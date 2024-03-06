namespace ProductCatalog
{
    public class CategoryItem
    {
        public string CategoryTitle { get; set; }

        public string CategoryDescription { get; set; }

        public IList<ProductItem> Items { get; set; }
    }
}
