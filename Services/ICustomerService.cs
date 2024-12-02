using daleel.Entities;
using daleel.Models;
using daleel.DTOs;

namespace daleel.Services
{
    public interface ICustomerService
    {
        Task<PaginatedResponseDto<Customer>> GetAllCustomersAsync(PaginationDto pagination);
        Task<Customer> GetCustomerAsync(Guid id);
        Task<Customer> CreateCustomerAsync(Customer customer);
        Task UpdateCustomerAsync(Customer customer);
        Task DeleteCustomerAsync(Guid id);
        Task<PaginatedResponseDto<Note>> GetCustomerNotesAsync(Guid customerId, PaginationDto pagination);
        Task<PaginatedResponseDto<Lead>> GetCustomerLeadsAsync(Guid customerId, PaginationDto pagination);
        Task<PaginatedResponseDto<Sale>> GetCustomerSalesAsync(Guid customerId, PaginationDto pagination);
    }
}
