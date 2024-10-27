using daleel.Entities;

namespace daleel.Services
{
    public interface IItemService
    {
        Task<IEnumerable<Item>> GetAllItemsAsync();
        Task<Item> GetItemAsync(Guid id);
        Task<Item> CreateItemAsync(Item item);
        Task UpdateItemAsync(Item item);
        Task DeleteItemAsync(Guid id);
    }
}
