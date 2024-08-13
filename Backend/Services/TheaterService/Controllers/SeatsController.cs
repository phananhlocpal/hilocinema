using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TheaterService.Dtos;
using TheaterService.Models;

namespace TheaterService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SeatsController : ControllerBase
    {
        private readonly TheaterContext _context;
        private readonly IMapper _mapper;

        public SeatsController(TheaterContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // GET: api/Seats
        [HttpGet]
        public async Task<ActionResult<IEnumerable<SeatReadDto>>> GetSeats()
        {
            var seats = await _context.Seats.ToListAsync();
            return Ok(_mapper.Map<IEnumerable<SeatReadDto>>(seats));
        }

        // GET: api/Seats/5
        [HttpGet("{id}")]
        public async Task<ActionResult<SeatReadDto>> GetSeat(int id)
        {
            var seat = await _context.Seats.FindAsync(id);

            if (seat == null)
            {
                return NotFound();
            }

            return Ok(_mapper.Map<SeatReadDto>(seat));
        }

        // PUT: api/Seats/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutSeat(int id, SeatCreateDto seatDto)
        {

            var seat = _mapper.Map<Seat>(seatDto);
            _context.Entry(seat).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SeatExists(id))
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

        // POST: api/Seats
        [HttpPost]
        public async Task<ActionResult<SeatReadDto>> PostSeat(SeatCreateDto seatDto)
        {
            var seat = _mapper.Map<Seat>(seatDto);
            _context.Seats.Add(seat);
            await _context.SaveChangesAsync();

            var seatReadDto = _mapper.Map<SeatReadDto>(seat);

            return CreatedAtAction("GetSeat", new { id = seatReadDto.Id }, seatReadDto);
        }

        // DELETE: api/Seats/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSeat(int id)
        {
            var seat = await _context.Seats.FindAsync(id);
            if (seat == null)
            {
                return NotFound();
            }

            _context.Seats.Remove(seat);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool SeatExists(int id)
        {
            return _context.Seats.Any(e => e.Id == id);
        }
    }
}
