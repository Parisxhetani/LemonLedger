using AuthService.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace AuthService.Infrastructure
{
    public class AuthDbContextFactory
        : IDesignTimeDbContextFactory<AuthDbContext>
    {
        public AuthDbContext CreateDbContext(string[] args)
        {
            var builder = new DbContextOptionsBuilder<AuthDbContext>();

            // Use the same connection string you have in appsettings.json
            // Replace placeholders with your real values or
            // read from an environment variable if you prefer
            var conn = "server=tonic.o2switch.net;database=lafe6113_LemonLedger_AuthService;user=lafe6113_kristi;password=!o!fl_8Y6oMM;";

            builder.UseMySql(conn, ServerVersion.AutoDetect(conn));

            return new AuthDbContext(builder.Options);
        }
    }
}
