namespace NguyenVinhSon_2122110315.Request
{
    public class OrderCreateRequest
    {
        public string Name { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public string? Note { get; set; }
        public decimal TotalAmount { get; set; }
        public string PaymentMethod { get; set; } = "cash";

        // Danh sách chi tiết đơn hàng gửi kèm
        public List<OrderDetailCreateRequest> OrderDetails { get; set; } = new();
    }

    public class OrderDetailCreateRequest
    {
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public string? Note { get; set; }
    }
}
