using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Web_Api_Cinema.Data;
using Web_Api_Cinema.DTOs;
using Web_Api_Cinema.Entities;
using AutoMapper;
using Web_Api_Cinema.Dtos;

namespace Web_Api_Cinema.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MovieController : ControllerBase
    {
        private readonly MovieDbContext ctx;
        private readonly IMapper _mapper;
        private readonly ILogger<MovieController> _logger;
        public MovieController(MovieDbContext ctx, IMapper mapper, ILogger<MovieController> logger)
        {
            this.ctx = ctx;
            _mapper = mapper;
            _logger = logger;

        }

        [HttpGet]
        public ActionResult GetAll(string? genre = null, string? sortBy = null)
        {
            var items = ctx.Movies.AsQueryable();

            
            if (!string.IsNullOrEmpty(genre))
                items = items.Where(m => m.Genre == genre);

            
            if (sortBy == "year")
                items = items.OrderBy(m => m.Year);
            else if (sortBy == "title")
                items = items.OrderBy(m => m.Title);

            return Ok(_mapper.Map<List<Movie>>(items.ToList()));
        }


        [HttpGet("{id}")]
        public ActionResult Get(int id)
        {
            var movie = ctx.Movies.Find(id);
            if (movie == null) return NotFound();
            return Ok(movie);
        }
      
        [HttpPost]
        public IActionResult Create([FromBody] CreateMovieDto dto)
        {
            _logger.LogInformation("Creating a new movie: {Title}", dto.Title);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var movie = _mapper.Map<Movie>(dto);
            ctx.Movies.Add(movie);
            ctx.SaveChanges();

            _logger.LogInformation("Movie created with ID: {Id}", movie.Id);
            return CreatedAtAction(nameof(Get), new { id = movie.Id }, movie);
        }


        [HttpPut("{id}")]
        public ActionResult Update(int id, [FromBody] UpdateMovieDto dto)
        {
            if (id != dto.Id) return BadRequest("Movie ID mismatch.");
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var existingMovie = ctx.Movies.Find(id);
            if (existingMovie == null) return NotFound();

            _mapper.Map(dto, existingMovie); 
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
