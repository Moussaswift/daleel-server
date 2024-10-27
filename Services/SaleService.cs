using Microsoft.EntityFrameworkCore;
using daleel.Data;
using daleel.Entities;

namespace daleel.Services
{
    public class SaleService : ISaleService
    {
        private readonly ApplicationDbContext _context;

        public SaleService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Sale>> GetAllSalesAsync()
        {
            return await _context.Sales
                .Include(s => s.Customer)
                .Include(s => s.Source)
                .Include(s => s.Item)
                .Include(s => s.Status)
                .ToListAsync();
        }

        public async Task<Sale> GetSaleAsync(Guid id)
        {
            return await _context.Sales
                .Include(s => s.Customer)
                .Include(s => s.Source)
                .Include(s => s.Item)
                .Include(s => s.Status)
                .FirstOrDefaultAsync(s => s.Id == id);
        }

        public async Task<Sale> CreateSaleAsync(Sale sale)
        {
            _context.Sales.Add(sale);
            await _context.SaveChangesAsync();
            return sale;
        }

        public async Task UpdateSaleAsync(Sale sale)
        {
            _context.Entry(sale).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task DeleteSaleAsync(Guid id)
        {
            var sale = await _context.Sales.FindAsync(id);
            if (sale != null)
            {
                _context.Sales.Remove(sale);
                await _context.SaveChangesAsync();
            }
        }
    }
}
