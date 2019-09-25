using OnlineRecruitment.Models;
using OnlineRecruitment.Models.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace OnlineRecruitment.Controllers
{
    public class AccountController : Controller
    {
        // GET: Account
        public ActionResult Index()
        {
            return View();
        }

        OnlineRecruitmentDBEntities _db = new OnlineRecruitmentDBEntities();
        public ActionResult Login()
        {
            return View();
        }
        public string encryptpass(string password)
        {
            string msg = "";
            byte[] encode = new byte[password.Length];
            encode = Encoding.UTF8.GetBytes(password);
            msg = Convert.ToBase64String(encode);
            return msg;
        }

        public string username = "";
        [HttpPost]
        public ActionResult Login(LoginViewModel l, string ReturnUrl = "")
        {
            string password = encryptpass(l.Password);
            using(OnlineRecruitmentDBEntities db = new OnlineRecruitmentDBEntities())
            {
                var users = db.tblUsers.Where(a => a.Username == l.Username && a.Password == password).FirstOrDefault();
                if(users != null)
                {
                    username = l.Username;
                    FormsAuthentication.SetAuthCookie(l.Username, l.RememberMe);
                    if (Url.IsLocalUrl(ReturnUrl))
                    {
                        return Redirect(ReturnUrl);
                    }
                    else
                    {
                        return RedirectToAction("Index", "Home");
                    }
                }
                else
                {
                    var usrnme = db.tblUsers.Where(a => a.Username == l.Username).FirstOrDefault();
                    if (usrnme == null)
                    {
                        ModelState.AddModelError("", "Invalid User");
                    }
                    else
                    {
                        ModelState.AddModelError("", "Incorrect Password");
                    }
                    
                }
            }
            return View();
        }

        [Authorize]
        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Login", "Account");
        }
        public ActionResult ChangePassword()
        {
            return View();
        }
        [HttpPost]
        public ActionResult ChangePassword(LoginViewModel lvm)
        {
            string usrnme = User.Identity.Name;
            string passwd = encryptpass(lvm.Password);
            tblUser tbu = _db.tblUsers.Where(a => a.Username == usrnme).FirstOrDefault();
            if(passwd != tbu.Password)
            {
                ModelState.AddModelError ("","Incorrect Password");
            }
            else
            {
                if (lvm.NewPassword != lvm.RetypePassword)
                {
                    ModelState.AddModelError("", "new password and retyped password donot match");
                }
                else
                {
                    string newpass = encryptpass(lvm.NewPassword);
                    tbu.Password = newpass;
                    _db.SaveChanges();
                    if (User.IsInRole("Employer"))
                        return RedirectToAction("EmployerProfile", "Home");
                    if (User.IsInRole("JobSeeker"))
                        return RedirectToAction("JobSeekerProfile", "Home");
                }
            }
            return View();
        }
    }
}