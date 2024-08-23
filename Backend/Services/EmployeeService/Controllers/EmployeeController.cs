using AutoMapper;
using EmployeeService.Dtos;
using EmployeeService.Models;
using EmployeeService.Repositories;
using EmployeeService.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using EmployeeService.Helper;

namespace EmployeeService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EmployeeController : ControllerBase
    {
        private readonly IEmployeeRepo _employeeRepo;
        private readonly IMapper _mapper;
        private readonly ILogger<EmployeeController> _logger;
        private readonly EmailService _emailService;
        public EmployeeController(IEmployeeRepo employeeRepo, IMapper mapper, ILogger<EmployeeController> logger, EmailService emailService)
        {
            _employeeRepo = employeeRepo;
            _mapper = mapper;
            _logger = logger;
            _emailService = emailService;
        }

        [HttpGet]
        [Authorize(Policy = "AdminEmployeeOnly")]
        public async Task<ActionResult<IEnumerable<EmployeeReadDto>>> GetAllEmployees()
        {
            // Log the user's roles and claims
            var userRoles = User.Claims.Where(c => c.Type == "role").Select(c => c.Value);
            _logger.LogInformation("User Roles: " + string.Join(",", userRoles));

            try
            {
                var employees = await _employeeRepo.GetAllAsync();
                var employeeDtos = _mapper.Map<IEnumerable<EmployeeReadDto>>(employees);
                return Ok(employeeDtos);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving employees.");
                return StatusCode(500, "Internal server error.");
            }
        }

        // Retrieve employee by ID
        [HttpGet("{id}", Name = "GetEmployeeById")]
        public async Task<ActionResult<EmployeeReadDto>> GetEmployeeById(int id)
        {
            try
            {
                var employee = await _employeeRepo.GetByIdAsync(id);
                if (employee == null)
                {
                    return NotFound();
                }

                var employeeDto = _mapper.Map<EmployeeReadDto>(employee);
                return Ok(employeeDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error retrieving employee with Id {id}.");
                return StatusCode(500, "Internal server error.");
            }
        }

        // Create new employee
        [HttpPost]
        [Authorize(Policy = "AdminOnly")]
        public async Task<ActionResult<EmployeeReadDto>> CreateEmployee(EmployeeCreateDto employeeCreateDto)
        {
            try
            {
                // Check if email already exists
                if (await _employeeRepo.EmailExistsAsync(employeeCreateDto.Email))
                {
                    return BadRequest("Email already exists.");
                }

                // Hash the password
                employeeCreateDto.Password = PasswordHasher.HashPassword(employeeCreateDto.Password);

                var employee = _mapper.Map<Employee>(employeeCreateDto);
                await _employeeRepo.CreateAsync(employee);
                var isSaved = await _employeeRepo.SaveChangesAsync();

                if (!isSaved)
                {
                    return StatusCode(500, "Error saving employee to database.");
                }

                var employeeReadDto = _mapper.Map<EmployeeReadDto>(employee);
                return CreatedAtRoute(nameof(GetEmployeeById), new { id = employeeReadDto.Id }, employeeReadDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating employee.");
                return StatusCode(500, "Internal server error.");
            }
        }

        // Update employee details
        [HttpPut("{id}")]
        [Authorize(Policy = "AdminOnly")]
        public async Task<IActionResult> UpdateEmployee(int id, EmployeeCreateDto employeeUpdateDto)
        {
            try
            {
                var existingEmployee = await _employeeRepo.GetByIdAsync(id);
                if (existingEmployee == null)
                {
                    return NotFound();
                }

                // Update password if provided
                if (!string.IsNullOrEmpty(employeeUpdateDto.Password))
                {
                    existingEmployee.Password = PasswordHasher.HashPassword(employeeUpdateDto.Password);
                }

                // Update other fields
                _mapper.Map(employeeUpdateDto, existingEmployee);
                await _employeeRepo.UpdateAsync(existingEmployee);
                var isSaved = await _employeeRepo.SaveChangesAsync();

                if (!isSaved)
                {
                    return StatusCode(500, "Error updating employee in database.");
                }

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error updating employee with Id {id}.");
                return StatusCode(500, "Internal server error.");
            }
        }

        // Hide employee (soft delete)
        [HttpPatch("{id}/hide")]
        [Authorize(Policy = "AdminOnly")]
        public async Task<IActionResult> HideEmployee(int id)
        {
            try
            {
                var employee = await _employeeRepo.GetByIdAsync(id);
                if (employee == null)
                {
                    return NotFound();
                }

                await _employeeRepo.HideEmployeeAsync(id);
                var isSaved = await _employeeRepo.SaveChangesAsync();

                if (!isSaved)
                {
                    return StatusCode(500, "Error hiding employee in database.");
                }

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error hiding employee with Id {id}.");
                return StatusCode(500, "Internal server error.");
            }
        }
    }
}
