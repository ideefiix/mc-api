using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Api.DAL;
using Api.DAL.DTO;
using Api.Models;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;

namespace Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly DatabaseContext _context;
        private readonly IConfiguration _config;

        public AuthController(DatabaseContext context, IConfiguration config)
        {
            _context = context;
            _config = config;
        }

        [HttpPost("/login")]
        public async Task<ActionResult> Login(LoginDTO req)
        {
            Player? player = _context.Players.FirstOrDefault(p => p.PlayerName == req.Name);

            if (player == null) return BadRequest("User not found");

            if (!BCrypt.Net.BCrypt.Verify(req.Password, player.PasswordHash)) return BadRequest("Invalid password");

            var token = CreateToken(player);

            return Ok(token);
        }

        private string CreateToken(Player player)
        {
            List<Claim> claims = new List<Claim>()
            {
                new Claim(ClaimTypes.Name, player.PlayerName),
                new Claim("id", player.PlayerId.ToString())
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config.GetSection("Token").Value!));

            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);
            
            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.Now.AddDays(1),
                signingCredentials: credentials
                );
            var jwt = new JwtSecurityTokenHandler().WriteToken(token);
            return jwt;
        }
        
    }
}
