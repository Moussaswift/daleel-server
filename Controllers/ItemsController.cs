using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using daleel.Entities;
using daleel.Services;
using Microsoft.EntityFrameworkCore;

namespace daleel.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ItemsController : ControllerBase
    {
        private readonly IItemService _itemService;

        public ItemsController(IItemService itemService)
        {
            _itemService = itemService;
        }

        // GET: api/Items
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Item>>> GetItems()
        {
            var items = await _itemService.GetAllItemsAsync();
            return Ok(items);
        }

        // GET: api/Items/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Item>> GetItem(Guid id)
        {
            var item = await _itemService.GetItemAsync(id);

            if (item == null)
            {
                return NotFound();
            }

            return item;
        }

        // POST: api/Items
        [HttpPost]
        public async Task<ActionResult<Item>> PostItem(Item item)
        {
            var createdItem = await _itemService.CreateItemAsync(item);
            return CreatedAtAction(nameof(GetItem), new { id = createdItem.Id }, createdItem);
        }

        // PUT: api/Items/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutItem(Guid id, Item item)
        {
            if (id != item.Id)
            {
                return BadRequest();
            }

            try
            {
                await _itemService.UpdateItemAsync(item);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await ItemExistsAsync(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // DELETE: api/Items/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteItem(Guid id)
        {
            var item = await _itemService.GetItemAsync(id);
            if (item == null)
            {
                return NotFound();
            }

            await _itemService.DeleteItemAsync(id);

            return NoContent();
        }

        private async Task<bool> ItemExistsAsync(Guid id)
        {
            var item = await _itemService.GetItemAsync(id);
            return item != null;
        }
    }
}
