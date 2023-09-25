using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Backend.DataBase.Entity;
using Backend.DataBase.Model;
using Backend.DataBase.Model.Request;
using Microsoft.AspNetCore.Http.HttpResults;

namespace Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TeamController : ControllerBase
    {
        private readonly AppDbContext _context;

        public TeamController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Team>>> GetTeam()
        {
            if (_context.Team == null)
            {
                return NotFound();
            }

            return await _context.Team.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Team>> GetTeam(int id)
        {
            if (_context.Team == null)
            {
                return NotFound();
            }

            var team = await _context.Team.FindAsync(id);

            if (team == null)
            {
                return NotFound();
            }

            return team;
        }

        [HttpPost("created-team/user/{adminId}")]
        // [Consumes("multipart/form-data")]
        public async Task<IActionResult> CreatedTeam(int adminId, [FromBody] TeamRequest teamRequest)
        {
            var admin = await _context.User.FirstOrDefaultAsync(u => u.Id == adminId);
            if (admin == null)
            {
                return NotFound($"Пользователя с {adminId} не найдено");
            }

            var team = new Team
            {
                AdminId = admin.Id,
                Admin = admin,
                Name = teamRequest.Name,
                LongDesc = teamRequest.LongDesc,
                ShortDesc = teamRequest.ShortDesc
            };
            _context.Team.Add(team);
            await _context.SaveChangesAsync();

            return Ok(team);
        }

        [HttpPost("add-team/user/{userId}/team/{teamId}")]
        public async Task<IActionResult> AddUserToTeam(int userId, int teamId)
        {
            var team = await _context.Team
                .Include(t => t.Users)
                .FirstOrDefaultAsync(t => t.Id == teamId);
            
            if (team == null)
            {
                return NotFound($"Группа с Id {teamId} не найдена");
            }

            var user = await _context.User.FirstOrDefaultAsync(u => u.Id == userId);
            
            if (user == null)
            {
                return NotFound($"User с Id {teamId} не найден");
            }

            if (team.Users == null)
            {
                team.Users = new List<User>();
            }

            if (team.Users.Any(u => u.Id == userId))
            {
                return BadRequest($"Пользователь с Id {userId} уже является участником группы с Id {teamId}");
            }

            user.Role = RoleType.Developer;
            
            team.Users.Add(user);
            await _context.SaveChangesAsync();

            return Ok(team);
        }
        
        private bool TeamExists(int id)
        {
            return (_context.Team?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}