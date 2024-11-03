using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using daleel.Data;
using daleel.Entities;
using Microsoft.EntityFrameworkCore;
using daleel.Services;
using daleel.DTOs;

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
        public async Task<IActionResult> PutLead(Guid id, Lead lead)
        {
            try
            {
                await _leadService.UpdateLeadAsync(id, lead);
                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (ArgumentException)
            {
                return BadRequest();
            }
            catch (DbUpdateConcurrencyException)
            {
                return StatusCode(409, "The lead has been modified by another user");
            }
            catch (Exception)
            {
                return StatusCode(500, "An error occurred while updating the lead");
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
