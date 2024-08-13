using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using MovieService.Data.MovieData;
using MovieService.Models;
using MovieService.Dtos;
using System.Collections.Generic;
using System.Threading.Tasks;
using MovieService.Dtos.MovieDtos;
using Microsoft.AspNetCore.Authorization;

namespace MovieService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MoviesController : ControllerBase
    {
        private readonly IMovieRepo _repository;
        private readonly IMapper _mapper;

        public MoviesController(IMovieRepo repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        // GET: api/movies
        [HttpGet]
        [Authorize(Policy = "AllCanAccess")]
        public async Task<ActionResult<IEnumerable<MovieReadDto>>> GetAllMovies()
        {
            var movies = await _repository.GetAllAsync();
            return Ok(_mapper.Map<IEnumerable<MovieReadDto>>(movies));
        }

        // GET: api/movies/{id}
        [HttpGet("{id}")]
        [Authorize(Policy = "AdminOnly")]
        public async Task<ActionResult<MovieReadDto>> GetMovieById(int id)
        {
            var movie = await _repository.GetByIdAsync(id);
            if (movie == null)
            {
                return NotFound();
            }
            return Ok(_mapper.Map<MovieReadDto>(movie));
        }

        // GET: api/movies/url/{url}
        [HttpGet("url/{url}")]
        [Authorize(Policy = "AllCanAccess")]
        public async Task<ActionResult<MovieReadDto>> GetMovieByUrl(string url)
        {
            var movie = await _repository.GetByUrlAsync(url);
            if (movie == null)
            {
                return NotFound();
            }
            return Ok(_mapper.Map<MovieReadDto>(movie));
        }

        // POST: api/movies
        [HttpPost]
        [Authorize(Policy = "AllCanAccess")]
        public async Task<ActionResult<MovieReadDto>> CreateMovie(MovieCreateDto movieCreateDto)
        {
            var movieModel = _mapper.Map<Movie>(movieCreateDto);
            await _repository.InsertAsync(movieModel);

            var movieReadDto = _mapper.Map<MovieReadDto>(movieModel);

            return CreatedAtAction(nameof(GetMovieById), new { id = movieReadDto.Id }, movieReadDto);
        }

        // PUT: api/movies/{id}
        [HttpPut("{id}")]
        [Authorize(Policy = "AdminOnly")]
        public async Task<IActionResult> UpdateMovie(int id, MovieCreateDto movieCreateDto)
        {
            var movieModel = await _repository.GetByIdAsync(id);
            if (movieModel == null)
            {
                return NotFound();
            }

            _mapper.Map(movieCreateDto, movieModel);
            await _repository.UpdateAsync(movieModel);

            return NoContent();
        }

        [HttpPatch("{id}/disable")]
        [Authorize(Policy = "AdminOnly")]
        public async Task<IActionResult> DisableMovie(int id)
        {
            var movie = await _repository.GetByIdAsync(id);
            if (movie == null)
            {
                return NotFound();
            }

            await _repository.DisableMovieAsync(id);

            return NoContent();
        }

    }
}
