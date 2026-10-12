using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Raceday.Api.Models
{
    public class Event
    {
        [Key]
        public int eventID { get; set; }
        [Required]
        public int organiserID { get; set; }
        public User? Organiser { get; set; }
        [Required, MaxLength(100)]
        public string title { get; set; } = string.Empty;
        [Required,]
        public string description { get; set; } = string.Empty;
        [Required, RegularExpression("Running|Cycling|Walking|Multisport")]
        public string Eevent_type { get; set; } = string.Empty;
        [Required]
        public DateTime event_date { get; set; }
        [Required]
        public DateTime registration_deadline { get; set; }
        [Required, MaxLength(100)]
        public string venue { get; set; } = string.Empty;
        [Required, MaxLength(50)]
        public string city { get; set; } = string.Empty;
        [Required, MaxLength(50)]
        public string province { get; set; } = string.Empty;
        [Required, RegularExpression("Draft|Published|Cancelled")]
        public string status { get; set; } = "Draft";
        public DateTime created_at { get; set; } = DateTime.UtcNow;
        public ICollection<Category> Categories { get; set; } = new List<Category>();
    }
}
