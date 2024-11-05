using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading;
using System;
using System.Linq;
using MinimalApiLoggingApp.Services;

namespace MinimalApiLoggingApp
{

    public class BlacklistUpdateService : BackgroundService
    {
        private readonly IServiceScopeFactory _serviceScopeFactory;
        private readonly ILogger<BlacklistUpdateService> _logger;
        private readonly BlacklistStore _blacklistStore;
        private TimeSpan _updateInterval;

        public BlacklistUpdateService(IServiceScopeFactory serviceScopeFactory, ILogger<BlacklistUpdateService> logger, BlacklistStore blacklistStore)
        {
            _serviceScopeFactory = serviceScopeFactory;
            _logger = logger;
            _blacklistStore = blacklistStore;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    using (var scope = _serviceScopeFactory.CreateScope())
                    {
                        var dbContext = scope.ServiceProvider.GetRequiredService<ApiLoggingDbContext>();
                        var configService = scope.ServiceProvider.GetRequiredService<ConfigurationService>();

                        // Obtains the update interval from the configuration
                        _updateInterval = TimeSpan.FromSeconds(configService.GetMaliciousIpCheckInterval());

                        // Updates the list of blacklisted IPs
                        var blacklistedIps = await dbContext.BlacklistedIps
                            .Where(ip => ip.IsActive)
                            .Select(ip => ip.IpAddress)
                            .ToListAsync();

                        // Updates the BlackListSore with the new list of IPs
                        _blacklistStore.UpdateBlacklist(blacklistedIps);
                    }

                    _logger.LogInformation("Blacklist updated with {Count} IPs", _blacklistStore.GetBlacklistedIps().Count);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error updating blacklist");
                }

                await Task.Delay(_updateInterval, stoppingToken); 
            }
        }
    }




}
