using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OnlineRecruitment.Models.ViewModel
{
    public class LoginViewModel
    {
        public int UserId { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public bool RememberMe { get; set; }
        public string NewPassword { get; set; }
        public string RetypePassword { get; set; }

    }
}