using Microsoft.EntityFrameworkCore;
using daleel.Data;
using daleel.Entities;

namespace daleel.Services
{
    public class ItemService : IItemService
    {
        private readonly ApplicationDbContext _context;

        public ItemService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Item>> GetAllItemsAsync()
        {
            return await _context.Items.ToListAsync();
        }

        public async Task<Item> GetItemAsync(Guid id)
        {
            return await _context.Items.FindAsync(id);
        }

        public async Task<Item> CreateItemAsync(Item item)
        {
            _context.Items.Add(item);
            await _context.SaveChangesAsync();
            return item;
        }

        public async Task UpdateItemAsync(Item item)
        {
            _context.Entry(item).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task DeleteItemAsync(Guid id)
        {
            var item = await _context.Items.FindAsync(id);
            if (item != null)
            {
                _context.Items.Remove(item);
                await _context.SaveChangesAsync();
            }
        }
    }
}
