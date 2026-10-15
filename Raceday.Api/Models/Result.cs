using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Raceday.Api.Models
{
    [Table("Result")]
    public class Result
    {
        [Key]
        public int resultID { get; set; }
        [Required]
        public int enrolmentID { get; set; }
        public Enrolment? Enrolment { get; set; }
        public TimeSpan? finish_time { get; set; }
        [Range(1, int.MaxValue)]
        public int? category_position { get; set; }
        [Required, RegularExpression("Finished|Did Not Finish|Did Not Start|Disqualified")]
        public string status { get; set; } = "Finished";
    }
}
