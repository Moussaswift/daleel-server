using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace daleel.Entities
{
    public class AddressInfo
    {
        [Key]
        public int Id { get; set; }

        public string StreetAddress { get; set; }

        public string AptNumber { get; set; }

        public string City { get; set; }

        public string State { get; set; }

        public string ZipCode { get; set; }

        public string Country { get; set; }

        public Guid CustomerId { get; set; }
        
        [JsonIgnore]
        public Customer Customer { get; set; }
    }
}
