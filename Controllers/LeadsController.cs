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
        public async Task<ActionResult<IEnumerable<Lead>>> GetLeads()
        {
            return await _context.Leads.ToListAsync();
        }

        // GET: api/Leads/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Lead>> GetLead(Guid id)
        {
            var lead = await _context.Leads.FindAsync(id);

            if (lead == null)
            {
                return NotFound();
            }

            return lead;
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
            if (id != lead.Id)
            {
                return BadRequest();
            }

            _context.Entry(lead).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!LeadExists(id))
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

        // DELETE: api/Leads/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteLead(Guid id)
        {
            var lead = await _context.Leads.FindAsync(id);
            if (lead == null)
            {
                return NotFound();
            }

            _context.Leads.Remove(lead);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool LeadExists(Guid id)
        {
            return _context.Leads.Any(e => e.Id == id);
        }
    }
}
