using System.Text.Json.Serialization;

namespace BitzArt.EntityFrameworkCore.EntityBase.Sample.ViewModels
{
    public class ConfigureBlogPostsRequest
    {
        [JsonPropertyName("posts")]
        public IEnumerable<Guid> PostIds { get; set; }
    }
}
