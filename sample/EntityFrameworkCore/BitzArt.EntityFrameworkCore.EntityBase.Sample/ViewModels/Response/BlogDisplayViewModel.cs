using BitzArt.EntityFrameworkCore.EntityBase.Sample.Models;
using System.Text.Json.Serialization;

namespace BitzArt.EntityFrameworkCore.EntityBase.Sample.ViewModels;

public class BlogDisplayViewModel
{
    [JsonPropertyName("id")]
    public object Id { get; set; }

    [JsonPropertyName("name")]
    public string Name { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public IEnumerable<PostDisplayViewModel> Posts { get; set; }

    public BlogDisplayViewModel(Blog category)
    {
        Id = category.Id.Value;
        Name = category.Name;

        Posts = category.Posts is null || !category.Posts.Any() ?
            null : category.Posts.Select(x => new PostDisplayViewModel(x));
    }
}
