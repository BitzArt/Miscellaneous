using BitzArt.EntityFrameworkCore.EntityBase.Sample.Models;
using System.Text.Json.Serialization;

namespace BitzArt.EntityFrameworkCore.EntityBase.Sample.ViewModels
{
    public class UpdateProductRequest
    {
        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("price")]
        public decimal Price { get; set; }

        public Product Apply(Product product, User updater)
        {
            product.Name = Name;
            product.Price = Price;

            product.Updated(updater);

            return product;
        }
    }
}
