using Microsoft.AspNetCore.Mvc;
using daleel.Entities;
using daleel.Services;
using Microsoft.EntityFrameworkCore;
using daleel.Models;
using daleel.DTOs;

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
            // GET: api/Customers
        [HttpGet]
        public async Task<ActionResult<PaginatedResponseDto<Customer>>> GetCustomers([FromQuery] PaginationDto pagination)
        {
            try
            {
                var paginatedCustomers = await _customerService.GetAllCustomersAsync(pagination);
                return Ok(paginatedCustomers);
            }
            catch (Exception)
            {
                return StatusCode(500, "An error occurred while retrieving customers");
            }
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

        // GET: api/Customers/{id}/notes
        [HttpGet("{id}/notes")]
        public async Task<ActionResult<PaginatedResponseDto<Note>>> GetCustomerNotes(Guid id, [FromQuery] PaginationDto pagination)
        {
            try
            {
                var notes = await _customerService.GetCustomerNotesAsync(id, pagination);
                return Ok(notes);
            }
            catch (Exception)
            {
                return StatusCode(500, "An error occurred while retrieving customer notes");
            }
        }

        // GET: api/Customers/{id}/leads
        [HttpGet("{id}/leads")]
        public async Task<ActionResult<PaginatedResponseDto<Lead>>> GetCustomerLeads(Guid id, [FromQuery] PaginationDto pagination)
        {
            try
            {
                var leads = await _customerService.GetCustomerLeadsAsync(id, pagination);
                return Ok(leads);
            }
            catch (Exception)
            {
                return StatusCode(500, "An error occurred while retrieving customer leads");
            }
        }

        // GET: api/Customers/{id}/sales
        [HttpGet("{id}/sales")]
        public async Task<ActionResult<PaginatedResponseDto<Sale>>> GetCustomerSales(Guid id, [FromQuery] PaginationDto pagination)
        {
            try
            {
                var sales = await _customerService.GetCustomerSalesAsync(id, pagination);
                return Ok(sales);
            }
            catch (Exception)
            {
                return StatusCode(500, "An error occurred while retrieving customer sales");
            }
        }
    }
}
