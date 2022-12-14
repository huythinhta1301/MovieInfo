using System.ComponentModel.DataAnnotations;

namespace Movie.Models
{
    public class Country
    {
        [Key]
        public int CountryID { get; set; }
        public string? CountryName { get; set; }
    }
}
