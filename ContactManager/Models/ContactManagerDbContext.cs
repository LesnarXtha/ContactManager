using Microsoft.EntityFrameworkCore;

namespace ContactManager.Models
{
    public class ContactManagerDbContext : DbContext
    {
        public ContactManagerDbContext(DbContextOptions<ContactManagerDbContext> options) : base(options) { }

        public DbSet<Contact> Contacts { get; set; }
    }
}
