using MongoDB.Bson;

namespace ProductCatalog.Entities
{
    public class Category
    {
        public Category(string title, string description, string owner)
        {
            Title = title;
            Description = description;
            Owner = owner;
        }

        public ObjectId Id { get; private set; }

        public string Title { get; private set; } = null!;

        public string Description { get; private set; } = null!;

        public string Owner { get; private set; } = null!;
    }
}
