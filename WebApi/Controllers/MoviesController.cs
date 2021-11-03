using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApi.Models;

namespace WebApi.Controllers
{
    [Route("api/Movies")]
    [ApiController]
    public class MoviesController : ControllerBase
    {
        private readonly MoviesContext _context;

        public MoviesController(MoviesContext context)
        {
            _context = context;
            _context.Database.EnsureCreated();
        }

        // GET: api/TodoItems
        [HttpGet]
        public async Task<ActionResult<IEnumerable<MoviesDTO>>> GetMovie()
        {
            return await _context.Movie
                .Select(x => MoviesDTO(x)).ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<MoviesDTO>> GetMovies(int id)
        {
            var movies = await _context.Movie.FindAsync(id);

            if (movies == null)
            {
                return NotFound();
            }

            return MoviesDTO(movies);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutMovies(int id, MoviesDTO moviesDTO)
        {
            if (id != moviesDTO.Id)
            {
                return BadRequest();
            }

            var movies = await _context.Movie.FindAsync(id);
            if (movies == null)
            {
                return NotFound();
            }

            movies.Title = moviesDTO.Title;
            movies.Director = moviesDTO.Director;
            movies.Genre = moviesDTO.Genre;
            movies.Year = moviesDTO.Year;
            movies.Id = moviesDTO.Id;


            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException) when (!MoviesExists(id))
            {
                return NotFound();
            }

            return NoContent();
        }

        [HttpPost]
        public async Task<ActionResult<MoviesDTO>> PostMovies(MoviesDTO moviesDTO)
        {
            var movies = new Movies
            {
                Id = moviesDTO.Id,
                Title = moviesDTO.Title,
                Genre = moviesDTO.Genre,
                Director = moviesDTO.Director
            };

            _context.Movie.Add(movies);
            await _context.SaveChangesAsync();

            return base.CreatedAtAction(
                nameof(Movies),
                new { id = movies.Id },
                MoviesController.MoviesDTO(movies));
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMovies(int id)
        {
            var movie = await _context.Movie.FindAsync(id);

            if (movie == null)
            {
                return NotFound();
            }

            _context.Movie.Remove(movie);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool MoviesExists(int id) =>
             _context.Movie.Any(e => e.Id == id);

        private static MoviesDTO MoviesDTO(Movies movies) =>
            new MoviesDTO
            {
                Id = movies.Id,
                Title = movies.Title,
                Genre = movies.Genre,
                Director = movies.Director
            };
    }
}