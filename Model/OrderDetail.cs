namespace NguyenVinhSon_2122110315.Model
{
    public class OrderDetail
    {
        public int Id { get; set; }

        // Quan hệ với đơn hàng
        public int OrderId { get; set; }
        public Order? Order { get; set; }

        // Quan hệ với sản phẩm
        public int ProductId { get; set; }
        public Product? Product { get; set; }

        public int Quantity { get; set; }
        public decimal Price { get; set; }         // Giá tại thời điểm mua
        public decimal Total => Quantity * Price;  // Tổng tiền

        public string? Note { get; set; }

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
