using System.ComponentModel.DataAnnotations;

namespace NguyenVinhSon_2122110315.Request
{
    public class ProductStoreRequest
    {
        [Required(ErrorMessage = "Tên sản phẩm không được để trống")]
        [MaxLength(200, ErrorMessage = "Tên không quá 200 ký tự")]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "Giá gốc không được để trống")]
        public decimal Price { get; set; }

        public decimal? PriceSale { get; set; }

        public string? Description { get; set; }

        [Required(ErrorMessage = "Chọn danh mục")]
        public int CategoryId { get; set; }

        [Required(ErrorMessage = "Chọn thương hiệu")]
        public int BrandId { get; set; }

        // file upload thumbnail
        [Display(Name = "Ảnh thumbnail")]
        public IFormFile? ThumbnailFile { get; set; }
    }
}
