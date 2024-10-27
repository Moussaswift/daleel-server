using System;
using System.ComponentModel.DataAnnotations;

namespace daleel.Entities
{
    public class Source
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        public string Name { get; set; }

        public Source()
        {
            Id = Guid.NewGuid();
        }

        public Source(string name) : this()
        {
            Name = name;
        }
    }
}
