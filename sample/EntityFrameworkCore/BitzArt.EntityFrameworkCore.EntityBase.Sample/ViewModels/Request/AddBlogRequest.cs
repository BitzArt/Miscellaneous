using BitzArt.EntityFrameworkCore.EntityBase.Sample.Models;
using System.Text.Json.Serialization;

namespace BitzArt.EntityFrameworkCore.EntityBase.Sample.ViewModels
{
    public class AddBlogRequest
    {
        [JsonPropertyName("name")]
        public string Name { get; set; }

        public Blog ToBlog() => new()
        {
            Name = Name
        };
    }
}
