using System.ComponentModel.DataAnnotations;

namespace Movie.Models
{
    public class Language
    {
        [Key]
        public int LanguageID { get; set; }
        public string? LanguageName { get; set; }
    }
}
