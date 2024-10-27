using daleel.Entities;

namespace daleel.Services
{
    public interface ICustomerService
    {
        Task<IEnumerable<Customer>> GetAllCustomersAsync();
        Task<Customer> GetCustomerAsync(Guid id);
        Task<Customer> CreateCustomerAsync(Customer customer);
        Task UpdateCustomerAsync(Customer customer);
        Task DeleteCustomerAsync(Guid id);
    }
}
