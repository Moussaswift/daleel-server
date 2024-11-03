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

        public async Task<PaginatedResponseDto<Lead>> GetLeadsAsync(PaginationDto pagination)
        {
            var query = _context.Leads
                .Select(l => new Lead
                {
                    Id = l.Id,
                    CustomerId = l.CustomerId,
                    Customer = new Customer
                    {
                        Id = l.Customer.Id,
                        FullName = l.Customer.FullName,
                        Company = l.Customer.Company,
                        Type = l.Customer.Type,
                        PhotoURL = l.Customer.PhotoURL,
                        ContactInfo = l.Customer.ContactInfo,
                        AddressInfo = l.Customer.AddressInfo,
                    },
                    SourceId = l.SourceId,
                    Source = l.Source,
                    Items = l.Items,
                    Notes = l.Notes,
                    Status = l.Status
                });

            var totalCount = await query.CountAsync();
            var totalPages = (int)Math.Ceiling(totalCount / (double)pagination.PageSize);

            var items = await query
                .Skip((pagination.PageNumber - 1) * pagination.PageSize)
                .Take(pagination.PageSize)
                .ToListAsync();

            return new PaginatedResponseDto<Lead>
            {
                Items = items,
                PageNumber = pagination.PageNumber,
                PageSize = pagination.PageSize,
                TotalPages = totalPages,
                TotalCount = totalCount
            };
        }

        public async Task<Lead> GetLeadByIdAsync(Guid id)
        {
            var lead = await _context.Leads
                .Select(l => new Lead
                {
                    Id = l.Id,
                    CustomerId = l.CustomerId,
                    Customer = new Customer
                    {
                        Id = l.Customer.Id,
                        FullName = l.Customer.FullName,
                        Company = l.Customer.Company,
                        Type = l.Customer.Type,
                        PhotoURL = l.Customer.PhotoURL,
                        ContactInfo = l.Customer.ContactInfo,
                        AddressInfo = l.Customer.AddressInfo,
                        // Leads and Sales are intentionally omitted
                    },
                    SourceId = l.SourceId,
                    Source = l.Source,
                    Items = l.Items,
                    Notes = l.Notes,
                    Status = l.Status
                })
                .FirstOrDefaultAsync(l => l.Id == id);

            if (lead == null)
                throw new KeyNotFoundException($"Lead with ID {id} not found");

            return lead;
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

        public async Task UpdateLeadAsync(Guid id, Lead lead)
        {
            if (id != lead.Id)
                throw new ArgumentException("ID mismatch");

            var existingLead = await _context.Leads.FindAsync(id);
            if (existingLead == null)
                throw new KeyNotFoundException($"Lead with ID {id} not found");

            _context.Entry(existingLead).State = EntityState.Detached;
            _context.Entry(lead).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                throw;
            }
        }

        public async Task DeleteLeadAsync(Guid id)
        {
            var lead = await _context.Leads.FindAsync(id);
            if (lead == null)
                throw new KeyNotFoundException($"Lead with ID {id} not found");

            _context.Leads.Remove(lead);
            await _context.SaveChangesAsync();
        }
    }

    public interface ILeadService
    {
        Task<PaginatedResponseDto<Lead>> GetLeadsAsync(PaginationDto pagination);
        Task<Lead> GetLeadByIdAsync(Guid id);
        Task<Lead> CreateLeadAsync(CreateLeadDto createLeadDto);
        Task UpdateLeadAsync(Guid id, Lead lead);
        Task DeleteLeadAsync(Guid id);
    }
} 