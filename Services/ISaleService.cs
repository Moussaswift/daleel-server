using daleel.Entities;

namespace daleel.Services
{
    public interface ISaleService
    {
        Task<IEnumerable<Sale>> GetAllSalesAsync();
        Task<Sale> GetSaleAsync(Guid id);
        Task<Sale> CreateSaleAsync(Sale sale);
        Task UpdateSaleAsync(Sale sale);
        Task DeleteSaleAsync(Guid id);
    }
}
