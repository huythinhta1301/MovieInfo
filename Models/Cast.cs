using System.ComponentModel.DataAnnotations;

namespace Movie.Models
{
    public class Cast
    {
        [Key]
        public int CastID { get; set; }
        public int CastFlimID { get; set; }
        public int CastActorID { get; set; }
        public string CastCharacterName { get; set; }

    }
}
