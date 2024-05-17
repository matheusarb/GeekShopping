using GeekShopping.ProductAPI.Data.ValueObjects;
using GeekShopping.ProductAPI.Models.Base;
using Microsoft.IdentityModel.Tokens;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace GeekShopping.ProductAPI.Models
{
    [Table("product")]
    public class Product : BaseEntity
    {
        [Column("name")]
        [Required]
        [StringLength(150)]
        public string? Name { get; set; }

        [Column("price")]
        [Required]
        [Range(1, 10000)]
        public decimal Price { get; set; }

        [Column("description")]
        [StringLength(500)]
        public string? Description { get; set; }

        [Column("category_name")]
        [StringLength(50)]
        public string? CategoryName { get; set; }

        [Column("img_url")]
        [StringLength(300)]
        public string? ImgURL { get; set; }

        public void Update(ProductVO entity)
        {
            Name = entity.Name.IsNullOrEmpty() ? Name : entity.Name;
            Price = entity.Price <= 0 ? Price : entity.Price;
            Description = entity.Description.IsNullOrEmpty() ? Description : entity.Description;
            CategoryName = entity.CategoryName.IsNullOrEmpty() ? CategoryName : entity.CategoryName;
            ImgURL = entity.ImgURL.IsNullOrEmpty() ? ImgURL : entity.ImgURL;
        }
    }
}
