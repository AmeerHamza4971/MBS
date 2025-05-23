using Microsoft.EntityFrameworkCore;

namespace MBS.Models;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions options) : base(options)
    {

    }

    public DbSet<Billing> Billings { get; set; }
    public DbSet<TodoTask> TodoTasks { get; set; }


}
