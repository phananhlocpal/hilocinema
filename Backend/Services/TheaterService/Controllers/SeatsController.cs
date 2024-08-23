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

        // GET: api/Seats/GetSeatsByRoom/{roomId}
        [HttpGet("GetSeatsByRoom/{roomId}")]
        public async Task<ActionResult<IEnumerable<SeatReadDto>>> GetSeatsByRoom(int roomId)
        {
            var seats = await _context.Seats.Where(s => s.RoomId == roomId).ToListAsync();

            if (seats == null || seats.Count == 0)
            {
                return NotFound("This room does not have seats, please add more seats");
            }

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
            var seat = await _context.Seats.FindAsync(id);

            if (seat == null)
            {
                return NotFound();
            }

            // Cập nhật các thuộc tính của ghế từ DTO
            seat.ColSeat = seatDto.ColSeat;
            seat.RowSeat = seatDto.RowSeat;
            seat.Name = seatDto.Name;
            seat.Type = seatDto.Type;
            seat.Status = seatDto.Status;

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
        public async Task<ActionResult<IEnumerable<SeatReadDto>>> PostSeats(IEnumerable<SeatCreateDto> seatDtos)
        {
            var seats = _mapper.Map<IEnumerable<Seat>>(seatDtos);
            _context.Seats.AddRange(seats);
            await _context.SaveChangesAsync();

            var seatReadDtos = _mapper.Map<IEnumerable<SeatReadDto>>(seats);

            return CreatedAtAction("GetSeats", seatReadDtos);
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

            // Ví dụ: không xóa ghế nếu trạng thái là 'in-use'
            if (seat.Status == "Active")
            {
                return BadRequest("Seat is currently in use and cannot be deleted.");
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

