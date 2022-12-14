using System.ComponentModel.DataAnnotations;

namespace Movie.Models
{
    public class Actor
    {
        [Key]
        public int ActorID { get; set; }
        public string ActorName { get; set; }
        public DateTime ActorDOB { get; set; }
        public string ActorGender { get; set; }

    }
}
