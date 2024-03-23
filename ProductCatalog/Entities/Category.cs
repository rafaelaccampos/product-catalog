using MongoDB.Bson;
using System.Text.Json.Serialization;

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

        [JsonIgnore]
        public ObjectId Id { get; private set; }

        public string Title { get; private set; } = null!;

        public string Description { get; private set; } = null!;

        public string Owner { get; private set; } = null!;
    }
}
