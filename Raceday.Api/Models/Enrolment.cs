using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Raceday.Api.Models
{
    public class Enrolment
    {
        [Key]
        public int enrolmentID { get; set; }
        [Required]
        public int participantID { get; set; }
        public User? Participant { get; set; }
        [Required]
        public int categoryID { get; set; }
        public Category? Category { get; set; }
        [Required, MaxLength(20)]
        public string racer_number { get; set; } = string.Empty;
        [Required, DataType(DataType.Date)]
        public DateTime enrolment_date { get; set; }
        [Required, RegularExpression("Pending|Paid|Cancelled")]
        public string payment_status { get; set; } = "Pending";
        public ICollection<Result> Results { get; set; } = new List<Result>();
    }
}
