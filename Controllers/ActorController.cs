using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
namespace Movie.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class ActorController : ControllerBase
    {
        private readonly DataContext _context;


        public ActorController(DataContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<List<Actor>>> ListActor()
        {
            var result = await _context.Actor.ToListAsync();
            return Ok(result);
        }

        [HttpGet("filter")]
        public async Task<ActionResult<List<Actor>>> ListActorLimit(int? limit = 5, string? name = "null", string? gender = "null")
        {
            var result = await _context.Actor.FromSqlRaw("EXEC [dbo].[FilterActor]"
                + "@Limit=" + limit + ","
                + "@Name=" + name + ","
                + "@Gender=" + gender).ToListAsync();


            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult> GetActor(int id)
        {
            var findActor = await _context.Actor.FindAsync(id);
            if (findActor == null)
            {
                return BadRequest("Not found ID : " + id + " in System");
            }
            else
            {
                return Ok(findActor);

            }

        }

        [HttpPost]
        public async Task<ActionResult<List<Actor>>> AddActor(Actor actor)
        {
            _context.Actor.Add(actor);
            await _context.SaveChangesAsync();
            return Ok(await _context.Actor.ToListAsync());

        }

        [HttpPut]
        public async Task<ActionResult<List<Actor>>> UpdateActor(Actor request)
        {
            var actor = await _context.Actor.FindAsync(request.ActorID);
            if (actor == null)
            {
                return BadRequest("Not found ID : " + request.ActorID + " in System");
            }

            actor.ActorName = request.ActorName;
            actor.ActorDOB = request.ActorDOB;
            actor.ActorGender = request.ActorGender;
            await _context.SaveChangesAsync();
            return Ok(actor);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<Actor>> DeleteActor(int id)
        {
            var actor = await _context.Actor.FindAsync(id);
            if (actor != null)
            {
                _context.Actor.Remove(actor);
                await _context.SaveChangesAsync();
                return Ok(await _context.Actor.ToListAsync());
            }
            else
            {
                return BadRequest("Not found");
            }
        }
    }
}
