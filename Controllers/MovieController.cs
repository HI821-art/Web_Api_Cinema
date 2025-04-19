using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Web_Api_Cinema.Data;
using Web_Api_Cinema.Entities;

namespace Web_Api_Cinema.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MovieController : ControllerBase
    {
        private readonly MovieDbContext ctx;

        public MovieController(MovieDbContext ctx)
        {
            this.ctx = ctx;
        }

        [HttpGet]
        public ActionResult GetAll()
        {
            var movies = ctx.Movies.ToList();
            return Ok(movies);
        }

      
        [HttpGet("{id}")]
        public ActionResult Get(int id)
        {
            var movie = ctx.Movies.Find(id);
            if (movie == null) return NotFound();
            return Ok(movie);
        }

       
        [HttpPost]
        public ActionResult Create([FromBody] Movie movie)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            ctx.Movies.Add(movie);
            ctx.SaveChanges();
            return CreatedAtAction(nameof(Get), new { id = movie.Id }, movie);
        }

        
        [HttpPut("{id}")]
        public ActionResult Update(int id, [FromBody] Movie movie)
        {
            if (id != movie.Id) return BadRequest("Movie ID mismatch.");
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var existingMovie = ctx.Movies.Find(id);
            if (existingMovie == null) return NotFound();

           
            existingMovie.Title = movie.Title;
            existingMovie.Year = movie.Year;
            existingMovie.Description = movie.Description;
            existingMovie.Genre = movie.Genre;
            existingMovie.Duration = movie.Duration;
            existingMovie.CoverImage = movie.CoverImage;
            existingMovie.Country = movie.Country;
            existingMovie.TrailerUrl = movie.TrailerUrl;
            existingMovie.DirectorId = movie.DirectorId;

            ctx.SaveChanges();
            return NoContent();
        }

        
        [HttpDelete("{id}")]
        public ActionResult Delete(int id)
        {
            var movie = ctx.Movies.Find(id);
            if (movie == null) return NotFound();

            ctx.Movies.Remove(movie);
            ctx.SaveChanges();
            return NoContent();
        }
    }
}
