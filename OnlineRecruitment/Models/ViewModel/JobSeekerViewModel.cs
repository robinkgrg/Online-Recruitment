using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OnlineRecruitment.Models.ViewModel
{
    public class JobSeekerViewModel
    {
        public int JobSeekerId { get; set; }
        public string FullName { get; set; }
        public string Gender { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public string Skills { get; set; }
        public string JobType { get; set; }
        public string CurrentPosition { get; set; }
        public string HighestDegree { get; set; }
        public string ExpectedSalary { get; set; }
        public string Experience { get; set; }
        public string Resume { get; set; }
        public int UserId { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string RetypePassword { get; set; }
    }
}