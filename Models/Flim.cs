using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Numerics;

namespace Movie.Models
{
    public class Flim
    {
        [Key]
        public int FlimID { get; set; }
        public string FlimName { get; set; }
        public DateTime FlimReleaseDate { get; set; }
        public int DirectorID { get; set; }
        [ForeignKey("DirectorID")]
        public virtual Director Director{ get; set; }
        public int LanguageID { get; set; }
        [ForeignKey("LanguageID")]
        public virtual Language Language { get; set; }
        public int CountryID { get; set; }
        [ForeignKey("CountryID")]
        public virtual Country Country { get; set; }
        public int StudioID { get; set; }
        [ForeignKey("StudioID")]
        public virtual Studio Studio { get; set; }
        public string? FilmSynopsis { get; set; }
        public int FilmRunTimeMinutes { get; set; }
        public long CertificateID { get; set; }
        [ForeignKey("CertificateID")]
        public virtual Certificate Certificate { get; set; }
        public int FilmBudgetDollars { get; set; }
        public int FilmBoxOfficeDollars { get; set; }
        public int FilmOscarNominations { get; set; }
        public int FilmOscarWins { get; set; }
    }
}
