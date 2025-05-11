namespace NguyenVinhSon_2122110315.Model
{
    public class Category
    {
        public int Id { get; set; }

        public string Name { get; set; } = string.Empty;

        public int? CreatedBy { get; set; }
        public User? CreatedByUser { get; set; }

        public int? UpdatedBy { get; set; }
        public User? UpdatedByUser { get; set; }

        public int? DeletedBy { get; set; }
        public User? DeletedByUser { get; set; }

        public DateTime? CreatedAt { get; set; } = DateTime.Now;
        public DateTime? UpdatedAt { get; set; } = DateTime.Now;
        public DateTime? DeletedAt { get; set; }

        public ICollection<Product> Products { get; set; } = new List<Product>();
    }
}
