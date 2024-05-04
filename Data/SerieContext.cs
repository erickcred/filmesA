using FilmesApi.Models;
using Microsoft.EntityFrameworkCore;

namespace FilmesApi.Data;

public class SerieContext : DbContext // FilmeContext
{
  //public SerieContext(DbContextOptions<SerieContext> options) : base(options)
  //{
  //}

  public DbSet<Serie> Serie { get; set; }

  protected override void OnModelCreating(ModelBuilder modelBuilder)
  {
    //base.OnModelCreating(modelBuilder);
    modelBuilder.Entity<Serie>().HasKey(e => e.Id);
    modelBuilder.Entity<Serie>().Property(e => e.Id).ValueGeneratedOnAdd();
  }
}
