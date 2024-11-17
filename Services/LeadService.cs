using daleel.Data;
using daleel.DTOs;
using daleel.Entities;
using Microsoft.EntityFrameworkCore;
using daleel.Models;

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

        public async Task UpdateLeadAsync(Guid id, LeadUpdateModel model)
        {
            var lead = await _context.Leads
                .Include(l => l.Customer)
                .Include(l => l.Source)
                .FirstOrDefaultAsync(l => l.Id == id);

            if (lead == null)
                throw new KeyNotFoundException($"Lead with ID {id} not found");

            if (model.CustomerId.HasValue)
            {
                var customer = await _context.Customers.FindAsync(model.CustomerId.Value);
                if (customer == null)
                    throw new KeyNotFoundException("Customer not found");
                lead.Customer = customer;
                lead.CustomerId = customer.Id;
            }

            if (model.SourceId.HasValue)
            {
                var source = await _context.Sources.FindAsync(model.SourceId.Value);
                if (source == null)
                    throw new KeyNotFoundException("Source not found");
                lead.Source = source;
                lead.SourceId = source.Id;
            }

            if (model.Status.HasValue)
            {
                lead.Status = model.Status.Value;
            }

            await _context.SaveChangesAsync();
        }

        public async Task DeleteLeadAsync(Guid id)
        {
            var lead = await _context.Leads.FindAsync(id);
            if (lead == null)
                throw new KeyNotFoundException($"Lead with ID {id} not found");

            _context.Leads.Remove(lead);
            await _context.SaveChangesAsync();
        }

        public async Task AddNoteToLeadAsync(Guid leadId, LeadNoteModel model)
        {
            var lead = await _context.Leads
                .Include(l => l.Notes)
                .FirstOrDefaultAsync(l => l.Id == leadId);

            if (lead == null)
                throw new KeyNotFoundException($"Lead with ID {leadId} not found");

            var note = new Note
            {
                Content = model.Content,
                LeadId = leadId
            };

            _context.Notes.Add(note);
            lead.Notes.Add(note);

            await _context.SaveChangesAsync();
        }

        public async Task DeleteNoteFromLeadAsync(Guid leadId, Guid noteId)
        {
            var lead = await _context.Leads
                .Include(l => l.Notes)
                .FirstOrDefaultAsync(l => l.Id == leadId);

            if (lead == null)
                throw new KeyNotFoundException($"Lead with ID {leadId} not found");

            var note = lead.Notes.FirstOrDefault(n => n.Id == noteId);
            if (note == null)
                throw new KeyNotFoundException($"Note with ID {noteId} not found");

            lead.Notes.Remove(note);
            await _context.SaveChangesAsync();
        }

        public async Task AddItemToLeadAsync(Guid leadId, LeadItemModel model)
        {
            var lead = await _context.Leads
                .Include(l => l.Items)
                .FirstOrDefaultAsync(l => l.Id == leadId);

            if (lead == null)
                throw new KeyNotFoundException($"Lead with ID {leadId} not found");

            var item = new Item
            {
                Id = model.ItemId,
                Name = model.Name,
                Price = model.Price,
                Quantity = model.Quantity,
                Notes = model.Notes
            };

            // Add item to both context and lead's collection
            _context.Items.Add(item);
            lead.Items.Add(item);

            await _context.SaveChangesAsync();
        }

        public async Task DeleteItemFromLeadAsync(Guid leadId, Guid itemId)
        {
            var lead = await _context.Leads
                .Include(l => l.Items)
                .FirstOrDefaultAsync(l => l.Id == leadId);

            if (lead == null)
                throw new KeyNotFoundException($"Lead with ID {leadId} not found");

            var item = lead.Items.FirstOrDefault(i => i.Id == itemId);
            if (item == null)
                throw new KeyNotFoundException($"Item with ID {itemId} not found");

            lead.Items.Remove(item);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateNoteAsync(Guid leadId, Guid noteId, LeadNoteUpdateModel model)
        {
            var lead = await _context.Leads
                .Include(l => l.Notes)
                .FirstOrDefaultAsync(l => l.Id == leadId);

            if (lead == null)
                throw new KeyNotFoundException($"Lead with ID {leadId} not found");

            var note = lead.Notes.FirstOrDefault(n => n.Id == noteId);
            if (note == null)
                throw new KeyNotFoundException($"Note with ID {noteId} not found");

            // Update note properties
            note.Content = model.Content;

            _context.Entry(note).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task UpdateItemAsync(Guid leadId, Guid itemId, LeadItemUpdateModel model)
        {
            var lead = await _context.Leads
                .Include(l => l.Items)
                .FirstOrDefaultAsync(l => l.Id == leadId);

            if (lead == null)
                throw new KeyNotFoundException($"Lead with ID {leadId} not found");

            var item = lead.Items.FirstOrDefault(i => i.Id == itemId);
            if (item == null)
                throw new KeyNotFoundException($"Item with ID {itemId} not found in this lead");

            // Update only provided properties
            if (model.Name != null)
                item.Name = model.Name;
            if (model.Price.HasValue)
                item.Price = model.Price.Value;
            if (model.Quantity.HasValue)
                item.Quantity = model.Quantity.Value;
            if (model.Notes != null)
                item.Notes = model.Notes;

            _context.Entry(item).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }
    }

    public interface ILeadService
    {
        Task<PaginatedResponseDto<Lead>> GetLeadsAsync(PaginationDto pagination);
        Task<Lead> GetLeadByIdAsync(Guid id);
        Task<Lead> CreateLeadAsync(CreateLeadDto createLeadDto);
        Task UpdateLeadAsync(Guid id, LeadUpdateModel model);
        Task DeleteLeadAsync(Guid id);
        Task AddNoteToLeadAsync(Guid leadId, LeadNoteModel model);
        Task DeleteNoteFromLeadAsync(Guid leadId, Guid noteId);
        Task AddItemToLeadAsync(Guid leadId, LeadItemModel model);
        Task DeleteItemFromLeadAsync(Guid leadId, Guid itemId);
        Task UpdateNoteAsync(Guid leadId, Guid noteId, LeadNoteUpdateModel model);
        Task UpdateItemAsync(Guid leadId, Guid itemId, LeadItemUpdateModel model);
    }
} 