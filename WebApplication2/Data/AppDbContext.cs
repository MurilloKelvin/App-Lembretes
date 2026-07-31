using Microsoft.EntityFrameworkCore;
using RemindersDTI.Models;

namespace RemindersDTI.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) 
            : base(options)
        {

        }

        public DbSet<Reminder> Reminders { get; set; } // tabela dos lembretes
    }
}
