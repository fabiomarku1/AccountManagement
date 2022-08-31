using System.ComponentModel;
using System.Data;
using AccountManagement.Data;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace AccountManagement.Data
{
    public class DapperDbContext
    {
        private readonly IConfiguration _configuration;
        private readonly string _connectionString;

        public DapperDbContext(IConfiguration configuration)
        {
            _configuration = configuration;
            _connectionString = _configuration.GetConnectionString("SqlConnection");
        }

        public IDbConnection CreateConnection() => new SqlConnection(_connectionString);


        /*
        public DapperDbContext(DbContextOptions<DapperDbContext> options)
            : base(options)
        {

        }



        public DbSet<Client> Clients { get; set; }
        public DbSet<Currency> Currencies { get; set; }
        public DbSet<BankAccount> BankAccounts { get; set; }
        */
    }
}
