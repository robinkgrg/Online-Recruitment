using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OnlineRecruitment.Models.ViewModel
{
    public class JobViewModel
    {
        public int JobId { get; set; }
        public string CategoryName { get; set; }

        public string CompanyName { get; set; }

        public Nullable<int> CategoryId { get; set; }
        public Nullable<int> EmployerId { get; set; }
        public string SalaryRange { get; set; }
        public string JobType { get; set; }
        public string JobDetails { get; set; }
        public Nullable<bool> isFeaturedJob { get; set; }
        public Nullable<System.DateTime> PostedDate { get; set; }
        public Nullable<System.DateTime> ExpiryDate { get; set; }
        public string Location { get; set; }
    }
}