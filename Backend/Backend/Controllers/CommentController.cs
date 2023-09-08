using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Backend.DataBase.Entity;
using Backend.DataBase.Model;
using Backend.DataBase.Request;
using Microsoft.AspNetCore.Authorization;

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
        public async Task<ActionResult<IEnumerable<Comment>>> GetComment()
        {
            if (_context.Comment == null)
            {
                return NotFound();
            }

            return await _context.Comment.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Comment>> GetComment(int id)
        {
            if (_context.Comment == null)
            {
                return NotFound();
            }

            var comment = await _context.Comment.FindAsync(id);

            if (comment == null)
            {
                return NotFound();
            }

            return comment;
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutComment(int id, Comment comment)
        {
            if (id != comment.Id)
            {
                return BadRequest();
            }

            _context.Entry(comment).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CommentExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        [HttpPost]
        public async Task<ActionResult<Comment>> PostComment(Comment comment)
        {
            if (_context.Comment == null)
            {
                return Problem("Entity set 'AppDbContext.Comment'  is null.");
            }

            _context.Comment.Add(comment);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetComment", new { id = comment.Id }, comment);
        }

        [HttpPost("created-comment{IdPost}/user-{idUser}")]
        public async Task<IActionResult> CreatedComment(int IdPost,int idUser, [FromBody] CommentRequest commentRequest)
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
            };
            
            _context.Comment.Add(newComment);
            await _context.SaveChangesAsync();
            
            return Ok(newComment);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteComment(int id)
        {
            if (_context.Comment == null)
            {
                return NotFound();
            }

            var comment = await _context.Comment.FindAsync(id);
            if (comment == null)
            {
                return NotFound();
            }

            _context.Comment.Remove(comment);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool CommentExists(int id)
        {
            return (_context.Comment?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}