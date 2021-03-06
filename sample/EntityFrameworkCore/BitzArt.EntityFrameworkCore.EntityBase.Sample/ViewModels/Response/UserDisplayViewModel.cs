using BitzArt.EntityFrameworkCore.EntityBase.Sample.Models;
using System.Text.Json.Serialization;

namespace BitzArt.EntityFrameworkCore.EntityBase.Sample.ViewModels
{
    public class UserDisplayViewModel
    {
        [JsonPropertyName("id")]
        public object Id { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonIgnore]
        public DateTime CreatedOn { get; set; }

        [JsonPropertyName("created")]
        public string CreatedOnString => CreatedOn.ToString("HH:mm:ss");

        public UserDisplayViewModel(User user)
        {
            Id = user.Id.Value;
            Name = user.Name;
            CreatedOn = user.CreationInfo.CreatedOn;
        }
    }
}
