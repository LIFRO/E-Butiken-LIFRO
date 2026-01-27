using Microsoft.EntityFrameworkCore;
using API.Models;
using Microsoft.Extensions.Options;

namespace API.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> Options) : base(Options){}

    public DbSet<AppUser> Users => Set<AppUser>();

}