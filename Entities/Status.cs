using System;
using System.ComponentModel.DataAnnotations;

namespace daleel.Entities
{
    public class Status
    {
        [Key]
        public Guid Id { get; set; }

        public StatusType Type { get; set; }

        public DateTime Date { get; set; }

        public Guid SaleId { get; set; }
        public Sale Sale { get; set; }

        public Status()
        {
            Id = Guid.NewGuid();
            Date = DateTime.UtcNow;
        }

        public Status(StatusType type, DateTime date) : this()
        {
            Type = type;
            Date = date;
        }
    }
}
