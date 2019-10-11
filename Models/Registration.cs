using System;
using System.Collections.Generic;
using System.Text;

namespace IOTLabo3.Models
{
    public class Registration
    {
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public string Zipcode { get; set; }
        public string Age { get; set; }
        public string Email { get; set; }
        public bool IsFirstTimer { get; set; }
        public string RegistrationId { get; internal set; }
    }

}
