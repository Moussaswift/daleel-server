using Microsoft.EntityFrameworkCore;
using daleel.Data;
using daleel.Entities;

namespace daleel.Services
{
    public class SourceService : ISourceService
    {
        private readonly ApplicationDbContext _context;

        public SourceService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Source>> GetAllSourcesAsync()
        {
            return await _context.Sources.ToListAsync();
        }

        public async Task<Source> GetSourceAsync(Guid id)
        {
            return await _context.Sources.FindAsync(id);
        }

        public async Task<Source> CreateSourceAsync(Source source)
        {
            _context.Sources.Add(source);
            await _context.SaveChangesAsync();
            return source;
        }

        public async Task UpdateSourceAsync(Source source)
        {
            _context.Entry(source).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task DeleteSourceAsync(Guid id)
        {
            var source = await _context.Sources.FindAsync(id);
            if (source != null)
            {
                _context.Sources.Remove(source);
                await _context.SaveChangesAsync();
            }
        }
    }
}
