using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Raceday.Api.Models
{
    [Table("Club")]
    public class Club
    {
        [Key]
        public int clubID { get; set; }
        [Required, MaxLength(100)]
        public string club_name { get; set; } = string.Empty;
        [Required, MaxLength(50)]
        public string province { get; set; } = string.Empty;
        [Required, MaxLength(50)]
        public string city { get; set; } = string.Empty;
        [Required, EmailAddress, MaxLength(50)]
        public string contact_email { get; set; } = string.Empty;
        [Required,RegularExpression("Running|Cycling|Walking|Multisport")]
        public string sport_type { get; set; } = string.Empty;
        //navigation property
        public ICollection<User> Users { get; set; } = new List<User>();
    }
}
