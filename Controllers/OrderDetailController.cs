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
    public class OrderDetailController : ControllerBase
    {
        private readonly AppDbContext _context;

        public OrderDetailController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/OrderDetail
        [HttpGet]
        public async Task<ActionResult<IEnumerable<OrderDetail>>> Get()
        {
            return await _context.OrderDetails
                //.Include(od => od.Order)
                .Include(od => od.Product)
                .ToListAsync();
        }

        // GET: api/OrderDetail/5
        [HttpGet("{id}")]
        public async Task<ActionResult<OrderDetail>> Show(int id)
        {
            var detail = await _context.OrderDetails
                //.Include(od => od.Order)
                .Include(od => od.Product)
                .FirstOrDefaultAsync(od => od.Id == id);

            if (detail == null)
            {
                return NotFound();
            }

            return detail;
        }

        // POST: api/OrderDetail
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Create([FromBody] OrderDetail model)
        {
            // Lấy user ID từ token
            var userId = int.Parse(User.FindFirst("id")?.Value ?? "1");

            // Kiểm tra sơ bộ
            if (model.OrderId <= 0 || model.ProductId <= 0 || model.Quantity <= 0 || model.Price <= 0)
                return BadRequest("Dữ liệu không hợp lệ");

            model.CreatedBy = userId;
            model.CreatedAt = DateTime.Now;
            model.UpdatedAt = DateTime.Now;
            model.DeletedAt = null;

            _context.OrderDetails.Add(model);
            await _context.SaveChangesAsync();

            return Ok(model);
        }


        //// PUT: api/OrderDetail/5
        [HttpPut("{id}")]
        [Authorize]
        public async Task<IActionResult> Update(int id, [FromBody] OrderDetailUpdateRequest request)
        {
            var detail = await _context.OrderDetails.FindAsync(id);
            if (detail == null || detail.DeletedAt != null)
                return NotFound("Không tìm thấy chi tiết đơn hàng.");

            var userId = int.Parse(User.FindFirst("id")?.Value ?? "1");

            // Chỉ cập nhật nếu có truyền lên
            if (request.Quantity.HasValue && request.Quantity > 0)
                detail.Quantity = request.Quantity.Value;

            if (request.Price.HasValue && request.Price > 0)
                detail.Price = request.Price.Value;

            if (!string.IsNullOrWhiteSpace(request.Note))
                detail.Note = request.Note;

            detail.UpdatedBy = userId;
            detail.UpdatedAt = DateTime.Now;
            detail.DeletedAt = null;

            await _context.SaveChangesAsync();

            return Ok(detail);
        }

        [HttpDelete("{id}")]
        [Authorize]
        public async Task<IActionResult> Delete(int id)
        {
            var orderDetail = await _context.OrderDetails.FindAsync(id);
            var userId = int.Parse(User.FindFirst("id")?.Value ?? "1");
            if (orderDetail == null) return NotFound();

            orderDetail.DeletedAt = DateTime.Now;
            orderDetail.DeletedBy = userId;
            await _context.SaveChangesAsync();

            return Ok("Đã xóa mềm");
        }



        [HttpPut("restore/{id}")]
        [Authorize]
        public async Task<IActionResult> Restore(int id)
        {
            var orderDetail = await _context.OrderDetails.FindAsync(id);
            var userId = int.Parse(User.FindFirst("id")?.Value ?? "1");
            if (orderDetail == null || orderDetail.DeletedAt == null)
                return NotFound();

            orderDetail.DeletedAt = null;
            orderDetail.UpdatedAt = DateTime.Now;
            orderDetail.UpdatedBy = userId;

            await _context.SaveChangesAsync();
            return Ok("Đã khôi phục");
        }


        [HttpDelete("destroy/{id}")]
        public async Task<IActionResult> Destroy(int id)
        {
            var orderDetail = await _context.OrderDetails.FindAsync(id);

            if (orderDetail == null)
                return NotFound();

            _context.OrderDetails.Remove(orderDetail);
            await _context.SaveChangesAsync();

            return Ok("Đã xóa vĩnh viễn");
        }



    }
}
