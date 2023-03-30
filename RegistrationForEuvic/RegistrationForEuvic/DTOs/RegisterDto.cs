using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using RegistrationForEuvic.Validators;

namespace RegistrationForEuvic.Models.DTOs
{
    public class RegisterDto
    {
        [Required]
        [NameLikeValue]
        [StringLength(50)]
        [Unicode(false)]
        public string Name { get; set; } = null!;

        [Required]
        [NameLikeValue]
        [StringLength(50)]
        [Unicode(false)]
        public string Surname { get; set; } = null!;
        [Required]
        [EmailAddress]
        [StringLength(320)]
        [Unicode(false)]
        public string Email { get; set; } = null!;
        [Required]
        [PeselValid]
        [StringLength(11)]
        [Unicode(false)]
        public string Pesel { get; set; } = null!;
        [Required]
        [StringLength(40)]
        [Unicode(false)]
        public string Password { get; set; } = null!;
        [Required]
        [PhoneValid]
        [StringLength(15)]
        [Unicode(false)]
        public string PhoneNumber { get; set; } = null!;
        public int? Age { get; set; }
        public double? PowerUsageAvg { get; set; }
    }
}
