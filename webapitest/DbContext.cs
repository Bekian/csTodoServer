using Microsoft.EntityFrameworkCore;
namespace webapitest;
public class TaskItem
{
    public required int ID { get; set; }

    public required string Description { get; set; }

    public required DateTime Date { get; set; }

    public required bool Completed { get; set; }
}

public class TaskContext : DbContext
{
    public DbSet<TaskItem> Tasks { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder options)
    {
        options.UseSqlite("Data Source=tasks.db");
    }
}
