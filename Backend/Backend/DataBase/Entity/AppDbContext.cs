using System.ComponentModel.DataAnnotations.Schema;
using Backend.DataBase.Model;
using Microsoft.EntityFrameworkCore;

namespace Backend.DataBase.Entity;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<User> User { get; set; }
    public DbSet<Post> Posts { get; set; }
    public DbSet<Comment> Comment { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseNpgsql("User ID=postgres;Password=1234;Host=localhost;Port=5432;Database=test;");
    }
}