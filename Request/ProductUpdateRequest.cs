
using System.ComponentModel.DataAnnotations;


namespace NguyenVinhSon_2122110315.Request

{
    public class ProductUpdateRequest
    {
        public string? Name { get; set; }

        public decimal? Price { get; set; }
        public decimal? PriceSale { get; set; }

        public string? Description { get; set; }

        public int? CategoryId { get; set; }
        public int? BrandId { get; set; }

        // thêm field để upload ảnh mới
        [Display(Name = "Ảnh thumbnail")]
        public IFormFile? ThumbnailFile { get; set; }


    }
}
