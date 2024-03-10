using MongoDB.Bson;

namespace ProductCatalog.Entities
{
    public class Product
    {
        public Product(
            string title, 
            string description, 
            decimal price, 
            string category,
            string owner)
        { 
            Title = title; 
            Description = description; 
            Price = price; 
            Category = category; 
            Owner = owner;
        }

        public ObjectId Id { get; private set; }

        public string Title { get; private set; } = null!;

        public string Description { get; private set; } = null!;

        public decimal Price { get; private set; }

        public string Category { get; private set; } = null!;

        public string Owner { get; private set; } = null!;
    }
}
