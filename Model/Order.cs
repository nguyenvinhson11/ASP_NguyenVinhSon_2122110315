namespace NguyenVinhSon_2122110315.Model
{
    public class Order
    {
        public int Id { get; set; }

        // Người đặt hàng
        public int UserId { get; set; }
        public User? User { get; set; }

        // Thông tin người nhận
        public string Name { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;

        public string? Note { get; set; }

        // Tổng tiền, phương thức thanh toán
        public decimal TotalAmount { get; set; }
        public string PaymentMethod { get; set; } = "cash"; // cash, vnpay, momo...

        public int? CreatedBy { get; set; }
        public User? CreatedByUser { get; set; }

        public int? UpdatedBy { get; set; }
        public User? UpdatedByUser { get; set; }

        public int? DeletedBy { get; set; }
        public User? DeletedByUser { get; set; }

        public DateTime? CreatedAt { get; set; } = DateTime.Now;
        public DateTime? UpdatedAt { get; set; } = DateTime.Now;
        public DateTime? DeletedAt { get; set; }

        // Quan hệ với chi tiết đơn hàng
        public ICollection<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();
    }
}
