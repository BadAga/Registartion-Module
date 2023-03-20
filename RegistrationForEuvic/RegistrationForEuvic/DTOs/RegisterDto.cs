using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using RegistrationForEuvic.Models.Validators;

namespace RegistrationForEuvic.Models.DTOs
{
    public class RegisterDto
    {
        [Required(AllowEmptyStrings = false)]
        [StringLength(50)]
        [Unicode(false)]
        public string Name { get; set; } = null!;

        [Required]
        [StringLength(50)]
        [Unicode(false)]
        public string Surname { get; set; } = null!;
        [Required]
        [StringLength(320)]
        [Unicode(false)]
        public string Email { get; set; } = null!;
        [Required]
        [StringLength(11)]
        [Unicode(false)]
        public string Pesel { get; set; } = null!;
        [Required]
        [StringLength(40)]
        [Unicode(false)]
        public string Password { get; set; } = null!;
        [Required]
        [StringLength(15)]
        [Unicode(false)]
        public string PhoneNumber { get; set; } = null!;
        public int? Age { get; set; }
        public float? PowerUsageAvg { get; set; } = 0;
    }
}
