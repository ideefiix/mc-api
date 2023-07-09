using Api.Authorization;
using Api.DAL;
using Api.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Api.Controllers;

[Route("api/[controller]")]
public class ItemController : ControllerBase
{
    private readonly DatabaseContext _context;

    public ItemController(DatabaseContext context)
    {
        _context = context;
    }
    [HasClaim("id", "playerId")]
    [HttpGet("{playerId}")]
    public async Task<ActionResult<IEnumerable<PlayerItem>>> GetItemsForPlayer(Guid playerId)
    {
        return await _context.PlayerItems.Where(item => item.Owner.PlayerId == playerId).ToListAsync();
    }
}