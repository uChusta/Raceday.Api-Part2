using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Raceday.Api.Models
{
    public class User
    {
        [Key]
        public int Id { get; set; }
        [Required, MaxLength(50)]
        public string Fullname { get; set; } = string.Empty;
        [Required, MaxLength(50)]
        public string Surname { get; set; } = string.Empty;
        [Required, RegularExpression("Male|Female|Other")]
        public string Gender { get; set; } = string.Empty;
        [Required,EmailAddress, MaxLength(100)]
        public string Email { get; set; } = string.Empty;
        [Required, MaxLength(120)]
        public string UserPassword { get; set; } = string.Empty;
        [Required]
        public int PhoneNumber { get; set; }
        [Required, RegularExpression("Organiser|Participant")]
        public string UserRole { get; set; } = string.Empty;
        [Required, DataType(DataType.Date)]
        public DateTime DateOfBirth { get; set; }
        public int? ClubID { get; set; }
        public Club? Club { get; set; }

    }
}
