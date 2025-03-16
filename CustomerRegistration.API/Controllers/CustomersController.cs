using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using CustomerRegistration.API.Core.Application.DTOs;
using CustomerRegistration.API.Core.Domain.Entities;
using CustomerRegistration.API.Core.Domain.Interfaces;

namespace CustomerRegistration.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CustomersController : ControllerBase
{
    private readonly ICustomerRepository _customerRepository;
    private readonly IMapper _mapper;

    public CustomersController(ICustomerRepository customerRepository, IMapper mapper)
    {
        _customerRepository = customerRepository;
        _mapper = mapper;
    }

    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<CustomerDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<CustomerDto>>> GetAll()
    {
        var customers = await _customerRepository.GetAllAsync();
        return Ok(_mapper.Map<IEnumerable<CustomerDto>>(customers));
    }

    [HttpGet("{id}")]
    [ProducesResponseType(typeof(CustomerDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<CustomerDto>> GetById(int id)
    {
        var customer = await _customerRepository.GetByIdAsync(id);
        if (customer == null)
        {
            return NotFound(new { message = "Customer not found" });
        }

        return Ok(_mapper.Map<CustomerDto>(customer));
    }

    [HttpPost]
    [ProducesResponseType(typeof(CustomerDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<CustomerDto>> Create([FromBody] CreateCustomerDto createCustomerDto)
    {
        // Validate unique constraints
        if (await _customerRepository.EmailExistsAsync(createCustomerDto.Email))
        {
            return BadRequest(new { message = "Email already exists" });
        }

        if (createCustomerDto.IdentityNumber != null && 
            await _customerRepository.IdentityNumberExistsAsync(createCustomerDto.IdentityNumber))
        {
            return BadRequest(new { message = "Identity number already exists" });
        }

        var customer = _mapper.Map<Customer>(createCustomerDto);
        customer.CreatedAt = DateTime.UtcNow;
        customer.IsActive = true;

        var createdCustomer = await _customerRepository.CreateAsync(customer);
        var customerDto = _mapper.Map<CustomerDto>(createdCustomer);

        return CreatedAtAction(nameof(GetById), new { id = customerDto.Id }, customerDto);
    }

    [HttpPut("{id}")]
    [ProducesResponseType(typeof(CustomerDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<CustomerDto>> Update(int id, [FromBody] UpdateCustomerDto updateCustomerDto)
    {
        var customer = await _customerRepository.GetByIdAsync(id);
        if (customer == null)
        {
            return NotFound(new { message = "Customer not found" });
        }

        // Validate unique constraints
        if (updateCustomerDto.Email != null && 
            updateCustomerDto.Email != customer.Email && 
            await _customerRepository.EmailExistsAsync(updateCustomerDto.Email, id))
        {
            return BadRequest(new { message = "Email already exists" });
        }

        if (updateCustomerDto.IdentityNumber != null && 
            updateCustomerDto.IdentityNumber != customer.IdentityNumber && 
            await _customerRepository.IdentityNumberExistsAsync(updateCustomerDto.IdentityNumber, id))
        {
            return BadRequest(new { message = "Identity number already exists" });
        }

        _mapper.Map(updateCustomerDto, customer);
        customer.UpdatedAt = DateTime.UtcNow;

        var updatedCustomer = await _customerRepository.UpdateAsync(customer);
        return Ok(_mapper.Map<CustomerDto>(updatedCustomer));
    }

    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(int id)
    {
        var customer = await _customerRepository.GetByIdAsync(id);
        if (customer == null)
        {
            return NotFound(new { message = "Customer not found" });
        }

        await _customerRepository.DeleteAsync(id);
        return NoContent();
    }
} 