using System.ComponentModel.DataAnnotations;
using daleel.Entities;

namespace daleel.Models
{
    public class CustomerCreateModel
    {
        [Required]
        public string FullName { get; set; }

        public string? Company { get; set; }

        [Required]
        public CustomerType Type { get; set; }

        public string? PhotoURL { get; set; }

        public ContactInfoCreateModel? ContactInfo { get; set; }

        public AddressInfoCreateModel? AddressInfo { get; set; }
    }

    public class ContactInfoCreateModel
    {
        public string EmailAddress { get; set; }
        public string HomePhone { get; set; }
        public string WorkPhone { get; set; }
    }

    public class AddressInfoCreateModel
    {
        public string StreetAddress { get; set; }
        public string AptNumber { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string ZipCode { get; set; }
        public string Country { get; set; }
    }
} 