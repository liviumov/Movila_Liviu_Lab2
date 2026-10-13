using Microsoft.EntityFrameworkCore;

public class Movila_Liviu_Lab2Context(DbContextOptions<Movila_Liviu_Lab2Context> options) : DbContext(options)
{
    public DbSet<Movila_Liviu_Lab2.Models.Book> Book { get; set; } = default!;
    public DbSet<Movila_Liviu_Lab2.Models.Publisher> Publisher { get; set; } = default!;
    public DbSet<Movila_Liviu_Lab2.Models.Author> Author { get; set; } = default!;
}   