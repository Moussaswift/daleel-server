using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

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

        [JsonIgnore]
        public Customer Customer { get; set; }
    }
}
