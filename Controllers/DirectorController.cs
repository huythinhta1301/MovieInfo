using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Movie.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DirectorController : ControllerBase
    {
        private DataContext _dataContext;
        public DirectorController(DataContext dataContext)
        {
            _dataContext = dataContext;
        }
        [HttpGet]
        public async Task<ActionResult<List<Director>>> ShowAllDirector()
        {
            var listDirector = await _dataContext.Director.ToListAsync();
            return Ok(listDirector);
        }
        [HttpPost]
        public async Task<ActionResult> ShowDirector(int id)
        {
            var director = await _dataContext.Director.FindAsync(id);
            if (director != null)
            {
                return Ok(director);
            }
            else
            {
                return BadRequest($"Sorry! Cant find {id} in the system");
            }
        }
        [HttpPost("filter")]
        public async Task<ActionResult> FilterDirector(string? name = "null", string? DOB= "null", string? gender = "null")
        {
            var filterDirector = await _dataContext.Director.FromSqlRaw($"EXEC FilterDirector " 
                + "@Name=" + name + ","
                + "@DOB=" + DOB + ","
                +"@Gender=" + gender).ToListAsync();

            return Ok(filterDirector);
        }
        [HttpPost("add")]
        public async Task<ActionResult<List<Director>>> AddDirector(Director dire)
        {
            _dataContext.Director.Add(dire);
            await _dataContext.SaveChangesAsync();
            return Ok(_dataContext.Director.ToListAsync());
        }
    }
}
