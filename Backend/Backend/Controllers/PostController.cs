using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Backend.DataBase.Entity;
using Backend.DataBase.Model;
using Backend.DataBase.Request;

namespace Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PostController : ControllerBase
    {
        private readonly AppDbContext _context;

        public PostController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Post>>> GetAllPost()
        {
            var post = await _context.Posts.ToListAsync();
            return Ok(post);
        }
        
        [HttpGet("user/{id}/posts")]
        public async Task<IActionResult> GetUserPosts(int id)
        {
            var userPosts = await _context.Posts.Include(u => u.Comments).Where(p => p.UserId == id).ToListAsync();

            if (userPosts == null)
            {
                return NotFound();
            }

            return Ok(userPosts);
        }

        [HttpPost("created-post-user{id}")]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> CreatedPost(int id, [FromForm] PostRequest postRequest)
        {
            var user = await _context.User.FindAsync(id);

            if (user == null)
            {
                return NotFound();
            }

            var newPost = new Post
            {
                CreatedAt = DateTime.UtcNow,
                UserId = id,
                Title = postRequest.Title,
                Text = postRequest.Text,
                StatusPost = StatusPost.Active
            };

            if (postRequest.Imgs != null && postRequest.Imgs.Any())
            {
                newPost.Imgs = new List<string>();

                foreach (var imgFile in postRequest.Imgs)
                {
                    if (imgFile.Length > 0)
                    {
                        var uniqueFileName = Guid.NewGuid().ToString() + "_" + Path.GetExtension(imgFile.FileName);

                        var filePath = Path.Combine("wwwroot/img", uniqueFileName);
                        using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                            await imgFile.CopyToAsync(stream);
                        }

                        newPost.Imgs.Add($"http://localhost:5273/img/{uniqueFileName}");
                    }
                }
            }

            _context.Posts.Add(newPost);
            await _context.SaveChangesAsync();

            return Ok(newPost);
        }

        [HttpPost("user/{id}/post-update")]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> UpdatePost(int id, [FromForm] PostUpdateRequest postUpdateRequest)
        {
            var userPost = await _context.Posts.FindAsync(id);

            if (userPost == null)
            {
                userPost.Title = postUpdateRequest.Title ?? userPost.Title;
                userPost.Text = postUpdateRequest.Text ?? userPost.Text;
            }

            if (postUpdateRequest.ImgIndexToUpdate != null && postUpdateRequest.ImgIndexToUpdate.Count > 0)
            {
                foreach (var imgIndex in postUpdateRequest.ImgIndexToUpdate)
                {
                    if (userPost.Imgs != null && imgIndex >= 0 && imgIndex < userPost.Imgs.Count)
                    {
                        var imgFile = postUpdateRequest.Imgs[imgIndex];
                        using (var ms = new MemoryStream())
                        {
                            var uniqueFileName = Guid.NewGuid().ToString() + "_" + Path.GetExtension(imgFile.FileName);
                            var filePath = Path.Combine("wwwroot/img", uniqueFileName);

                            using (var stream = new FileStream(filePath, FileMode.Create))
                            {
                                await imgFile.CopyToAsync(stream);
                            }

                            userPost.Imgs[imgIndex] = $"http://localhost:5273/img/{uniqueFileName}";
                        }
                    }
                    else
                    {
                        return BadRequest($"Неверный индекс изображения: {imgIndex}");
                    }
                }
            }

            // _context.Posts.Add(userPost);
            _context.SaveChangesAsync();

            return Ok(userPost);
        }

        [HttpDelete("deleted-post/{id}")]
        public async Task<IActionResult> DeletePost(int id)
        {
            var post = await _context.Posts.FindAsync(id);

            if (post == null)
            {
                return NotFound();
            }

            _context.Posts.Remove(post);
            _context.SaveChangesAsync();

            return NoContent();
        }

        private bool PostExists(int id)
        {
            return (_context.Posts?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}