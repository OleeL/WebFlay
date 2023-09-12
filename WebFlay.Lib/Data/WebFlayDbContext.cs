using Microsoft.EntityFrameworkCore;
using WebFlay.Lib.Models.Database;

namespace WebFlay.Lib.Data;

public class WebFlayDbContext : DbContext
{
    
    public WebFlayDbContext(DbContextOptions<WebFlayDbContext> options) : base(options)
    {   
    }

    public DbSet<ScrapeResultDbModel> ScrapeHistory { get; set; } = null!;

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
    }
}