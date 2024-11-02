using daleel.Entities;

namespace daleel.DTOs
{
    public class CreateLeadDto
    {
        public Guid CustomerId { get; set; }
        public List<Guid> ItemIds { get; set; }
        public Guid SourceId { get; set; }
        public string? Note { get; set; }
        public LeadStatus Status { get; set; }
    }
} 