namespace PhishingApp.Model
{
    public class Website
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Url { get; set; }

        public int EmailsSent { get; set; }

        public int FormsFilled { get; set; }

        Website() { }
    }
}
