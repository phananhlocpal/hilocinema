using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using TheaterService.Dtos;
using TheaterService.Models;

namespace TheaterService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TheatersController : ControllerBase
    {
        private readonly TheaterContext _context;
        private readonly IMapper _mapper;

        public TheatersController(TheaterContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // GET: api/Theaters
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TheaterReadDto>>> GetTheaters()
        {
            var theaters = await _context.Theaters.ToListAsync();
            return Ok(_mapper.Map<IEnumerable<TheaterReadDto>>(theaters));
        }

        // GET: api/Theaters/5
        [HttpGet("{id}")]
        public async Task<ActionResult<TheaterReadDto>> GetTheater(int id)
        {
            var theater = await _context.Theaters.FindAsync(id);

            if (theater == null)
            {
                return NotFound();
            }

            return Ok(_mapper.Map<TheaterReadDto>(theater));
        }

        // PUT: api/Theaters/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTheater(int id, TheaterReadDto theaterUpdateDto)
        {

            var theater = await _context.Theaters.FindAsync(id);
            if (theater == null)
            {
                return NotFound();
            }

            // Map the updates from theaterUpdateDto to the existing Theater entity
            _mapper.Map(theaterUpdateDto, theater);

            // Save changes to the database
            await _context.SaveChangesAsync();

            return NoContent();
        }
        [HttpPut("{id}/disable")]
        public async Task<IActionResult> DisableTheater(int id)
        {
            var theater = await _context.Theaters.FindAsync(id);

            if (theater == null)
            {
                return NotFound();
            }

            theater.Status = "Inactive";
            _context.Entry(theater).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TheaterExists(id))
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
        // POST: api/Theaters
        [HttpPost]
        public async Task<ActionResult<TheaterReadDto>> PostTheater(TheaterCreateDto theaterCreateDto)
        {
            var theater = _mapper.Map<Theater>(theaterCreateDto);
            _context.Theaters.Add(theater);
            await _context.SaveChangesAsync();

            var theaterReadDto = _mapper.Map<TheaterReadDto>(theater);

            return CreatedAtAction("GetTheater", new { id = theaterReadDto.Id }, theaterReadDto);
        }

        // DELETE: api/Theaters/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTheater(int id)
        {
            var theater = await _context.Theaters.FindAsync(id);
            if (theater == null)
            {
                return NotFound();
            }

            _context.Theaters.Remove(theater);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool TheaterExists(int id)
        {
            return _context.Theaters.Any(e => e.Id == id);
        }
    }
}
