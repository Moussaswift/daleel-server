using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using daleel.Data;
using daleel.Entities;
using Microsoft.EntityFrameworkCore;
using daleel.Services;
using daleel.DTOs;
using daleel.Models;

namespace daleel.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LeadsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly ILeadService _leadService;

        public LeadsController(ApplicationDbContext context, ILeadService leadService)
        {
            _context = context;
            _leadService = leadService;
        }

        // GET: api/Leads
        [HttpGet]
        public async Task<ActionResult<PaginatedResponseDto<Lead>>> GetLeads([FromQuery] PaginationDto pagination)
        {
            try
            {
                var paginatedLeads = await _leadService.GetLeadsAsync(pagination);
                return Ok(paginatedLeads);
            }
            catch (Exception)
            {
                return StatusCode(500, "An error occurred while retrieving leads");
            }
        }

        // GET: api/Leads/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Lead>> GetLead(Guid id)
        {
            try
            {
                var lead = await _leadService.GetLeadByIdAsync(id);
                return Ok(lead);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception)
            {
                return StatusCode(500, "An error occurred while retrieving the lead");
            }
        }

        // POST: api/Leads
        [HttpPost]
        public async Task<ActionResult> PostLead(CreateLeadDto createLeadDto)
        {
            try
            {
                var lead = await _leadService.CreateLeadAsync(createLeadDto);
                return Created(string.Empty, null);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception)
            {
                return StatusCode(500, "An error occurred while creating the lead");
            }
        }

        // PUT: api/Leads/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutLead(Guid id, LeadUpdateModel model)
        {
            try
            {
                await _leadService.UpdateLeadAsync(id, model);
                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception)
            {
                return StatusCode(500, "An error occurred while updating the lead");
            }
        }

        // POST: api/Leads/{id}/notes
        [HttpPost("{id}/notes")]
        public async Task<IActionResult> AddNote(Guid id, LeadNoteModel model)
        {
            try
            {
                await _leadService.AddNoteToLeadAsync(id, model);
                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception)
            {
                return StatusCode(500, "An error occurred while adding the note");
            }
        }

        // PUT: api/Leads/{id}/notes/{noteId}
        [HttpPut("{id}/notes/{noteId}")]
        public async Task<IActionResult> UpdateNote(Guid id, Guid noteId, LeadNoteUpdateModel model)
        {
            try
            {
                await _leadService.UpdateNoteAsync(id, noteId, model);
                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception)
            {
                return StatusCode(500, "An error occurred while updating the note");
            }
        }

        // DELETE: api/Leads/{id}/notes/{noteId}
        [HttpDelete("{id}/notes/{noteId}")]
        public async Task<IActionResult> DeleteNote(Guid id, Guid noteId)
        {
            try
            {
                await _leadService.DeleteNoteFromLeadAsync(id, noteId);
                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception)
            {
                return StatusCode(500, "An error occurred while deleting the note");
            }
        }

        // POST: api/Leads/{id}/items
        [HttpPost("{id}/items")]
        public async Task<IActionResult> AddItem(Guid id, LeadItemModel model)
        {
            try
            {
                await _leadService.AddItemToLeadAsync(id, model);
                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception)
            {
                return StatusCode(500, "An error occurred while adding the item");
            }
        }

        // PUT: api/Leads/{id}/items/{itemId}
        [HttpPut("{id}/items/{itemId}")]
        public async Task<IActionResult> UpdateItem(Guid id, Guid itemId, LeadItemUpdateModel model)
        {
            try
            {
                await _leadService.UpdateItemAsync(id, itemId, model);
                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception)
            {
                return StatusCode(500, "An error occurred while updating the item");
            }
        }

        // DELETE: api/Leads/{id}/items/{itemId}
        [HttpDelete("{id}/items/{itemId}")]
        public async Task<IActionResult> DeleteItem(Guid id, Guid itemId)
        {
            try
            {
                await _leadService.DeleteItemFromLeadAsync(id, itemId);
                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception)
            {
                return StatusCode(500, "An error occurred while deleting the item");
            }
        }

        // DELETE: api/Leads/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteLead(Guid id)
        {
            try
            {
                await _leadService.DeleteLeadAsync(id);
                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception)
            {
                return StatusCode(500, "An error occurred while deleting the lead");
            }
        }
    }
}
