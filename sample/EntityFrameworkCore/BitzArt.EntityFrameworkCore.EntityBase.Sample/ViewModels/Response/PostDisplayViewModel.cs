using BitzArt.EntityFrameworkCore.EntityBase.Sample.Models;
using System.Text.Json.Serialization;

namespace BitzArt.EntityFrameworkCore.EntityBase.Sample.ViewModels
{
    public class PostDisplayViewModel
    {
        [JsonPropertyName("id")]
        public object Id { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("price")]
        public decimal Price { get; set; }

        [JsonPropertyName("createdBy")]
        public object CreatedBy { get; set; }

        [JsonIgnore]
        public DateTime CreatedOn { get; set; }

        [JsonPropertyName("created")]
        public string CreatedOnString => CreatedOn.ToString("HH:mm:ss");

        [JsonPropertyName("lastUpdatedBy")]
        public object LastUpdatedBy { get; set; }

        [JsonIgnore]
        public DateTime LastUpdatedOn { get; set; }

        [JsonPropertyName("updated")]
        public string UpdatedOnString => LastUpdatedOn.ToString("HH:mm:ss");

        public PostDisplayViewModel(Post post)
        {
            Id = post.Id.Value;
            Name = post.Name;
            Price = post.Price;

            CreatedBy = post.CreationInfo.CreatorId!.Value;
            CreatedOn = post.CreationInfo.CreatedOn;

            LastUpdatedBy = post.UpdateInfo.UpdaterId!.Value;
            LastUpdatedOn = post.UpdateInfo.UpdatedOn;
        }
    }
}
