using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace daleel.Entities
{
    public class Customer
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        public string FullName { get; set; }

        public string Company { get; set; }

        [Required]
        public CustomerType Type { get; set; }

        public string? PhotoURL { get; set; }

        public ContactInfo? ContactInfo { get; set; }

        public AddressInfo? AddressInfo { get; set; }

        public ICollection<Lead>? Leads { get; set; }

        public ICollection<Sale>? Sales { get; set; }
    }

    public enum CustomerType
    {
        Individual = 0,
        Company = 1
    }
}
