using AVSC.Database.Enums;

namespace AVSC.Database.Settings
{
    public class DatabaseSettings
    {
        public DatabaseType DatabaseType { get; set; }
        public string ServerName { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string DatabaseName { get; set; }
    }
}