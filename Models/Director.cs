using System.ComponentModel.DataAnnotations;

namespace Movie.Models
{
    public class Director
    {
        [Key]
        public int DirectorID { get; set; }
        public string DirectorName { get; set; }
        public DateTime DirectorDOB { get; set; }
        public string DirectorGender { get; set; }
    }
}
