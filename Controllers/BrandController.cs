using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NguyenVinhSon_2122110315.Data;
using NguyenVinhSon_2122110315.Model;
using Microsoft.EntityFrameworkCore;
using NguyenVinhSon_2122110315.Request;
using Microsoft.AspNetCore.Authorization;


namespace NguyenVinhSon_2122110315.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BrandController : ControllerBase
    {
        private readonly AppDbContext _context;

        public BrandController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Get()
        {
            var data = await _context.Brands
                .Include(b => b.CreatedByUser)
                .Include(b => b.UpdatedByUser)
                .Include(b => b.DeletedByUser)
                .Where(b => b.DeletedAt == null)
                .ToListAsync();
            return Ok(data);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Show(int id)
        {
            var brand = await _context.Brands.FindAsync(id);
            if (brand == null) return NotFound();
            return Ok(brand);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Create([FromBody] BrandStoreRequest request)
        {
            var userId = int.Parse(User.FindFirst("id")?.Value ?? "1");

            var brand = new Brand
            {
                Name = request.Name,
                CreatedBy = userId,
                CreatedAt = DateTime.Now,
                UpdatedAt = null,
                DeletedAt = null,
                DeletedBy = null,
                UpdatedBy = null
            };

            _context.Brands.Add(brand);
            await _context.SaveChangesAsync();

            return Ok(brand);
        }



        [HttpPut("{id}")]
        [Authorize]
        public async Task<IActionResult> Update(int id, [FromBody] BrandUpdateRequest request)
        {
            var brand = await _context.Brands.FindAsync(id);
            var userId = int.Parse(User.FindFirst("id")?.Value ?? "1");
            if (brand == null) return NotFound();

            if (!string.IsNullOrWhiteSpace(request.Name))
                brand.Name = request.Name;

           
            brand.UpdatedBy = userId;
            brand.UpdatedAt = DateTime.Now;
            brand.DeletedAt = null;

            await _context.SaveChangesAsync();
            return Ok(brand); // Trả lại brand sau khi cập nhật
        }


        [HttpDelete("{id}")]
        [Authorize]
        public async Task<IActionResult> Delete(int id)
        {
            var brand = await _context.Brands.FindAsync(id);
            var userId = int.Parse(User.FindFirst("id")?.Value ?? "1");
            if (brand == null) return NotFound();

            brand.DeletedAt = DateTime.Now;
            brand.DeletedBy = userId;
            await _context.SaveChangesAsync();

            return Ok("Đã xóa mềm");
        }


       
        [HttpPut("restore/{id}")]
        [Authorize]
        public async Task<IActionResult> Restore(int id)
        {
            var brand = await _context.Brands.FindAsync(id);
            var userId = int.Parse(User.FindFirst("id")?.Value ?? "1");
            if (brand == null || brand.DeletedAt == null)
                return NotFound();

            brand.DeletedAt = null;
            brand.UpdatedAt = DateTime.Now;
            brand.UpdatedBy = userId;

            await _context.SaveChangesAsync();
            return Ok("Đã khôi phục");
        }

       
        [HttpDelete("destroy/{id}")]
        public async Task<IActionResult> Destroy(int id)
        {
            var brand = await _context.Brands.FindAsync(id);

            if (brand == null)
                return NotFound();

            _context.Brands.Remove(brand);
            await _context.SaveChangesAsync();

            return Ok("Đã xóa vĩnh viễn");
        }
    }
}
