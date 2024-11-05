using System.Collections.Generic;

namespace MinimalApiLoggingApp.Services
{
    public class BlacklistStore
    {
        private List<string> _blacklistedIps = new List<string>();

        public List<string> GetBlacklistedIps()
        {
            return _blacklistedIps;
        }

        public void UpdateBlacklist(List<string> blacklistedIps)
        {
            _blacklistedIps = blacklistedIps;
        }
    }

}
