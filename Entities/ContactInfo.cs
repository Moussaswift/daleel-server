using System.ComponentModel.DataAnnotations;

namespace daleel.Entities
{
    public class ContactInfo
    {
        [Key]
        public int Id { get; set; }

        public string EmailAddress { get; set; }

        public string HomePhone { get; set; }

        public string WorkPhone { get; set; }

        public Guid CustomerId { get; set; }
        public Customer Customer { get; set; }
    }
}
