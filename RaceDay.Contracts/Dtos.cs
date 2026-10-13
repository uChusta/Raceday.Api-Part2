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
}
