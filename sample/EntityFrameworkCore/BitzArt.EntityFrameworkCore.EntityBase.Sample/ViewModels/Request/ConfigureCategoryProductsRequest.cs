using System.Text.Json.Serialization;

namespace BitzArt.EntityFrameworkCore.EntityBase.Sample.ViewModels
{
    public class ConfigureCategoryProductsRequest
    {
        [JsonPropertyName("products")]
        public IEnumerable<Guid> ProductIds { get; set; }
    }
}
