using daleel.Data;
using daleel.DTOs;
using daleel.Entities;
using Microsoft.EntityFrameworkCore;

namespace daleel.Services
{
    public class LeadService : ILeadService
    {
        private readonly ApplicationDbContext _context;

        public LeadService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Lead> CreateLeadAsync(CreateLeadDto createLeadDto)
        {
            var customer = await _context.Customers
                .FirstOrDefaultAsync(c => c.Id == createLeadDto.CustomerId);
            if (customer == null)
                throw new KeyNotFoundException("Customer not found");

            var source = await _context.Sources
                .FirstOrDefaultAsync(s => s.Id == createLeadDto.SourceId);
            if (source == null)
                throw new KeyNotFoundException("Source not found");

            var items = await _context.Items
                .Where(i => createLeadDto.ItemIds.Contains(i.Id))
                .ToListAsync();
            if (items.Count != createLeadDto.ItemIds.Count)
                throw new KeyNotFoundException("One or more items not found");

            var lead = new Lead
            {
                CustomerId = customer.Id,
                Customer = customer,
                SourceId = source.Id,
                Source = source,
                Items = items,
                Status = createLeadDto.Status
            };

            if (!string.IsNullOrEmpty(createLeadDto.Note))
            {
                lead.Notes.Add(new Note(createLeadDto.Note));
            }

            _context.Leads.Add(lead);
            await _context.SaveChangesAsync();

            return lead;
        }
    }

    public interface ILeadService
    {
        Task<Lead> CreateLeadAsync(CreateLeadDto createLeadDto);
    }
} 