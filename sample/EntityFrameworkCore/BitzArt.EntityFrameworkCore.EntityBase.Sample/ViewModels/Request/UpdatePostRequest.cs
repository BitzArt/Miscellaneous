using BitzArt.EntityFrameworkCore.EntityBase.Sample.Models;
using System.Text.Json.Serialization;

namespace BitzArt.EntityFrameworkCore.EntityBase.Sample.ViewModels
{
    public class UpdatePostRequest
    {
        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("content")]
        public string Content { get; set; }

        public Post Apply(Post post, User updater)
        {
            post.Name = Name;
            post.Content = Content;

            post.Updated(updater);

            return post;
        }
    }
}
