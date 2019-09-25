using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OnlineRecruitment.Models.ViewModel
{
    public class EmployerViewModel
    {
        public Nullable<int> EmployerId { get; set; }
        public string CompanyName { get; set; }
        public string ContactPerson { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string CompanyWebsite { get; set; }
        public string Location { get; set; }
        public string CompanyDetails { get; set; }
        public Nullable<int> UserId { get; set; }
        public string Logo { get; set; }
        
        public string Username { get; set; }
        public string Password { get; set; }
        public string RetypePassword { get; set; }
    }
}