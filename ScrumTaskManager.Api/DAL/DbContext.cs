using Microsoft.EntityFrameworkCore;
using ScrumTaskManager.Api.DAL.Entities;

namespace ScrumTaskManager.Api.DAL
{
    public class DbContext : Microsoft.EntityFrameworkCore.DbContext
    {
        public DbSet<ToDoTask> Tasks { get; set; }
        public DbSet<User> Users { get; set; }

        public DbContext(DbContextOptions<DbContext> options) : base(options)
        {

        }
    }
}
