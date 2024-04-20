using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using SignupWeb.Server.Models;

namespace SignupWeb.Server.DatabaseContext
{
    public class EgbLoyaltyDb : DbContext
    {
        readonly string _connectionString;
        public EgbLoyaltyDb(IOptions<Connection> connection)
        {
            var connectInUse = connection.Value.Connects.Single(x => x.InUse);
            var sConnB = new SqlConnectionStringBuilder()
            {
                DataSource = connectInUse.DataSource,
                InitialCatalog = "EGBLoyalty",
                UserID = connectInUse.User,
                Password = connectInUse.Password,
                TrustServerCertificate = true,
                IntegratedSecurity = false,
                ConnectTimeout = 60,
            };
            _connectionString = sConnB.ConnectionString;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(_connectionString);
            }
        }

        public DbSet<Customer> Customers { get; set; } = null!;
    }
}
