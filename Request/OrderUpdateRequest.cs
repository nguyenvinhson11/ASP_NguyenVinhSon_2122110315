namespace NguyenVinhSon_2122110315.Request
{
    public class OrderUpdateRequest
    {
        public string? Name { get; set; }
        public string? Phone { get; set; }
        public string? Email { get; set; }
        public string? Address { get; set; }
        public string? Note { get; set; }
        public decimal? TotalAmount { get; set; }
        public string? PaymentMethod { get; set; }

        // Danh sách chi tiết đơn hàng (nếu có cập nhật thêm)
        public List<OrderDetailUpdateRequest> OrderDetails { get; set; } = new();
    }

    public class OrderDetailUpdateRequest
    {
        public int Id { get; set; } // ⚠️ Bắt buộc có ID để biết sửa cái nào
        public int? Quantity { get; set; }
        public decimal? Price { get; set; }
        public string? Note { get; set; }
    }

}
