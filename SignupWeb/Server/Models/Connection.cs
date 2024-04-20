namespace SignupWeb.Server.Models
{
    public class Connection
    {
        public Connect[] Connects { get; set; }
    }

    public class Connect
    {
        public string DataSource { get; set; }
        public string User { get; set; }
        public string Password { get; set; }
        public string Key { get; set; }
        public bool InUse { get; set; }
    }
}
