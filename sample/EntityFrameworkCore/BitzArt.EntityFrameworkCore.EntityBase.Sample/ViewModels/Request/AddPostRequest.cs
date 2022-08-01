using BitzArt.EntityFrameworkCore.EntityBase.Sample.Models;
using System.Text.Json.Serialization;

namespace BitzArt.EntityFrameworkCore.EntityBase.Sample.ViewModels
{
    public class AddPostRequest
    {
        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("price")]
        public decimal Price { get; set; }

        public Post ToPost(User creator) => new Post(creator)
        {
            Name = Name,
            Price = Price,
        };
    }
}
