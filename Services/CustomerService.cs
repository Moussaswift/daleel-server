using Microsoft.EntityFrameworkCore;
using daleel.Data;
using daleel.DTOs;
using daleel.Entities;

namespace daleel.Services
{
    public class CustomerService : ICustomerService
    {
        private readonly ApplicationDbContext _context;

        public CustomerService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<PaginatedResponseDto<Customer>> GetAllCustomersAsync(PaginationDto pagination)
        {
            var query = _context.Customers
                .Include(c => c.ContactInfo)
                .Include(c => c.AddressInfo);

            var totalCount = await query.CountAsync();
            var totalPages = (int)Math.Ceiling(totalCount / (double)pagination.PageSize);

            var items = await query
                .Skip((pagination.PageNumber - 1) * pagination.PageSize)
                .Take(pagination.PageSize)
                .ToListAsync();

            return new PaginatedResponseDto<Customer>
            {
                Items = items,
                PageNumber = pagination.PageNumber,
                PageSize = pagination.PageSize,
                TotalPages = totalPages,
                TotalCount = totalCount
            };
        }

        public async Task<Customer> GetCustomerAsync(Guid id)
        {
            return await _context.Customers
                .Include(c => c.ContactInfo)
                .Include(c => c.AddressInfo)
                .FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task<Customer> CreateCustomerAsync(Customer customer)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                // Add the customer and related entities
                await _context.Customers.AddAsync(customer);
                
                if (customer.ContactInfo != null)
                {
                    await _context.ContactInfos.AddAsync(customer.ContactInfo);
                }
                
                if (customer.AddressInfo != null)
                {
                    await _context.AddressInfos.AddAsync(customer.AddressInfo);
                }

                await _context.SaveChangesAsync();
                await transaction.CommitAsync();
                
                return customer;
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }

        public async Task UpdateCustomerAsync(Customer customer)
        {
            _context.Entry(customer).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task DeleteCustomerAsync(Guid id)
        {
            var customer = await _context.Customers.FindAsync(id);
            if (customer != null)
            {
                _context.Customers.Remove(customer);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<PaginatedResponseDto<Note>> GetCustomerNotesAsync(Guid customerId, PaginationDto pagination)
        {
            var query = _context.Notes.Where(n => n.Lead.CustomerId == customerId);

            var totalCount = await query.CountAsync();
            var totalPages = (int)Math.Ceiling(totalCount / (double)pagination.PageSize);

            var items = await query
                .Skip((pagination.PageNumber - 1) * pagination.PageSize)
                .Take(pagination.PageSize)
                .ToListAsync();

            return new PaginatedResponseDto<Note>
            {
                Items = items,
                PageNumber = pagination.PageNumber,
                PageSize = pagination.PageSize,
                TotalPages = totalPages,
                TotalCount = totalCount
            };
        }

        public async Task<PaginatedResponseDto<Lead>> GetCustomerLeadsAsync(Guid customerId, PaginationDto pagination)
        {
            var query = _context.Leads.Where(l => l.CustomerId == customerId);

            var totalCount = await query.CountAsync();
            var totalPages = (int)Math.Ceiling(totalCount / (double)pagination.PageSize);

            var items = await query
                .Skip((pagination.PageNumber - 1) * pagination.PageSize)
                .Take(pagination.PageSize)
                .ToListAsync();

            return new PaginatedResponseDto<Lead>
            {
                Items = items,
                PageNumber = pagination.PageNumber,
                PageSize = pagination.PageSize,
                TotalPages = totalPages,
                TotalCount = totalCount
            };
        }

        public async Task<PaginatedResponseDto<Sale>> GetCustomerSalesAsync(Guid customerId, PaginationDto pagination)
        {
            var query = _context.Sales.Where(s => s.CustomerId == customerId);

            var totalCount = await query.CountAsync();
            var totalPages = (int)Math.Ceiling(totalCount / (double)pagination.PageSize);

            var items = await query
                .Skip((pagination.PageNumber - 1) * pagination.PageSize)
                .Take(pagination.PageSize)
                .ToListAsync();

            return new PaginatedResponseDto<Sale>
            {
                Items = items,
                PageNumber = pagination.PageNumber,
                PageSize = pagination.PageSize,
                TotalPages = totalPages,
                TotalCount = totalCount
            };
        }
    }
}
