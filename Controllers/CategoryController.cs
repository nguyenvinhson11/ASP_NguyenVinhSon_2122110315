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
    public class CategoryController : ControllerBase
    {
        private readonly AppDbContext _context;

        public CategoryController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/Category
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Category>>> Get()
        {
            var data = await _context.Categories
                .Include(c => c.CreatedByUser)
                .Include(c => c.UpdatedByUser)
                .Include(c => c .DeletedByUser)
                .Where(c => c.DeletedAt == null)
                .ToListAsync();
            return Ok(data);
        }

        // GET: api/Category/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Category>> Show(int id)
        {
            var category = await _context.Categories.FindAsync(id);

            if (category == null || category.DeletedAt != null)
            {
                return NotFound();
            }

            return category;
        }

        // POST: api/Category
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CategoryStoreRequest request)
        {
            var userId = int.Parse(User.FindFirst("id")?.Value ?? "1");

            var category = new Category
            {
                Name = request.Name,
                CreatedBy = userId,
                CreatedAt = DateTime.Now,
                UpdatedAt = null,
                DeletedAt = null,
                DeletedBy = null,
                UpdatedBy = null
            };

            _context.Categories.Add(category);
            await _context.SaveChangesAsync();

            return Ok(category);
        }


        // PUT: api/Category/5
        [HttpPut("{id}")]
       
        public async Task<IActionResult> Update(int id, [FromBody] CategoryUpdateRequest request)
        {
            var category = await _context.Categories.FindAsync(id);
            var userId = int.Parse(User.FindFirst("id")?.Value ?? "1");
            if (category == null || category.DeletedAt != null)
                return NotFound();

            if (!string.IsNullOrWhiteSpace(request.Name))
                category.Name = request.Name;

            

          
            category.UpdatedAt = DateTime.Now;
            category.UpdatedBy = userId;
            category.DeletedAt = null;

            await _context.SaveChangesAsync();

            return Ok(category); // Trả về thông tin sau khi cập nhật
        }

        // DELETE: api/Category/5 (soft delete)
        [HttpDelete("{id}")]
      
        public async Task<IActionResult> Delete(int id)
        {
            var category = await _context.Categories.FindAsync(id);
            var userId = int.Parse(User.FindFirst("id")?.Value ?? "1");
            if (category == null || category.DeletedAt != null)
            {
                return NotFound();
            }

            category.DeletedAt = DateTime.Now;
            category.DeletedBy = userId;

            await _context.SaveChangesAsync();

            return Ok("Dã xóa mềm");
        }

        [HttpPut("restore/{id}")]
       
        public async Task<IActionResult> Restore(int id)
        {
            var category = await _context.Categories.FindAsync(id);
            var userId = int.Parse(User.FindFirst("id")?.Value ?? "1");
            if (category == null || category.DeletedAt == null)
                return NotFound();

            category.DeletedAt = null;
            category.UpdatedAt = DateTime.Now;
            category.UpdatedBy = userId;

            await _context.SaveChangesAsync();
            return Ok("Đã khôi phục");
        }


        [HttpDelete("destroy/{id}")]
        public async Task<IActionResult> Destroy(int id)
        {
            var category = await _context.Categories.FindAsync(id);

            if (category == null)
                return NotFound();

            _context.Categories.Remove(category);
            await _context.SaveChangesAsync();

            return Ok("Đã xóa vĩnh viễn");
        }
    }
}
