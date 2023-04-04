using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ThienASPMVC08032023.Models;

namespace ThienASPMVC08032023.Database
{
    public class AppDbContext : IdentityDbContext<AppUser>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<Clip>? Clips { get; set; }

        public DbSet<MainComment> MainComments { get; set; }
    }
}
