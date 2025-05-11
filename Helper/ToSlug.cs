using System.Text;
using System.Text.RegularExpressions;

namespace NguyenVinhSon_2122110315.Helper
{
    public static class SlugHelper
    {
        public static string ToSlug(string input)
        {
            // Bỏ dấu tiếng Việt
            string normalized = input.Normalize(NormalizationForm.FormD);
            var bytes = Encoding.UTF8.GetBytes(normalized);
            string slug = Encoding.UTF8.GetString(bytes);

            // Loại bỏ các ký tự không phải chữ cái, số
            slug = Regex.Replace(slug, @"[^a-zA-Z0-9\s-]", "");

            // Chuyển thành lowercase, thay khoảng trắng = dấu gạch
            slug = Regex.Replace(slug, @"\s+", "-").ToLower();

            return slug;
        }
    }
}
