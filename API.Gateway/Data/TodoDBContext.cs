using API.Gateway.Models;
using Microsoft.EntityFrameworkCore;

namespace API.Gateway.Data
{
    public class TodoDBContext : DbContext
    {
        public DbSet<Todo> Todo => Set<Todo>();

        public TodoDBContext(DbContextOptions<TodoDBContext> options) : base(options)
        { 

        }
    }
}
