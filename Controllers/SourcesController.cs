using Microsoft.AspNetCore.Mvc;
using daleel.Entities;
using daleel.Services;
using Microsoft.EntityFrameworkCore;

namespace daleel.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SourcesController : ControllerBase
    {
        private readonly ISourceService _sourceService;

        public SourcesController(ISourceService sourceService)
        {
            _sourceService = sourceService;
        }

        // GET: api/Sources
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Source>>> GetSources()
        {
            var sources = await _sourceService.GetAllSourcesAsync();
            return Ok(sources);
        }

        // GET: api/Sources/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Source>> GetSource(Guid id)
        {
            var source = await _sourceService.GetSourceAsync(id);

            if (source == null)
            {
                return NotFound();
            }

            return source;
        }

        // POST: api/Sources
        [HttpPost]
        public async Task<ActionResult<Source>> PostSource(Source source)
        {
            var createdSource = await _sourceService.CreateSourceAsync(source);
            return CreatedAtAction(nameof(GetSource), new { id = createdSource.Id }, createdSource);
        }

        // PUT: api/Sources/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutSource(Guid id, Source source)
        {
            if (id != source.Id)
            {
                return BadRequest();
            }

            try
            {
                await _sourceService.UpdateSourceAsync(source);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await SourceExistsAsync(id))
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

        // DELETE: api/Sources/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSource(Guid id)
        {
            var source = await _sourceService.GetSourceAsync(id);
            if (source == null)
            {
                return NotFound();
            }

            await _sourceService.DeleteSourceAsync(id);

            return NoContent();
        }

        private async Task<bool> SourceExistsAsync(Guid id)
        {
            var source = await _sourceService.GetSourceAsync(id);
            return source != null;
        }
    }
}
