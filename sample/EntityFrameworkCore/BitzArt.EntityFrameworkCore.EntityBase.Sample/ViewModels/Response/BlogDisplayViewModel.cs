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

    public BlogDisplayViewModel(Blog blog)
    {
        Id = blog.Id.Value;
        Name = blog.Name;

        Posts = blog.Posts is null || !blog.Posts.Any() ?
            null : blog.Posts.Select(x => new PostDisplayViewModel(x));
    }
}
