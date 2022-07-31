using BitzArt.EntityFrameworkCore.EntityBase.Sample.Models;
using System.Text.Json.Serialization;

namespace BitzArt.EntityFrameworkCore.EntityBase.Sample.ViewModels
{
    public class AddUserRequest
    {
        [JsonPropertyName("name")]
        public string Name { get; set; }

        public User ToUser() => new User
        {
            Id = null,
            Name = Name
        };
    }
}
