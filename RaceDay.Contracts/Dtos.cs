using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RaceDay.Contracts
{
    public class RegisterDto
    {
        public string fullname { get; set; } = string.Empty;
        public string surname { get; set; } = string.Empty;
        public string gender { get; set; } = string.Empty;
        public string email { get; set; } = string.Empty;
        public string user_password { get; set; } = string.Empty;
        public int phone_number { get; set; }
        public string user_role { get; set; } = string.Empty; // "Organiser" or "Participant"
        public DateTime date_of_birth { get; set; }
        public int? clubID { get; set; }
    }

    public class LoginDto
    {
      public string email { get; set; } = string.Empty;
      public string user_password { get; set; } = string.Empty;
    }

    public class CategoryDto
    {
        public string name { get; set; } = string.Empty;
        public decimal distance_KM { get; set; }
        public TimeSpan start_time { get; set; }
        public decimal entry_fee { get; set; }
        public string age_category { get; set; } = string.Empty;
        public int max_entries { get; set; }
    }

    public class EnrolmentDto
    {
       public int categoryID { get; set; }
       public string racer_number { get; set; } = string.Empty;
    }

    public class ResultDto
    {
        public int enrolmentID { get; set; }
        public TimeSpan finish_time { get; set; }
        public int? category_position { get; set; }
        public string status { get; set; } = "Finished"; // "Finished", "DNF", "DSQ"
    }
}
