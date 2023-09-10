using Api.Authorization;
using Api.DAL;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[IsPlayer]
[Route("api/[controller]")]
[ApiController]
public class MissionController : ControllerBase
{
    private readonly DatabaseContext _context;

    public MissionController(DatabaseContext context)
    {
        _context = context;
    }

    [HttpPost]
    public ActionResult StartMission()
    {
        
    }
    
    
}