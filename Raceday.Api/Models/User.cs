using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Raceday.Api.Models
{
    [Table("User")]
    public class User
    {
        [Key]
        public int userID { get; set; }
        [Required, MaxLength(50)]
        public string fullname { get; set; } = string.Empty;
        [Required, MaxLength(50)]
        public string surname { get; set; } = string.Empty;
        [Required, RegularExpression("Male|Female|Other")]
        public string gender { get; set; } = string.Empty;
        [Required,EmailAddress, MaxLength(100)]
        public string email { get; set; } = string.Empty;
        [Required, MaxLength(120)]
        public string user_password { get; set; } = string.Empty;
        [Required]
        public int phone_number { get; set; }
        [Required, RegularExpression("Organiser|Participant")]
        public string user_role { get; set; } = string.Empty;
        [Required, DataType(DataType.Date)]
        public DateTime date_of_birth { get; set; }
        public int? clubID { get; set; }
        [ForeignKey(nameof(clubID))]
        public Club? Club { get; set; }
        public ICollection<Event> Events { get; set; } = new List<Event>();
        public ICollection<Enrolment> Enrolments { get; set; } = new List<Enrolment>();

    }
}
