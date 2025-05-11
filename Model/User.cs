namespace NguyenVinhSon_2122110315.Model
{
    public class User
    {
        public int Id { get; set; }

        public string FullName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;

        public string? Role { get; set; } = "customer"; // admin, staff, customer...
        public string? Avatar { get; set; }
        public string? Address { get; set; }


        public int? CreatedBy { get; set; }
        public int? UpdatedBy { get; set; }
        public int? DeletedBy { get; set; }

        public DateTime? CreatedAt { get; set; } = DateTime.Now;
        public DateTime?  UpdatedAt { get; set; } = DateTime.Now;
        public DateTime? DeletedAt { get; set; }

        // ✅ Đơn hàng người dùng đặt
        //public ICollection<Order> Orders { get; set; } = new List<Order>();

       

    }
}
