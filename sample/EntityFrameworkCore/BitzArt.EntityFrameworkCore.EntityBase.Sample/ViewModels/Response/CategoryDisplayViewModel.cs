using BitzArt.EntityFrameworkCore.EntityBase.Sample.Models;
using System.Text.Json.Serialization;

namespace BitzArt.EntityFrameworkCore.EntityBase.Sample.ViewModels;

public class CategoryDisplayViewModel
{
    [JsonPropertyName("id")]
    public object Id { get; set; }

    [JsonPropertyName("name")]
    public string Name { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public IEnumerable<ProductDisplayViewModel>? Products { get; set; }

    public CategoryDisplayViewModel(Category category)
    {
        Id = category.Id!.Value;
        Name = category.Name;

        Products = category.Products is null || !category.Products.Any() ?
            null : category.Products.Select(x => new ProductDisplayViewModel(x));
    }
}
