using System.ComponentModel.DataAnnotations;
using System.Numerics;

namespace Movie.Models
{
    public class Certificate
    {
        [Key]
        public long CertificateID { get; set; }
        public string? CertificateName { get; set; }
    }
}
