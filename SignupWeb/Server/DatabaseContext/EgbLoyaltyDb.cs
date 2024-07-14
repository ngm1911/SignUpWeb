using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Options;
using SignupWeb.Server.Models;

namespace SignupWeb.Server.DatabaseContext
{
    public class EgbLoyaltyDb(DbContextOptions<EgbLoyaltyDb> options) : Microsoft.EntityFrameworkCore.DbContext(options)
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder
                          .LogTo(x => Console.WriteLine(x),
                                    events:
                                    [
                                        RelationalEventId.CommandExecuted,
                                    ])
                          .EnableDetailedErrors()
                          .EnableSensitiveDataLogging();
        }

        public DbSet<Customer> Customers { get; set; } = null!;
    }
}
