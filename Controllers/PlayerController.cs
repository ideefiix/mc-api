using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Api.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Api.DAL;
using Api.DAL.DTO;
using Api.Models;
using Microsoft.AspNetCore.Authorization;

namespace Api.Controllers
{
    [IsPlayer]
    [Route("api/[controller]")]
    [ApiController]
    public class PlayerController : ControllerBase
    {
        private readonly DatabaseContext _context;

        public PlayerController(DatabaseContext context)
        {
            _context = context;
        }

        // GET: api/Player
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Player>>> GetPlayers()
        {
            return await _context.Players.ToListAsync();
        }

        // GET: api/Player/5
        [HasClaim("id", "id")]
        [HttpGet("{id}")]
        public async Task<ActionResult<Player>> GetPlayer(Guid id)
        {
            var player = await _context.Players.FindAsync(id);

            if (player == null)
            {
                return NotFound();
            }

            return player;
        }
        
        [AllowAnonymous]
        [HttpPost]
        public async Task<ActionResult> RegisterPlayer([FromBody] RegisterDTO dto)
        {
            if (PlayerExists(dto.Name)) return Conflict("Username already taken");
            if (dto.Password == "") return BadRequest("Password cant be empty");
            
            string hashedPassword = BCrypt.Net.BCrypt.HashPassword(dto.Password);
            
            Player newPlayer = new Player
            {
                PlayerName = dto.Password,
                PasswordHash = hashedPassword,
                MaxHp = 100,
                Hp = 100,
                Dmg = 0,
                Defence = 0,
            };

            _context.Players.Add(newPlayer);
            await _context.SaveChangesAsync();
            return Created("player", newPlayer);
        }

        // PUT: api/Player/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutPlayer(Guid id, Player player)
        {
            if (id != player.PlayerId)
            {
                return BadRequest();
            }

            _context.Entry(player).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PlayerExists(id))
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
        
        // DELETE: api/Player/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePlayer(Guid id)
        {
            var player = await _context.Players.FindAsync(id);
            if (player == null)
            {
                return NotFound();
            }

            _context.Players.Remove(player);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool PlayerExists(Guid id)
        {
            return _context.Players.Any(e => e.PlayerId == id);
        }
        private bool PlayerExists(String playerName)
        {
            return _context.Players.Any(e => e.PlayerName == playerName);
        }
    }
}
