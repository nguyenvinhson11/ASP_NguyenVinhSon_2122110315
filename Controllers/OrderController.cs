using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NguyenVinhSon_2122110315.Data;
using NguyenVinhSon_2122110315.Model;
using NguyenVinhSon_2122110315.Request;

namespace NguyenVinhSon_2122110315.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize] // 🛡️ Yêu cầu phải có JWT hợp lệ mới truy cập được
    public class OrderController : ControllerBase
    {
        private readonly AppDbContext _context;

        public OrderController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/Order
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Order>>> Get()
        {
            return await _context.Orders
                //.Include(o => o.User)
                //.Include(o => o.OrderDetails)
                .ToListAsync();
        }

        // GET: api/Order/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Order>> Show(int id)
        {
            var order = await _context.Orders
                         .Include(o => o.User)
                         .Include(o => o.OrderDetails)
                         .ThenInclude(od => od.Product) // 👉 Lồng vào để lấy Product
                         .FirstOrDefaultAsync(o => o.Id == id);


            if (order == null)
            {
                return NotFound();
            }

            return order;
        }

        // POST: api/Order
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Create([FromBody] OrderCreateRequest request)
        {
            var userId = int.Parse(User.FindFirst("id")?.Value ?? "1");

            // Tạo đơn hàng
            var order = new Order
            {
                UserId = userId,
                Name = request.Name,
                Phone = request.Phone,
                Email = request.Email,
                Address = request.Address,
                Note = request.Note,
                TotalAmount = request.TotalAmount,
                PaymentMethod = request.PaymentMethod,

                CreatedBy = userId,
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now,
                DeletedAt = null
            };

            // Thêm order vào context
            _context.Orders.Add(order);
            await _context.SaveChangesAsync(); // Lưu trước để lấy order.Id

            // Tạo các chi tiết đơn hàng
            foreach (var item in request.OrderDetails)
            {
                var detail = new OrderDetail
                {
                    OrderId = order.Id,
                    ProductId = item.ProductId,
                    Quantity = item.Quantity,
                    Price = item.Price,
                    Note = item.Note,

                    CreatedBy = userId,
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now,
                    DeletedAt = null
                };

                _context.OrderDetails.Add(detail);
            }

            await _context.SaveChangesAsync();

            return Ok(new
            {
                Message = "Tạo đơn hàng thành công",
                Order = order
            });
        }




        // PUT: api/Order/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] OrderUpdateRequest request)
        {
            var order = await _context.Orders.FindAsync(id);
            var userId = int.Parse(User.FindFirst("id")?.Value ?? "1");

            if (order == null || order.DeletedAt != null)
                return NotFound();

            // Cập nhật thông tin đơn hàng nếu có
            if (!string.IsNullOrWhiteSpace(request.Name)) order.Name = request.Name;
            if (!string.IsNullOrWhiteSpace(request.Phone)) order.Phone = request.Phone;
            if (!string.IsNullOrWhiteSpace(request.Email)) order.Email = request.Email;
            if (!string.IsNullOrWhiteSpace(request.Address)) order.Address = request.Address;
            if (!string.IsNullOrWhiteSpace(request.Note)) order.Note = request.Note;
            if (!string.IsNullOrWhiteSpace(request.PaymentMethod)) order.PaymentMethod = request.PaymentMethod;
            if (request.TotalAmount.HasValue) order.TotalAmount = request.TotalAmount.Value;

            order.UpdatedAt = DateTime.Now;
            order.UpdatedBy = userId;
            order.DeletedAt = null;

           

            await _context.SaveChangesAsync();
            return Ok(order);
        }


        [HttpGet("user/{userId}")]
        public async Task<IActionResult> GetOrdersByUserId(int userId)
        {
            var orders = await _context.Orders
                .Where(o => o.UserId == userId && o.DeletedAt == null)
                .Include(o => o.OrderDetails) // nếu muốn lấy cả chi tiết
                .ToListAsync();

            return Ok(orders);
        }




        // DELETE: api/Order/5
        [HttpDelete("{id}")]
        [Authorize]
        public async Task<IActionResult> Delete(int id)
        {
            var order = await _context.Orders
                .Include(o => o.OrderDetails)
                .FirstOrDefaultAsync(o => o.Id == id);

            if (order == null || order.DeletedAt != null)
                return NotFound();

            var userId = int.Parse(User.FindFirst("id")?.Value ?? "1");
            var now = DateTime.Now;

            order.DeletedAt = now;
            order.DeletedBy = userId;

            // ❗ Xoá mềm tất cả OrderDetail liên quan
            foreach (var detail in order.OrderDetails)
            {
                detail.DeletedAt = now;
                detail.DeletedBy = userId;
            }

            await _context.SaveChangesAsync();
            return Ok("Đã xoá mềm đơn hàng và chi tiết liên quan.");
        }


        [HttpPut("restore/{id}")]
        [Authorize]
        public async Task<IActionResult> Restore(int id)
        {
            var order = await _context.Orders
                .Include(o => o.OrderDetails)
                .FirstOrDefaultAsync(o => o.Id == id);

            var userId = int.Parse(User.FindFirst("id")?.Value ?? "1");

            if (order == null || order.DeletedAt == null)
                return NotFound();

            order.DeletedAt = null;
            order.UpdatedAt = DateTime.Now;
            order.UpdatedBy = userId;

            foreach (var detail in order.OrderDetails)
            {
                detail.DeletedAt = null;
                detail.UpdatedAt = DateTime.Now;
                detail.UpdatedBy = userId;
            }

            await _context.SaveChangesAsync();
            return Ok("✅ Đã khôi phục đơn hàng và chi tiết liên quan.");
        }



        [HttpDelete("destroy/{id}")]
        [Authorize]
        public async Task<IActionResult> Destroy(int id)
        {
            var order = await _context.Orders
                .Include(o => o.OrderDetails)
                .FirstOrDefaultAsync(o => o.Id == id);

            if (order == null)
                return NotFound();

            // Xóa OrderDetail trước (nếu có)
            _context.OrderDetails.RemoveRange(order.OrderDetails);

            // Xóa Order
            _context.Orders.Remove(order);

            await _context.SaveChangesAsync();
            return Ok("🗑️ Đã xoá vĩnh viễn đơn hàng và chi tiết liên quan.");
        }

    }
}
