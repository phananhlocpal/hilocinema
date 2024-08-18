using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using ScheduleService.Repositories.ScheduleRepository;
using ScheduleService.Dtos;
using ScheduleService.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ScheduleService.OtherModels;
using ScheduleService.Service.HttpServices;

namespace ScheduleService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ScheduleController : ControllerBase
    {
        private readonly IScheduleRepo _repository;
        private readonly IMapper _mapper;
        private readonly MovieHttpService _movieHttpService;
        private readonly TheaterHttpService _theaterHttpService;
        private readonly InvoiceHttpService _invoiceHttpService;
        private readonly ILogger<ScheduleController> _logger;

        public ScheduleController(
            IScheduleRepo repository,
            IMapper mapper,
            MovieHttpService movieHttpService,
            TheaterHttpService theaterHttpService,
            InvoiceHttpService invoiceHttpService,
            ILogger<ScheduleController> logger)
        {
            _repository = repository;
            _mapper = mapper;
            _movieHttpService = movieHttpService;
            _theaterHttpService = theaterHttpService;
            _invoiceHttpService = invoiceHttpService;
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ScheduleReadDto>>> GetAllSchedules()
        {
            var schedules = await _repository.GetAllScheduleAsync();

            var scheduleReadDtos = new List<ScheduleReadDto>();

            foreach (var schedule in schedules)
            {
                var movie = await _movieHttpService.GetMovieById(schedule.MovieId);
                Seat seat = await _theaterHttpService.GetSeatById(schedule.SeatId);
                Room room = (seat != null) ? await _theaterHttpService.GetRoomById(seat.RoomId) : null;
                Theater theater = (room != null) ? await _theaterHttpService.GetTheaterById(room.TheaterId) : null;

                var invoiceId = schedule.InvoiceId.GetValueOrDefault();
                var invoice = (schedule.InvoiceId.HasValue) ? await _invoiceHttpService.GetInvoiceById(invoiceId) : null;

                room.Theater = theater;
                seat.Room = room;

                var scheduleReadDto = new ScheduleReadDto
                {
                    Date = schedule.Date,
                    Time = schedule.Time,
                    InvoiceId = schedule.InvoiceId,
                    Movie = new Movie
                    {
                        Id = movie.Id,
                        Title = movie.Title,
                        MovieUrl = movie.MovieUrl,
                    },
                    Seat = seat,
                    Invoice = invoice,
                };

                scheduleReadDtos.Add(scheduleReadDto);
            }

            return Ok(scheduleReadDtos);
        }

        [HttpGet("getSeatsBySchedule")]
        public async Task<ActionResult<IEnumerable<object>>> GetSeatsBySchedude( int movieId, DateOnly date, int theaterId, int roomId, TimeOnly time)
        {
            var schedules = await _repository.GetSeatsBySchedude(movieId, date, theaterId, roomId, time);
            var room = await _theaterHttpService.GetRoomById(roomId);
            var theater = await _theaterHttpService.GetTheaterById(theaterId);


            var seats = new List<object>();

            foreach (var schedule in schedules)
            {
                var seat = await _theaterHttpService.GetSeatById(schedule.SeatId);
                if (seat.RoomId == roomId)
                {
                    seats.Add(seat);
                }

            }

            object result = new
            {
                Room = room,
                Theater = theater,
                Schedules = schedules,
                Seats = seats,
            };

            return Ok(result);

        }

        [HttpGet("GetOnlyScheduleWithoutSeats")]
        public async Task<ActionResult<IEnumerable<ScheduleReadDto>>> GetOnlyScheduleWithoutSeats()
        {
            var schedules = await _repository.GetAllScheduleAsync();

            var scheduleReadDtos = new List<ScheduleReadDto>();

            foreach (var schedule in schedules)
            {
                var movie = await _movieHttpService.GetMovieById(schedule.MovieId);

                var scheduleReadDto = new ScheduleReadDto
                {
                    Date = schedule.Date,
                    Time = schedule.Time,
                    InvoiceId = schedule.InvoiceId,
                    Movie = new Movie
                    {
                        Id = movie.Id,
                        Title = movie.Title,
                        MovieUrl = movie.MovieUrl,
                    },
                };

                scheduleReadDtos.Add(scheduleReadDto);
            }

            return Ok(scheduleReadDtos);
        }

        [HttpGet("movieUrl/{url}")]
        public async Task<ActionResult<IEnumerable<ScheduleReadDto>>> GetSchedulesByMovieUrl(string url)
        {
            var movie = await _movieHttpService.GetMovieByUrl(url);
            if (movie == null)
            {
                return NotFound();
            }

            var schedules = await _repository.GetSchedulesByMovieIdAsync(movie.Id);

            var scheduleReadDtos = new List<object>();

            foreach (var schedule in schedules)
            {
                var seat = await _theaterHttpService.GetSeatById(schedule.SeatId);
                var room = (seat != null) ? await _theaterHttpService.GetRoomById(seat.RoomId) : null;
                var theater = (room != null) ? await _theaterHttpService.GetTheaterById(room.TheaterId) : null;

                var invoiceId = schedule.InvoiceId.GetValueOrDefault();
                var invoice = (schedule.InvoiceId.HasValue) ? await _invoiceHttpService.GetInvoiceById(invoiceId) : null;

                room.Theater = theater;
                seat.Room = room;

                var scheduleReadDto = new 
                {
                    Date = schedule.Date,
                    Time = schedule.Time,
                    Seat = seat,
                    Invoice = invoice,
                };

                scheduleReadDtos.Add(scheduleReadDto);
            }

            object result = new
            {
                Movie = new 
                {
                    Id = movie.Id,
                    Title = movie.Title,
                    MovieUrl = movie.MovieUrl,
                },
                schedules = scheduleReadDtos
            };

            return Ok(result);
        }

        [HttpGet("GetScheduleByInvoiceId/{invoiceId}")]
        public async Task<ActionResult<IEnumerable<Schedule>>> GetScheduleByInvoiceId(int invoiceId)
        {
            var schedules = await _repository.GetScheduleByInvoiceIdAsync(invoiceId);

            return Ok(schedules);
        }

        // POST: api/schedule
        [HttpPost]
        public async Task<ActionResult<ScheduleCreateDto>> CreateSchedule(ScheduleCreateDto scheduleCreateDto)
        {
            try
            {
                var schedules = new List<Schedule>();
                foreach (var seatId in scheduleCreateDto.SeatIds)
                {
                    var schedule = new Schedule
                    {
                        MovieId = scheduleCreateDto.MovieId,
                        Date = scheduleCreateDto.Date,
                        Time = scheduleCreateDto.Time,
                        SeatId = seatId
                    };
                    schedules.Add(schedule);
                }

                var createdSchedules = new List<Schedule>();
                foreach (var schedule in schedules)
                {
                    var createdSchedule = await _repository.CreateScheduleAsync(schedule);
                    createdSchedules.Add(createdSchedule);
                }

                var scheduleCreateDtoResponse = new ScheduleCreateDto
                {
                    MovieId = scheduleCreateDto.MovieId,
                    Date = scheduleCreateDto.Date,
                    Time = scheduleCreateDto.Time,
                    SeatIds = createdSchedules.Select(s => s.SeatId).ToList()
                };

                return CreatedAtAction(nameof(GetAllSchedules), new { id = createdSchedules.First().MovieId }, scheduleCreateDtoResponse);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while creating the schedule.");
                return StatusCode(500, "Internal server error");
            }
        }

    }

}
