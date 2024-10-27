using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace daleel.Entities
{
    public class Lead
    {
        [Key]
        public Guid Id { get; set; }

        public Guid? CustomerId { get; set; }
        public Customer Customer { get; set; }

        public ICollection<Item> Items { get; set; }

        public Guid? SourceId { get; set; }
        public Source Source { get; set; }

        public ICollection<Note> Notes { get; set; }

        public LeadStatus Status { get; set; }

        public DateTime CreatedAt { get; set; }

        public Lead()
        {
            Id = Guid.NewGuid();
            CreatedAt = DateTime.UtcNow;
            Status = LeadStatus.Open;
            Items = new List<Item>();
            Notes = new List<Note>();
        }

        public Lead(Customer customer, IEnumerable<Item> items, Source source, IEnumerable<string> notes, LeadStatus status) : this()
        {
            Customer = customer;
            Items = new List<Item>(items);
            Source = source;
            Notes = new List<Note>(notes.Select(n => new Note(n)));
            Status = status;
        }
    }
}
