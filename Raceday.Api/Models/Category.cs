using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Raceday.Api.Models
{
    public class Category
    {
        [Key]
        public int categoryID { get; set; }
        [Required]
        public int eventID { get; set; }
        public Event? Event { get; set; }
        [Required, MaxLength(60)]
        public string name { get; set; } = string.Empty;
        [Required, Column(TypeName = "decimal(6,2)")]
        public decimal distance_KM { get; set; }
        [Required]
        public TimeSpan start_time { get; set; }
        [Required, Column(TypeName = "decimal(8,2)")]
        public decimal entry_fee { get; set; }
        [Required, RegularExpression("Kids| Teenagers| Adults| Elders|No restriction")]
        public string age_category { get; set; } = string.Empty;
        [Required, Range(1, int.MaxValue)]
        public int max_entries { get; set; }
        public ICollection<Enrolment> Enrolments { get; set; } = new List<Enrolment>();
    }
}
