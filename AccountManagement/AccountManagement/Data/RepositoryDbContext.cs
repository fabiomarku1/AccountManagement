using Microsoft.EntityFrameworkCore;

namespace AccountManagement.Data
{
    public class RepositoryDbContext : DbContext
    {

        public RepositoryDbContext(DbContextOptions<RepositoryDbContext> options)
            : base(options)
        {

        }

        
    
        public DbSet<Client> Clients { get; set; }
        public DbSet<Currency> Currencies { get; set; }
        public DbSet<BankAccount> BankAccounts { get; set; }

    }
}
