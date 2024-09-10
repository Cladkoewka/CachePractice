using CachePractice.API.Contracts;
using CachePractice.Domain.Models;
using CachePractice.Logic.Services;
using Microsoft.AspNetCore.Mvc;

namespace CachePractice.API.Controllers;


[ApiController]
[Route("api/[controller]")]
public class CustomersController : ControllerBase
{
    private readonly CustomerService _customerService;

    public CustomersController(CustomerService customerService)
    {
        _customerService = customerService;
    }

    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<Customer>>> GetAllCustomers()
    {
        var customers = await _customerService.GetAllCustomersAsync();
        if (customers == null || customers.Count == 0)
        {
            return NotFound("No customers found.");
        }
        return Ok(customers);
    }
    
    [HttpGet("{id:int}")]
    public async Task<ActionResult<Customer>> GetCustomerById(int id)
    {
        var customer = await _customerService.GetCustomerByIdAsync(id);
        if (customer == null)
        {
            return NotFound($"Customer with ID {id} not found.");
        }
        return Ok(customer);
    }
    
    [HttpPost]
    public async Task<ActionResult> AddCustomer([FromBody] AddCustomerRequest request)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        
        var customer = new Customer
        {
            Email = request.Email,
            Name = request.Name
        };

        await _customerService.AddCustomerAsync(customer);
        return CreatedAtAction(nameof(GetCustomerById), new { id = customer.Id }, customer);
    }
    
    [HttpPut("{id:int}")]
    public async Task<ActionResult> UpdateCustomer(int id, [FromBody] UpdateCustomerRequest request)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        
        var customer = new Customer
        {
            Email = request.Email,
            Name = request.Name
        };

        var result = await _customerService.UpdateCustomerAsync(id, customer);
        if (!result)
        {
            return NotFound($"Customer with ID {id} not found.");
        }

        return NoContent();
    }
    
    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteCustomer(int id)
    {
        var result = await _customerService.DeleteCustomerAsync(id);
        if (!result)
        {
            return NotFound($"Customer with ID {id} not found.");
        }
        
        return NoContent(); // 204 No Content
    }
}