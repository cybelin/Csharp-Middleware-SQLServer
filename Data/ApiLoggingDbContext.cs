using Microsoft.EntityFrameworkCore;
using MinimalApiLoggingApp.Models;

public class ApiLoggingDbContext : DbContext
{
    public ApiLoggingDbContext(DbContextOptions<ApiLoggingDbContext> options) : base(options) { }

    public DbSet<RequestLog> RequestLogs { get; set; }
    public DbSet<ResponseLog> ResponseLogs { get; set; }

    public DbSet<BlacklistedIp> BlacklistedIps { get; set; }
    public DbSet<Configuration> Configurations { get; set; } 


}
