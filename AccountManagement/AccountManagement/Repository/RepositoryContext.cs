using AccountManagement.Data;
using Microsoft.EntityFrameworkCore;

namespace AccountManagement.Repository
{
    public class RepositoryContext : DbContext
    {
        public RepositoryContext(DbContextOptions<RepositoryContext> options)
            : base(options)
        {

        }


        public DbSet<Client> Clients { get; set; }
        // public DbSet<Client> Clients { get; set; }
        //  public DbSet<Currency> Currencies { get; set; }
        //    public DbSet<BankAccount> BankAccounts { get; set; }

    }
}