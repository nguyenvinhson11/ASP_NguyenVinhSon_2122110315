using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NguyenVinhSon_2122110315.Data;
using NguyenVinhSon_2122110315.Helper;
using NguyenVinhSon_2122110315.Model;
using NguyenVinhSon_2122110315.Request;

namespace NguyenVinhSon_2122110315.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly AppDbContext _context;

        public UserController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/User
        [HttpGet]
        public async Task<ActionResult<IEnumerable<User>>> Get()
        {
            return await _context.Users
                .Where(u => u.DeletedAt == null)
                .ToListAsync();
        }

        // GET: api/User/5
        [HttpGet("{id}")]
        public async Task<ActionResult<User>> Show(int id)
        {
            var user = await _context.Users.FindAsync(id);

            if (user == null || user.DeletedAt != null)
                return NotFound();

            return Ok( user);
        }

        // POST: api/User
        [HttpPost]
        public async Task<IActionResult> Create([FromForm] UserStoreRequest request, IFormFile? AvatarFile)
        {
            string? avatarPath = null;

            // Nếu có ảnh thì xử lý lưu
            if (AvatarFile != null && AvatarFile.Length > 0)
            {
                var uploadsDir = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads", "user");

                if (!Directory.Exists(uploadsDir))
                    Directory.CreateDirectory(uploadsDir);

                // Tạo slug từ full name
                string slug = SlugHelper.ToSlug(request.FullName ?? "user");

                // Tạo tên file từ slug + đuôi mở rộng ảnh
                var fileExt = Path.GetExtension(AvatarFile.FileName);
                var fileName = $"{slug}{fileExt}";
                var filePath = Path.Combine(uploadsDir, fileName);

                // Lưu file
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await AvatarFile.CopyToAsync(stream);
                }

                // Lưu đường dẫn ảnh (tương đối) vào DB
                avatarPath =  fileName;
            }

            // Gán dữ liệu vào model
            var user = new User
            {
                FullName = request.FullName,
                Email = request.Email,
                Phone = request.Phone,
                Password = request.Password,
                Role = request.Role ?? "customer",
                Avatar = avatarPath,
                Address = request.Address,


                CreatedAt = DateTime.Now,
                UpdatedAt = null,
                DeletedAt = null
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return Ok(user);
        }



        // PUT: api/User/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromForm] UserUpdateRequest request, IFormFile? AvatarFile)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null || user.DeletedAt != null)
                return NotFound();

            // ✅ Gán thông tin nếu có
            if (!string.IsNullOrWhiteSpace(request.FullName))
                user.FullName = request.FullName;

            if (!string.IsNullOrWhiteSpace(request.Email))
                user.Email = request.Email;

            if (!string.IsNullOrWhiteSpace(request.Phone))
                user.Phone = request.Phone;
            
            if (!string.IsNullOrWhiteSpace(request.Address))
                user.Address = request.Address;


            if (!string.IsNullOrWhiteSpace(request.Password))
                user.Password = request.Password;

            if (!string.IsNullOrWhiteSpace(request.Role))
                user.Role = request.Role;

            user.UpdatedBy = request.UpdatedBy;
            user.UpdatedAt = DateTime.Now;

            // ✅ Nếu có upload ảnh mới
            if (AvatarFile != null && AvatarFile.Length > 0)
            {
                var uploadsDir = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads", "user");

                if (!Directory.Exists(uploadsDir))
                    Directory.CreateDirectory(uploadsDir);

                string slug = SlugHelper.ToSlug(user.FullName);
                string fileExt = Path.GetExtension(AvatarFile.FileName);
                string fileName = $"{slug}{fileExt}";
                string filePath = Path.Combine(uploadsDir, fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await AvatarFile.CopyToAsync(stream);
                }

                user.Avatar = fileName;
            }

            await _context.SaveChangesAsync();
            return Ok(user);
        }





        // DELETE: api/User/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
                return NotFound();

            user.DeletedAt = DateTime.Now;
            user.DeletedBy = 1;

            await _context.SaveChangesAsync();

            return Ok("Đã xóa mềm");
        }

        // GET: api/User/trash
        [HttpGet("trash")]
        public async Task<ActionResult<IEnumerable<User>>> Trash()
        {
            var users = await _context.Users
                .Where(u => u.DeletedAt != null)
                .ToListAsync();

            return Ok(users);
        }


        // [PUT] api/User/restore/5
        [HttpPut("restore/{id}")]
        public async Task<IActionResult> Restore(int id)
        {
            var user = await _context.Users.FindAsync(id);

            if (user == null || user.DeletedAt == null)
                return NotFound();

            user.DeletedAt = null;
            user.UpdatedAt = DateTime.Now;

            await _context.SaveChangesAsync();
            return Ok(  "Đã khôi phục"); 
        }

        // [DELETE] api/User/destroy/5
        [HttpDelete("destroy/{id}")]
        public async Task<IActionResult> Destroy(int id)
        {
            var user = await _context.Users.FindAsync(id);

            if (user == null)
                return NotFound();

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();

            return Ok("Đã xóa vĩnh viễn");
        }

    }
}
