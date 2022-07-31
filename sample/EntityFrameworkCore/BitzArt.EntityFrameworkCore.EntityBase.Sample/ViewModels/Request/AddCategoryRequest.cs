using BitzArt.EntityFrameworkCore.EntityBase.Sample.Models;
using System.Text.Json.Serialization;

namespace BitzArt.EntityFrameworkCore.EntityBase.Sample.ViewModels
{
    public class AddCategoryRequest
    {
        [JsonPropertyName("name")]
        public string Name { get; set; }

        public Category ToCategory() => new()
        {
            Name = Name
        };
    }
}
