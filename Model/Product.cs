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

        public int BrandId { get; set; }
        public Brand? Brand { get; set; }



        public int? CreatedBy { get; set; }
        public User? CreatedByUser { get; set; }

        public int? UpdatedBy { get; set; }
        public User? UpdatedByUser { get; set; }

        public int? DeletedBy { get; set; }
        public User? DeletedByUser { get; set; }


        public DateTime? CreatedAt { get; set; } = DateTime.Now;
        public DateTime? UpdatedAt { get; set; } = DateTime.Now;
        public DateTime? DeletedAt { get; set; }
    }

}
