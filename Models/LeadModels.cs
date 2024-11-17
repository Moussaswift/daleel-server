using System;
using System.ComponentModel.DataAnnotations;
using daleel.Entities;

namespace daleel.Models
{
    public class LeadUpdateModel
    {
        public Guid? CustomerId { get; set; }
        public Guid? SourceId { get; set; }
        public LeadStatus? Status { get; set; }
    }

    public class LeadCreateModel
    {
        [Required]
        public Guid CustomerId { get; set; }
        
        [Required]
        public Guid SourceId { get; set; }
        
        public LeadStatus Status { get; set; }
        
        public List<Guid> ItemIds { get; set; } = new();
        
        public string? Note { get; set; }
    }

    public class LeadNoteModel
    {
        [Required]
        public string Content { get; set; }
    }

    public class LeadNoteUpdateModel
    {
        [Required]
        public string Content { get; set; }
    }

    public class LeadItemModel
    {
        [Required]
        public Guid ItemId { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public decimal Price { get; set; }

        [Required]
        public int Quantity { get; set; }

        public string? Notes { get; set; }
    }

    public class LeadItemUpdateModel
    {
        public string? Name { get; set; }
        public decimal? Price { get; set; }
        public int? Quantity { get; set; }
        public string? Notes { get; set; }
    }
} 