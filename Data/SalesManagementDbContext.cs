using Microsoft.EntityFrameworkCore;
using SalesManagement.Models;

namespace SalesManagement.Data
{
    public class SalesManagementDbContext : DbContext
    {
        public SalesManagementDbContext(DbContextOptions<SalesManagementDbContext> options)
            : base(options)
        {
        }
        public DbSet<Seller> Seller { get; set; }
        public DbSet<Sale> Sale { get; set; }
    }
}