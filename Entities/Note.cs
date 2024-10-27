using System;
using System.ComponentModel.DataAnnotations;

namespace daleel.Entities
{
    public class Note
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        public string Content { get; set; }

        public DateTime CreatedAt { get; set; }

        public Guid LeadId { get; set; }
        public Lead Lead { get; set; }

        public Note()
        {
            Id = Guid.NewGuid();
            CreatedAt = DateTime.UtcNow;
        }

        public Note(string content) : this()
        {
            Content = content;
        }
    }
}
