namespace MinimalApiLoggingApp.Services
{
    using Microsoft.EntityFrameworkCore;
    using System.Globalization;
    using System.Linq;

    public class ConfigurationService
    {
        private readonly ApiLoggingDbContext _dbContext;

        public ConfigurationService(ApiLoggingDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        
        public int GetMaliciousIpCheckInterval()
        {
            var config = _dbContext.Configurations.FirstOrDefault(c => c.Key == "MaliciousIpCheckIntervalInSeconds");
            return config != null ? int.Parse(config.Value, CultureInfo.InvariantCulture) : 60; // Valor predeterminado
        }

        
    }

}
