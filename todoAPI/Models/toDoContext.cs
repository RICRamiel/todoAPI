using Microsoft.EntityFrameworkCore;
using todoAPI.Models;

namespace TodoApi.Models;

public class TodoContext : DbContext
{
    public TodoContext(DbContextOptions<TodoContext> options)
        : base(options)
    {
    }

    public DbSet<todoItem> TodoItems { get; set; } = null!;
}