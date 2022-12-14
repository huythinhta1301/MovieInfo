namespace Movie.Models
{
    public class EmailDTO
    {
        public User user { get; set; } 
        public string Subject { get; set; } = string.Empty;
        public string Body { get; set; } = string.Empty;

    }
}
