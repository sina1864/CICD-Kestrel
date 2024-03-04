using CICD_Core.Models;
using CICD_Core.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace CICD_API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class HomeController : ControllerBase
{
    private readonly ICustomerRepository _customerRepository;
    public HomeController(ICustomerRepository customerRepository)
    {
        _customerRepository = customerRepository;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Customer>>> GetAllCustomers()
    {
        var customers = await _customerRepository.GetAllCustomers();
        return Ok(customers);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Customer>> GetCustomerById(int id)
    {
        var customer = await _customerRepository.GetCustomerById(id);
        if (customer == null)
        {
            return NotFound();
        }
        return Ok(customer);
    }

    [HttpPost]
    public async Task<IActionResult> CreateACustomer(Customer customer)
    {
        await _customerRepository.CreateCustomer(customer);
        return CreatedAtAction(nameof(GetCustomerById), new { id = customer.Id }, customer);
    }

    [HttpPut]
    public async Task<ActionResult> UpdateCustomer([Bind(include: "CustomerId, Name, Age, Salary")] Customer customer)
    {
        var customerFromDb = await _customerRepository.GetCustomerById(customer.CustomerId);
        if (customerFromDb == null)
        {
            return NotFound();
        }
        customer.Id = customerFromDb.Id;
        await _customerRepository.UpdateCustomer(customer);
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteCustomer(int id)
    {
        var customerFromDb = await _customerRepository.GetCustomerById(id);
        if (customerFromDb == null)
        {
            return NotFound();
        }
        await _customerRepository.DeleteCustomer(id);
        return NoContent();
    }
}
