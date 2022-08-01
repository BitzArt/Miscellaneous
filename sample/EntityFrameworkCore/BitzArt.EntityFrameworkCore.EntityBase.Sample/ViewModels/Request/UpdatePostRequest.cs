using BitzArt.EntityFrameworkCore.EntityBase.Sample.Models;
using System.Text.Json.Serialization;

namespace BitzArt.EntityFrameworkCore.EntityBase.Sample.ViewModels
{
    public class UpdatePostRequest
    {
        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("price")]
        public decimal Price { get; set; }

        public Post Apply(Post post, User updater)
        {
            post.Name = Name;
            post.Price = Price;

            post.Updated(updater);

            return post;
        }
    }
}
