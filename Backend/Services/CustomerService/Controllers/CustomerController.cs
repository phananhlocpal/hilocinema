﻿using AutoMapper;
using CustomerService.Data;
using CustomerService.Dtos;
using CustomerService.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.IO;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using CustomerService.Services;

namespace CustomerService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CustomerController : ControllerBase
    {
        private readonly ICustomerRepository _repository;
        private readonly IMapper _mapper;
        private readonly CustomerPublisher _customerPublisher;
        private readonly ILogger<CustomerController> _logger;

        public CustomerController(
            ICustomerRepository repository,
            IMapper mapper,
            CustomerPublisher customerPublisher,
            ILogger<CustomerController> logger)
        {
            _repository = repository;
            _mapper = mapper;
            _customerPublisher = customerPublisher;
            _logger = logger;
        }

        [HttpGet]
        [Authorize(Policy = "AdminEmployeeOnly")]
        public async Task<ActionResult<IEnumerable<CustomerReadDTO>>> GetAllCustomers()
        {
            try
            {
                var customers = await _repository.GetAllCustomersAsync();
                if (!customers.Any())
                {
                    return NotFound("Customer list is empty");
                }
                return Ok(_mapper.Map<IEnumerable<CustomerReadDTO>>(customers));
            }
            catch (IOException e)
            {
                _logger.LogError(e, "An error occurred while getting all customers");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("count")]
        public async Task<IActionResult> GetAllCount()
        {
            try
            {
                var count = await _repository.GetAllCount();
                return Ok(count);
            }
            catch (IOException e)
            {
                _logger.LogError(e, "An error occurred while getting the customer count");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("{id}")]
        [Authorize(Policy = "AdminEmployeeOnly")]
        public async Task<ActionResult<CustomerReadDTO>> GetCustomerById(int id)
        {
            try
            {
                var customer = await _repository.GetCustomerByIdAsync(id);
                if (customer == null)
                {
                    return NotFound($"Customer with ID {id} not found");
                }
                return Ok(_mapper.Map<CustomerReadDTO>(customer));
            }
            catch (IOException e)
            {
                _logger.LogError(e, "An error occurred while getting the customer by ID");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("email/{email}")]
        [Authorize(Policy = "AdminEmployeeOnly")]
        public async Task<ActionResult<CustomerReadDTO>> GetCustomerByEmail(string email)
        {
            try
            {
                var customer = await _repository.GetCustomerByEmailAsync(email);
                if (customer == null)
                {
                    return NotFound($"Customer with email {email} not found");
                }
                return Ok(_mapper.Map<CustomerReadDTO>(customer));
            }
            catch (IOException e)
            {
                _logger.LogError(e, "An error occurred while getting the customer by email");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("phone/{phone}")]
        [Authorize(Policy = "AdminEmployeeOnly")]
        public async Task<ActionResult<CustomerReadDTO>> GetCustomerByPhone(string phone)
        {
            try
            {
                var customer = await _repository.GetCustomerPhoneAsync(phone);
                if (customer == null)
                {
                    return NotFound($"Customer with phone number {phone} not found");
                }
                return Ok(_mapper.Map<CustomerReadDTO>(customer));
            }
            catch (IOException e)
            {
                _logger.LogError(e, "An error occurred while getting the customer by phone");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPost]
        [Authorize(Policy = "AdminEmployeeOnly")]
        public async Task<ActionResult> CreateCustomer(CustomerCreateDTO customerCreateDto)
        {
            try
            {
                var customerModel = _mapper.Map<Customer>(customerCreateDto);
                await _repository.CreateCustomerAsync(customerModel);
                await _repository.SaveChangeAsync();

                _customerPublisher.CreateCustomerPubSub(customerModel);

                var customerReadDto = _mapper.Map<CustomerReadDTO>(customerModel);

                return CreatedAtAction(nameof(GetCustomerById), new { id = customerReadDto.Id }, customerReadDto);
            }
            catch (IOException e)
            {
                _logger.LogError(e, "An error occurred while creating the customer");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPut("{customerId}")]
        [Authorize(Policy = "AllCanAccess")]
        public async Task<ActionResult> UpdateCustomer(int customerId, [FromBody] Customer customer)
        {
            if (customer.Id == null)
            {
                return BadRequest("Invalid customer data");
            }

            try
            {
                await _repository.UpdateCustomerAsync(customer);
                await _repository.SaveChangeAsync();
                return NoContent();
            }
            catch (KeyNotFoundException)
            {
                return NotFound("Customer not found");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while updating the customer");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPut("{customerId}/disable")]
        [Authorize(Policy = "AllCanAccess")]
        public async Task<ActionResult> DisableCustomer(int customerId)
        {
            if (customerId == null)
            {
                return BadRequest("Invalid customer data");
            }

            try
            {
                await _repository.DisableCustomerAsync(customerId);
                await _repository.SaveChangeAsync();
                return NoContent();
            }
            catch (KeyNotFoundException)
            {
                return NotFound("Customer not found");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while updating the customer");
                return StatusCode(500, "Internal server error");
            }
        }
    }
}
