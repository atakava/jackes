using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Backend.DataBase.Entity;
using Backend.DataBase.Model;
using Backend.DataBase.Request;

namespace Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CommentController : ControllerBase
    {
        private readonly AppDbContext _context;

        public CommentController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Comment>>> GetAllComment()
        {
            var comment = await _context.Comment.ToListAsync();
            return Ok(comment);
        }

        [HttpGet("user/{id}/comments")]
        public async Task<IActionResult> GetUserComments(int id)
        {
            var userComments = _context.Comment.Where(u => u.UserId == id);

            if (userComments == null)
            {
                return NotFound();
            }

            return Ok(userComments);
        }

        [HttpPost("created-comment{IdPost}/user-{idUser}")]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> CreatedComment(int IdPost, int idUser,
            [FromForm] CommentRequest commentRequest)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var postValid = await _context.Posts.FirstOrDefaultAsync(p => p.Id == IdPost);
            var userValid = await _context.User.FirstOrDefaultAsync(p => p.Id == idUser);

            if (postValid == null)
            {
                return NotFound("Пост не найден");
            }

            if (userValid == null)
            {
                return NotFound("Пользователя не существует");
            }

            var newComment = new Comment
            {
                Text = commentRequest.Text,
                CreatedAt = DateTime.UtcNow,
                PostId = IdPost,
                UserId = idUser,
                UserName = userValid.Login,
                UserAvatar = userValid.Avatar
            };

            if (commentRequest.Imgs != null && commentRequest.Imgs.Any())
            {
                newComment.Imgs = new List<string>();

                foreach (var imgFile in commentRequest.Imgs)
                {
                    if (imgFile.Length > 0)
                    {
                        var uniqueFileName = Guid.NewGuid().ToString() + "_" + Path.GetExtension(imgFile.FileName);

                        var filePath = Path.Combine("wwwroot/img", uniqueFileName);
                        using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                            await imgFile.CopyToAsync(stream);
                        }

                        newComment.Imgs.Add("http://localhost:5273/img/" + uniqueFileName);
                    }
                }
            }

            _context.Comment.Add(newComment);
            await _context.SaveChangesAsync();

            return Ok(newComment);
        }

        [HttpPost("update/user/{userId}/post/{postId}/comment/{commentId}")]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> UpdateComment(int userId, int postId, int commentId,
            [FromForm] CommentUpdateRequest commentUpdateRequest)
        {
            var user = await _context.User.FindAsync(userId);
            var post = await _context.Posts.FindAsync(postId);
            var comment = await _context.Comment.FindAsync(commentId);

            if (user == null || post == null || comment == null)
            {
                return NotFound();
            }

            if (commentUpdateRequest.ImgIndex != null && commentUpdateRequest.ImgIndex.Count > 0)
            {
                foreach (var imgIndex in commentUpdateRequest.ImgIndex)
                {
                    if (comment.Imgs != null && imgIndex >= 0 && imgIndex < comment.Imgs.Count)
                    {
                        var imgFile = commentUpdateRequest.Imgs[imgIndex];
                        using (var ms = new MemoryStream())
                        {
                            var uniqueFileName = Guid.NewGuid().ToString() + "_" + Path.GetExtension(imgFile.FileName);
                            var filePath = Path.Combine("wwwroot/img", uniqueFileName);

                            using (var stream = new FileStream(filePath, FileMode.Create))
                            {
                                await imgFile.CopyToAsync(stream);
                            }

                            comment.Imgs[imgIndex] = $"http://localhost:5273/img/{uniqueFileName}";
                        }
                    }
                    else
                    {
                        return BadRequest($"Неверный индекс изображения: {imgIndex}");
                    }
                }
            }

            _context.SaveChangesAsync();
            return Ok(comment);
        }

        [HttpDelete("deleted-comment/{id}")]
        public async Task<IActionResult> DeleteComment(int id)
        {
            var comment = await _context.Comment.FindAsync(id);

            if (comment == null)
            {
                return NotFound();
            }

            _context.Comment.Remove(comment);
            _context.SaveChangesAsync();

            return NoContent();
        }

        private bool CommentExists(int id)
        {
            return (_context.Comment?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}