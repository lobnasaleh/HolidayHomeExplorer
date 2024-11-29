using MagicVillaApI2.Models;
using Microsoft.EntityFrameworkCore;

namespace MagicVillaApI2.Data
{
    public class ApplicationDbContext:DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }

        public DbSet<Villa> Villas { get; set; }
    }
}
