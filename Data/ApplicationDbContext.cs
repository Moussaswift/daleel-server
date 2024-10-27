using Microsoft.EntityFrameworkCore;
using daleel.Entities;

namespace daleel.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        // Add DbSet properties for your entities here
        public DbSet<Product> Products { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<ContactInfo> ContactInfos { get; set; }
        public DbSet<AddressInfo> AddressInfos { get; set; }
        public DbSet<Item> Items { get; set; }
        public DbSet<Lead> Leads { get; set; }
        public DbSet<Note> Notes { get; set; }
        public DbSet<Source> Sources { get; set; }
        public DbSet<Sale> Sales { get; set; }
        public DbSet<Status> Statuses { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Customer>()
                .HasOne(c => c.ContactInfo)
                .WithOne(ci => ci.Customer)
                .HasForeignKey<ContactInfo>(ci => ci.CustomerId);

            modelBuilder.Entity<Customer>()
                .HasOne(c => c.AddressInfo)
                .WithOne(ai => ai.Customer)
                .HasForeignKey<AddressInfo>(ai => ai.CustomerId);

            modelBuilder.Entity<Lead>()
                .HasMany(l => l.Items)
                .WithMany();

            modelBuilder.Entity<Lead>()
                .HasMany(l => l.Notes)
                .WithOne(n => n.Lead)
                .HasForeignKey(n => n.LeadId);

            modelBuilder.Entity<Sale>()
                .HasMany(s => s.Status)
                .WithOne(st => st.Sale)
                .HasForeignKey(st => st.SaleId);
        }
    }
}
