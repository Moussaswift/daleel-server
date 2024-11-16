using Microsoft.AspNetCore.Mvc;
using daleel.Entities;
using daleel.Services;
using Microsoft.EntityFrameworkCore;
using daleel.Models;

namespace daleel.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomersController : ControllerBase
    {
        private readonly ICustomerService _customerService;

        public CustomersController(ICustomerService customerService)
        {
            _customerService = customerService;
        }

        // GET: api/Customers
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Customer>>> GetCustomers()
        {
            var customers = await _customerService.GetAllCustomersAsync();
            return Ok(customers);
        }

        // GET: api/Customers/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Customer>> GetCustomer(Guid id)
        {
            var customer = await _customerService.GetCustomerAsync(id);

            if (customer == null)
            {
                return NotFound();
            }

            return customer;
        }

        // POST: api/Customers
        [HttpPost]
        public async Task<ActionResult<Customer>> PostCustomer(CustomerCreateModel model)
        {
            var customer = new Customer
            {
                Id = Guid.NewGuid(),
                FullName = model.FullName,
                Company = model.Company,
                Type = model.Type,
                PhotoURL = model.PhotoURL
            };

            if (model.ContactInfo != null)
            {
                customer.ContactInfo = new ContactInfo
                {
                    EmailAddress = model.ContactInfo.EmailAddress,
                    HomePhone = model.ContactInfo.HomePhone,
                    WorkPhone = model.ContactInfo.WorkPhone,
                    CustomerId = customer.Id
                };
            }

            if (model.AddressInfo != null)
            {
                customer.AddressInfo = new AddressInfo
                {
                    StreetAddress = model.AddressInfo.StreetAddress,
                    AptNumber = model.AddressInfo.AptNumber,
                    City = model.AddressInfo.City,
                    State = model.AddressInfo.State,
                    ZipCode = model.AddressInfo.ZipCode,
                    Country = model.AddressInfo.Country,
                    CustomerId = customer.Id
                };
            }

            var createdCustomer = await _customerService.CreateCustomerAsync(customer);
            return CreatedAtAction(nameof(GetCustomer), new { id = createdCustomer.Id }, createdCustomer);
        }

        // PUT: api/Customers/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCustomer(Guid id, CustomerUpdateModel model)
        {
            var existingCustomer = await _customerService.GetCustomerAsync(id);
            if (existingCustomer == null)
            {
                return NotFound();
            }

            existingCustomer.ApplyUpdate(model);

            try
            {
                await _customerService.UpdateCustomerAsync(existingCustomer);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await CustomerExistsAsync(id))
                {
                    return NotFound();
                }
                throw;
            }

            return NoContent();
        }

        // DELETE: api/Customers/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCustomer(Guid id)
        {
            var customer = await _customerService.GetCustomerAsync(id);
            if (customer == null)
            {
                return NotFound();
            }

            await _customerService.DeleteCustomerAsync(id);

            return NoContent();
        }

        private async Task<bool> CustomerExistsAsync(Guid id)
        {
            var customer = await _customerService.GetCustomerAsync(id);
            return customer != null;
        }
    }
}
