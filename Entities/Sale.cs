using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace daleel.Entities
{
    public class Sale
    {
        [Key]
        public Guid Id { get; set; }

        public DateTime Date { get; set; }

        public Guid? CustomerId { get; set; }
        public Customer Customer { get; set; }

        public Guid? SourceId { get; set; }
        public Source Source { get; set; }

        public Guid? ItemId { get; set; }
        public Item Item { get; set; }

        public decimal SalesTax { get; set; }

        public decimal Discount { get; set; }

        public decimal Total { get; set; }

        public ICollection<Status> Status { get; set; }

        public string Note { get; set; }

        public Sale()
        {
            Id = Guid.NewGuid();
            Date = DateTime.UtcNow;
            Status = new List<Status>();
        }

        public Sale(DateTime date, Customer customer, Source source, Item item, decimal salesTax, decimal discount, decimal total, IEnumerable<Status> status, string note) : this()
        {
            Date = date;
            Customer = customer;
            Source = source;
            Item = item;
            SalesTax = salesTax;
            Discount = discount;
            Total = total;
            Status = new List<Status>(status);
            Note = note;
        }
    }
}
