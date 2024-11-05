using System;

namespace MinimalApiLoggingApp.Models
{
    public class Configuration
    {
        public int Id { get; set; }
        public string Key { get; set; } 
        public string Value { get; set; } 
        public DateTime LastUpdated { get; set; } 
    }

}
