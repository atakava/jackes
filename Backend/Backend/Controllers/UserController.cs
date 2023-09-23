using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Backend.DataBase.Entity;
using Backend.DataBase.Model;
using Backend.DataBase.Request;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public UserController(AppDbContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<User>>> GetUser()
        {
            if (_context.User == null)
            {
                return NotFound();
            }

            return await _context.User.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<User>> GetUser(int id)
        {
            if (_context.User == null)
            {
                return NotFound();
            }

            var user = await _context.User.FindAsync(id);

            if (user == null)
            {
                return NotFound();
            }

            return user;
        }

        [HttpGet("current-user")]
        public async Task<IActionResult> CurrentUser()
        {
            if (!User.Identity.IsAuthenticated)
            {
                return Unauthorized();
            }

            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var currentUser = await _context.User
                .Include(u => u.Comments)
                .Include(u => u.Posts)
                .FirstOrDefaultAsync(i => i.Id == int.Parse(userId));

            return Ok(currentUser);
        }

        [HttpPost]
        public async Task<ActionResult<User>> PostUser(User user)
        {
            if (_context.User == null)
            {
                return Problem("Entity set 'AppDbContext.User'  is null.");
            }

            _context.User.Add(user);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetUser", new { id = user.Id }, user);
        }

        [HttpPost("register")]
        public async Task<ActionResult> RegisterUser([FromBody] UserRequest userRequest)
        {
            if (await _context.User.AnyAsync(u => u.Login == userRequest.Login || u.Mail == userRequest.Mail))
            {
                return BadRequest("Почта или логин заняты");
            }

            var entity = new User
            {
                Login = userRequest.Login,
                Password = userRequest.Password,
                Mail = userRequest.Mail,
                Avatar = "http://localhost:5273/img/29acce9d-9d4f-4583-b2f2-319a3b58134c_.jpg",
                Role = "User",
            };

            _context.User.Add(entity);
            await _context.SaveChangesAsync();

            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, entity.Id.ToString()),
                new Claim(ClaimTypes.Name, entity.Login),
                new Claim(ClaimTypes.Email, entity.Mail),
                new Claim(ClaimTypes.Uri, entity.Avatar),
                new Claim(ClaimTypes.Role, entity.Role),
            };

            var indentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var principal = new ClaimsPrincipal(indentity);

            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);

            return Ok("Регистрация прошла успешно");
        }

        [HttpPost("login")]
        public async Task<IActionResult> LoginUser([FromBody] UserRequest userRequest)
        {
            var authUser =
                await _context.User.FirstOrDefaultAsync(u =>
                    u.Login == userRequest.Login && u.Password == userRequest.Password);

            if (authUser == null)
            {
                return BadRequest("Не верный логин или пароль");
            }

            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, authUser.Id.ToString()),
                new Claim(ClaimTypes.Name, authUser.Login),
                new Claim(ClaimTypes.Email, authUser.Mail),
                new Claim(ClaimTypes.Role, authUser.Role),
                new Claim(ClaimTypes.Uri, authUser.Avatar),
            };

            var indentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var principal = new ClaimsPrincipal(indentity);

            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);

            return Ok("Вы успешно зашли в аккаунт");
        }

        [HttpPost("logout")]
        public async Task<IActionResult> LogoutUser()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return Ok("Вы успешно вышли из аккаунта");
        }

        [HttpPost("update-user/{id}")]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> UpdateUser(int id, [FromForm] UpdateUserRequest updateUserRequest)
        {
            var user = await _context.User.FindAsync(id);

            if (user != null)
            {
                user.Mail = updateUserRequest.Mail ?? user.Mail;
                user.Login = updateUserRequest.Login ?? user.Login;
            }

            if (updateUserRequest.Avatar != null)
            {
                var fileExentions = Path.GetExtension(updateUserRequest.Avatar.FileName);

                var wwwRootPath = _webHostEnvironment.WebRootPath;
                var avatarsPath = Path.Combine(wwwRootPath, "img");
                var avatarName = $"{Guid.NewGuid().ToString()}_{fileExentions}";
                var avatarPath = Path.Combine(avatarsPath, avatarName);

                using (var stream = new FileStream(avatarPath, FileMode.Create))
                {
                    await updateUserRequest.Avatar.CopyToAsync(stream);
                }

                user.Avatar = $"http://localhost:5273/img/{avatarName}";
            }

            _context.User.Update(user);
            await _context.SaveChangesAsync();

            return Ok(user);
        }

        [HttpDelete("deleted-user/{id}")]
        public async Task<IActionResult> DeletedUser(int id)
        {
            if (_context.User == null)
            {
                return NotFound();
            }

            var user = await _context.User.FindAsync(id);

            if (user == null)
            {
                return NotFound();
            }

            _context.User.Remove(user);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool UserExists(int id)
        {
            return (_context.User?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}