using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Raceday.Api.Models
{
    public class Event
    {
        [Key]
        public int EventId { get; set; }
        [Required]
        public int OrganiserId { get; set; }
        public User? Organiser { get; set; }
        [Required, MaxLength(100)]
        public string Title { get; set; } = string.Empty;
        [Required,]
        public string Description { get; set; } = string.Empty;
        [Required, RegularExpression("Running|Cycling|Walking|Multisport")]
        public string EventType { get; set; } = string.Empty;
        [Required]
        public DateTime EventDate { get; set; }
        [Required]
        public DateTime RegistrationDeadline { get; set; }
        [Required, MaxLength(100)]
        public string Venue { get; set; } = string.Empty;
        [Required, MaxLength(50)]
        public string City { get; set; } = string.Empty;
        [Required, MaxLength(50)]
        public string Province { get; set; } = string.Empty;
        [Required, RegularExpression("Draft|Published|Cancelled")]
        public string Status { get; set; } = "Draft";
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public ICollection<Category> Categories { get; set; } = new List<Category>();
    }
}
