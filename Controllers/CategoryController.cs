using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NguyenVinhSon_2122110315.Data;
using NguyenVinhSon_2122110315.Model;

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
        public ActionResult<IEnumerable<Category>> Get()
        {
            var categories = _context.Categories.ToList();
            return Ok(categories);
        }

        // GET: api/Category/5
        [HttpGet("{id}")]
        public ActionResult<Category> Get(int id)
        {
            var category = _context.Categories.Find(id);
            if (category == null)
            {
                return NotFound();
            }
            return Ok(category);
        }

        // POST: api/Category
        [HttpPost]
        public ActionResult<Category> Post([FromBody] Category category)
        {
            category.CreatedAt = DateTime.Now;
            category.UpdatedAt = DateTime.Now;

            _context.Categories.Add(category);
            _context.SaveChanges();

            return Ok(category);
        }

        // PUT: api/Category/5
        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody] Category category)
        {
            var existingCategory = _context.Categories.Find(id);
            if (existingCategory == null)
            {
                return NotFound();
            }

            existingCategory.Name = category.Name;
            existingCategory.UpdatedBy = category.UpdatedBy;
            existingCategory.UpdatedAt = DateTime.Now;

            _context.SaveChanges();

            return Ok(existingCategory);
        }


        // DELETE: api/Category/5
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var category = _context.Categories.Find(id);
            if (category == null)
            {
                return NotFound();
            }

            category.DeletedAt = DateTime.Now;
            category.DeletedBy = "Sơn"; // 👈 Có thể lấy từ người dùng đăng nhập
            _context.SaveChanges();

            return Ok(new { message = "Đã xóa mềm danh mục" });
        }

    }
}
