using System.ComponentModel.DataAnnotations;

namespace NguyenVinhSon_2122110315.Model
{
    public class Product
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(200)]
        public string Name { get; set; } = string.Empty;

        public decimal Price { get; set; }
        public decimal? PriceSale { get; set; }

        public string? Description { get; set; }
        public string? Thumbnail { get; set; }

       
        public int CategoryId { get; set; }
        public Category? Category { get; set; } 


        public string? CreatedBy { get; set; }
        public string? UpdatedBy { get; set; }
        public string? DeletedBy { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime UpdatedAt { get; set; } = DateTime.Now;
        public DateTime? DeletedAt { get; set; }
    }

}
