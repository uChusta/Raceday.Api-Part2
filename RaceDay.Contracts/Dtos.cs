using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;



namespace RaceDay.Contracts
{
    // DTO for user registration
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

    // DTO for user login
    public class LoginDto
    {
      public string email { get; set; } = string.Empty;
      public string user_password { get; set; } = string.Empty;
    }

    public class EventCreateDto
    {
        public string title { get; set; } = string.Empty;
        public string description { get; set; } = string.Empty;
        public string event_type { get; set; } = string.Empty; // "Running", "Cycling", "Walking", "Multisport"
        public DateTime event_date { get; set; }
        public DateTime registration_deadline { get; set; }
        public string venue { get; set; } = string.Empty;
        public string city { get; set; } = string.Empty;
        public string province { get; set; } = string.Empty;
        public string status { get; set; } = "Draft"; // "Draft", "Published","Completed", "Cancelled"
    }

    //DTO for Event update
    public class EventUpdateDto
    {
        public string title { get; set; } = string.Empty;
        public string description { get; set; } = string.Empty;
        public string event_type { get; set; } = string.Empty; // "Running", "Cycling", "Walking", "Multisport"
        public DateTime event_date { get; set; }
        public DateTime registration_deadline { get; set; }
        public string venue { get; set; } = string.Empty;
        public string city { get; set; } = string.Empty;
        public string province { get; set; } = string.Empty;
        public string status { get; set; } = "Draft"; // "Draft", "Published","Completed", "Cancelled"
    }

    // DTO for event category selection
    public class CategoryDto
    {
        public string name { get; set; } = string.Empty;
        public decimal distance_KM { get; set; }
        public TimeSpan start_time { get; set; }
        public decimal entry_fee { get; set; }
        public string age_category { get; set; } = string.Empty;
        public int max_entries { get; set; }
    }

    // DTO for enrolment in an event category
    public class EnrolmentDto
    {
       public int categoryID { get; set; }
       public string racer_number { get; set; } = string.Empty;
    }

    // DTO for race result submission
    public class ResultDto
    {
        public int enrolmentID { get; set; }
        public TimeSpan finish_time { get; set; }
        public int? category_position { get; set; }
        public string status { get; set; } = "Finished"; // "Finished", "DNF", "DSQ"
    }

    // DTO for club creation
    public class ClubDto
    {
        public string club_name { get; set; } = string.Empty;
        public string province { get; set; } = string.Empty;
        public string city { get; set; } = string.Empty;
        public string contact_email { get; set; } = string.Empty;
        public string sport_type { get; set; } = string.Empty; // "Running", "Cycling", "Walking", "Multisport"
    }
}
