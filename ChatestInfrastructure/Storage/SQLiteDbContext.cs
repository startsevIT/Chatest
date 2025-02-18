using ChatestDomain.BusinessEntities.Entities;
using Microsoft.EntityFrameworkCore;

namespace ChatestInfrastructure.Storage;

public class SQLiteDbContext : DbContext
{
    public DbSet<Message> Messages { get; set; }
    public DbSet<Chat> Chats { get; set; }
    public DbSet<User> Users { get; set; }

    public SQLiteDbContext()
    {
        Database.EnsureCreated();
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlite("Data Source = chatest.db");
    }
}
