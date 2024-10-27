using System;
using System.ComponentModel.DataAnnotations;

namespace daleel.Entities
{
    public class Item
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        public string Name { get; set; }

        public int Quantity { get; set; }

        public string Notes { get; set; }

        public decimal Price { get; set; }

        public Item()
        {
            Id = Guid.NewGuid();
        }

        public Item(string name, int quantity, string notes, decimal price) : this()
        {
            Name = name;
            Quantity = quantity;
            Notes = notes;
            Price = price;
        }
    }
}
