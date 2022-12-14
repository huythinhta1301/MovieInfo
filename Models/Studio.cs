using System.ComponentModel.DataAnnotations;

namespace Movie.Models
{
    public class Studio
    {
        [Key]
        public int StudioID { get; set; }
        public string? StudioName { get; set; }
    }
}
