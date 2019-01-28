using MySql.Data.EntityFrameworkCore.DataAnnotations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
namespace MySite.Models
{
    [MySqlCharset("utf8")]
    public class GropsAndProducts
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public int Id { get; set; }
        [ForeignKey("Products")]
        public int ProductId { get; set; }
        [ForeignKey("Groups")]
        public int GroupId { get; set; }

        public Groups Groups { get; set; }
        public Products Products { get; set; }
    }

    [MySqlCharset("utf8")]
    public class Groups
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public int Id { get; set; }
        [Required]
        [MySqlCharset("utf-8")]
        public string Title { get; set; }
        [Required]
        [MySqlCharset("utf-8")]
        public string Discription { get; set; }
        public List<GropsAndProducts> groupAndProducts { get; set; }
        [ForeignKey("Category")]
        public int CategoryId { get; set; }

        public Category Category { get; set; }
    }

    [MySqlCharset("utf8")]
    public class Products
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public int Id { get; set; }
        [Required]
        [MySqlCharset("utf-8")]
        public string Title { get; set; }
        // Еденица измерения
        [Required]
        [MySqlCharset("utf-8")]
        public string Unit { get; set; }
        [Required]
        public int Count { get; set; }
        [Required]
        public int Price { get; set; }
        [Required]
        public int Discount { get; set; }
        [MySqlCharset("utf-8")]
        public string Discription { get; set; }

        public List<GropsAndProducts> groupAndProducts { get; set; }
        public List<Images> Images { get; set; }
    }

    [MySqlCharset("utf-8")]
    public class Category
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public int Id { get; set; }
        [Required]
        [MySqlCharset("utf-8")]
        public string Title { get; set; }
        [MySqlCharset("utf-8")]
        public string Discription { get; set; }

        public List<Groups> Groups { get; set; }
    }

    [MySqlCharset("utf-8")]
    public class Images
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public int Id { get; set; }
        [MySqlCharset("utf-8")]
        public string ImageType { get; set; }
        public int ImageSize { get; set; }
        [MySqlCharset("utf-8")]
        public string ImageName { get; set; }
        [ForeignKey("Products")]
        public int ProductId { get; set; }

        public Products Products { get; set; }
    }
}
