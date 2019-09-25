using OnlineRecruitment.Models;
using OnlineRecruitment.Models.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace OnlineRecruitment.Controllers
{
    public class HomeController : Controller
    {
        [AllowAnonymous]
        public ActionResult Index()
        {
            GetJobList();
            GetCategory();
            return View();
        }

        
        [Authorize(Roles ="Employer")]
        public ActionResult EmployerProfile()
        {
            string usr = User.Identity.Name;
            int usrid = _db.tblUsers.Where(a => a.Username == usr).FirstOrDefault().UserId;
            tblEmployer tbe = _db.tblEmployers.Where(a => a.UserId == usrid).FirstOrDefault();
            EmployerViewModel evm = new EmployerViewModel();
            evm.EmployerId = tbe.EmployerId;
            evm.CompanyName = tbe.CompanyName;
            evm.CompanyDetails = tbe.CompanyDetails;
            evm.ContactPerson = tbe.ContactPerson;
            evm.Phone = tbe.Phone;
            evm.Email = tbe.Email;
            evm.CompanyWebsite = tbe.CompanyWebsite;
            evm.Location = tbe.Location;
            evm.Logo = tbe.Logo;
            evm.UserId = tbe.UserId;
            return View(evm);
        }
        [Authorize(Roles ="Admin")]
        public ActionResult AdminIndex()
        {
            return View();
        }
        [Authorize (Roles ="Employer")]
        public ActionResult EmployerIndex()
        {
            return View();
        }
        [Authorize(Roles = "Employer")]
        public ActionResult UpdateEmployer(int id)
        {
            tblEmployer tbe = _db.tblEmployers.Where(a => a.EmployerId == id).FirstOrDefault();
            EmployerViewModel evm = new EmployerViewModel();
            evm.EmployerId = tbe.EmployerId;
            evm.CompanyName = tbe.CompanyName;
            evm.CompanyDetails = tbe.CompanyDetails;
            evm.ContactPerson = tbe.ContactPerson;
            evm.Phone = tbe.Phone;
            evm.Email = tbe.Email;
            evm.CompanyWebsite = tbe.CompanyWebsite;
            evm.Location = tbe.Location;
            evm.Logo = tbe.Logo;
            evm.UserId = tbe.UserId;
            return View(evm);
        }
        [HttpPost]
        public ActionResult UpdateEmployer(EmployerViewModel evm)
        {
            
            tblEmployer tbe = _db.tblEmployers.Where(a => a.EmployerId == evm.EmployerId).FirstOrDefault();
            tbe.CompanyName = evm.CompanyName;
            tbe.ContactPerson = evm.ContactPerson;
            tbe.Phone = evm.Phone;
            tbe.Email = evm.Email;
            tbe.CompanyWebsite = evm.CompanyWebsite;
            tbe.Location = evm.Location;
            tbe.CompanyDetails = evm.CompanyDetails;
            HttpPostedFileBase fup = Request.Files["Logo"];
            if (fup != null)
            {
                if (fup.FileName != "")
                {
                    tbe.Logo = fup.FileName;
                    fup.SaveAs(Server.MapPath("~/CompanyLogo/" + fup.FileName));
                }
            }
            _db.SaveChanges();
            return RedirectToAction("EmployerProfile", "Home");

        }
        [Authorize(Roles = "JobSeeker")]
        public ActionResult JobSeekerIndex()
        {
            return View();
        }
        public ActionResult SignUp()
        {
            RefreshPageControl();  
            return View();
        }

        private ActionResult RefreshPageControl()
        {
            List<JobTypeViewModel> lst = new List<JobTypeViewModel>();
            lst.Add(new JobTypeViewModel() { JobTypeId = 1, JobTypeName = "Full Time" });
            lst.Add(new JobTypeViewModel() { JobTypeId = 2, JobTypeName = "Part Time" });
            lst.Add(new JobTypeViewModel() { JobTypeId = 3, JobTypeName = "Freelancer" });
            ViewBag.JobType = lst;

            List<ExpectedSalaryViewModel> lst1 = new List<ExpectedSalaryViewModel>();
            lst1.Add(new ExpectedSalaryViewModel() { ExpectedSalaryId = 1, ExpectedSalary = "10000-15000" });
            lst1.Add(new ExpectedSalaryViewModel() { ExpectedSalaryId = 2, ExpectedSalary = "15000-20000" });
            lst1.Add(new ExpectedSalaryViewModel() { ExpectedSalaryId = 3, ExpectedSalary = "20000-25000" });
            lst1.Add(new ExpectedSalaryViewModel() { ExpectedSalaryId = 4, ExpectedSalary = "25000-30000" });
            lst1.Add(new ExpectedSalaryViewModel() { ExpectedSalaryId = 5, ExpectedSalary = "30000-35000" });
            lst1.Add(new ExpectedSalaryViewModel() { ExpectedSalaryId = 6, ExpectedSalary = "35000-50000" });
            lst1.Add(new ExpectedSalaryViewModel() { ExpectedSalaryId = 7, ExpectedSalary = "50000-70000" });
            lst1.Add(new ExpectedSalaryViewModel() { ExpectedSalaryId = 8, ExpectedSalary = "Above 70000" });
            ViewBag.ExpectedSalary = lst1;
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

        OnlineRecruitmentDBEntities _db = new OnlineRecruitmentDBEntities();
        [HttpPost]
        public ActionResult SignUp(JobSeekerViewModel jsm)
        {
            List<tblUser> lst = _db.tblUsers.Where(u => u.Username == jsm.Username).ToList();
            if (lst.Count == 0)
            {
                if (jsm.Password != jsm.RetypePassword)
                {
                    ViewBag.Message = "Password Mismatch";
                }
                else
                {

                    string password = encryptpass(jsm.Password);
                    tblUser tbu = new tblUser();
                    tbu.Username = jsm.Username;
                    tbu.Password = password;
                    _db.tblUsers.Add(tbu);
                    _db.SaveChanges();

                    tblUserRole tbur = new tblUserRole();
                    tbur.UserId = tbu.UserId;
                    tbur.RoleId = 3;
                    _db.tblUserRoles.Add(tbur);
                    _db.SaveChanges();

                    tblJobSeeker tbj = new tblJobSeeker();
                    tbj.FullName = jsm.FullName;
                    tbj.Gender = jsm.Gender;
                    tbj.Email = jsm.Email;
                    tbj.Address = jsm.Address;
                    tbj.Skills = jsm.Skills;
                    tbj.JobType = jsm.JobType;
                    tbj.CurrentPosition = jsm.CurrentPosition;
                    tbj.HighestDegree = jsm.HighestDegree;
                    tbj.ExpectedSalary = jsm.ExpectedSalary;
                    tbj.Experience = jsm.Experience;
                    tbj.UserId = tbu.UserId;

                    HttpPostedFileBase fup = Request.Files["Resume"];
                    if(fup != null)
                    {
                        if (fup.FileName != "")
                        {
                            tbj.Resume = fup.FileName;
                            fup.SaveAs(Server.MapPath("~/ResumeUpload/" + fup.FileName));
                        }
                    }
                    _db.tblJobSeekers.Add(tbj);
                    _db.SaveChanges();
                    ViewBag.Message = "Your Account was created successfully";
                    RefreshPageControl();

                }
            }
            else
            {
                ViewBag.Message = "Username already exists!!!";
            }

            return View();
        }
        public ActionResult EmployerSignUp()
        {
          
            return View();
        }
        [HttpPost]
        public ActionResult EmployerSignUp(EmployerViewModel em)
        {
            if (ModelState.IsValid)
            {

                List<tblUser> lst = _db.tblUsers.Where(u => u.Username == em.Username).ToList();
                if (lst.Count == 0)
                {
                    if (em.Password != null) {

                        if (em.Password != em.RetypePassword)
                        {
                            ViewBag.Message = "Password Mismatch";
                        }
                        else
                        {

                            string password = encryptpass(em.Password);
                            tblUser tbu = new tblUser();
                            tbu.Username = em.Username;
                            tbu.Password = password;
                            _db.tblUsers.Add(tbu);
                            _db.SaveChanges();

                            tblUserRole tbur = new tblUserRole();
                            tbur.UserId = tbu.UserId;
                            tbur.RoleId = 2;
                            _db.tblUserRoles.Add(tbur);
                            _db.SaveChanges();

                            tblEmployer tbe = new tblEmployer();
                            tbe.CompanyName = em.CompanyName;
                            tbe.ContactPerson = em.ContactPerson;
                            tbe.Phone = em.Phone;
                            tbe.Email = em.Email;
                            tbe.CompanyWebsite = em.CompanyWebsite;
                            tbe.Location = em.Location;
                            tbe.CompanyDetails = em.CompanyDetails;
                            tbe.UserId = tbu.UserId;

                            HttpPostedFileBase fup = Request.Files["Logo"];
                            if (fup != null)
                            {
                                if (fup.FileName != "")
                                {
                                    tbe.Logo = fup.FileName;
                                    fup.SaveAs(Server.MapPath("~/CompanyLogo/" + fup.FileName));
                                }
                            }
                            _db.tblEmployers.Add(tbe);
                            _db.SaveChanges();
                            ViewBag.Message = "Your Account was created successfully";

                        }
                    }
                    else
                    {
                        ViewBag.Message = "Password Field is empty";
                    }
                }
                else
                {
                    ViewBag.Message = "Username already exists!!!";
                }
            }

            return View();
        }
       

        [Authorize (Roles = "Employer")]
        public ActionResult PostJob()
        {
            RefreshPageControl();
            GetCategory();
            return View();
        }

        private void GetCategory()
        {
            List<tblCategory> lstcat = _db.tblCategories.ToList();
            ViewBag.Category = lstcat;
        }

        [HttpPost]
        public ActionResult PostJob(JobViewModel jvm)
        {
            string user = User.Identity.Name;
            tblUser tb = _db.tblUsers.Where(a => a.Username == user).FirstOrDefault();
            tblEmployer tbe = _db.tblEmployers.Where(b => b.UserId == tb.UserId).FirstOrDefault();
            tblJob tbj = new tblJob();
            tbj.CategoryId = jvm.CategoryId;
            tbj.EmployerId = tbe.EmployerId;
            tbj.SalaryRange = jvm.SalaryRange;
            tbj.JobType = jvm.JobType;
            tbj.JobDetails = jvm.JobDetails;
            tbj.isFeaturedJob = jvm.isFeaturedJob;
            tbj.PostedDate = DateTime.Now;
            tbj.ExpiryDate = jvm.ExpiryDate;
            _db.tblJobs.Add(tbj);
            _db.SaveChanges();
            return RedirectToAction("Index", "Home");
        }

        

        private ActionResult GetJobList()
        {
            List<tblJob> job = _db.tblJobs.OrderByDescending(x=>x.JobId).ToList();
            List<JobViewModel> jvmlst = new List<JobViewModel>();
            foreach (var item in job)
            {
                JobViewModel jvm = new JobViewModel();
                jvm.CategoryId = item.CategoryId;
                jvm.CategoryName = _db.tblCategories.Where(a => a.CategoryId == item.CategoryId).FirstOrDefault().CategoryName;
                jvm.EmployerId = item.EmployerId;
                jvm.CompanyName = _db.tblEmployers.Where(a => a.EmployerId == item.EmployerId).FirstOrDefault().CompanyName;
                jvm.JobId = item.JobId;
                jvm.SalaryRange = item.SalaryRange;
                jvm.JobType = item.JobType;
                jvm.JobDetails = item.JobDetails;
                jvm.isFeaturedJob = item.isFeaturedJob;
                jvm.PostedDate = item.PostedDate;
                jvm.ExpiryDate = item.ExpiryDate;

                jvmlst.Add(jvm);
            }
            ViewBag.jvmlst = jvmlst;
            return View();
        }

        public PartialViewResult SearchJob(string txtCategory, string txtLocation)
        {
           List<JobViewModel> lst = (from j in _db.tblJobs
                                     join e in _db.tblEmployers
                                     on j.EmployerId equals e.EmployerId
                                     where j.JobDetails.Contains(txtCategory) && e.Location.Contains(txtLocation) 
                                     select new JobViewModel
                                     {
                                         CategoryId = j.CategoryId,
                                         CategoryName = _db.tblCategories.Where(a => a.CategoryId == j.CategoryId).FirstOrDefault().CategoryName,
                                         EmployerId = j.EmployerId,
                                         CompanyName =e.CompanyName,
                                         JobId = j.JobId,
                                         SalaryRange = j.SalaryRange,
                                         JobType = j.JobType,
                                         JobDetails = j.JobDetails,
                                         isFeaturedJob = j.isFeaturedJob,
                                         PostedDate = j.PostedDate,
                                         ExpiryDate = j.ExpiryDate,
                                         Location = e.Location
                                    }).ToList(); 
            return PartialView("_SearchJob", lst);
        }

        public ActionResult ManageJobs()
        {
            string usr = User.Identity.Name;
            int usrid = _db.tblUsers.Where(a => a.Username == usr).FirstOrDefault().UserId;
            int empid = _db.tblEmployers.Where(b => b.UserId == usrid).FirstOrDefault().EmployerId;
            List<tblJob> j = _db.tblJobs.Where(c => c.EmployerId == empid).ToList();
            List<JobViewModel> lst1 = new List<JobViewModel>();
            foreach(var item in j)
            {
                JobViewModel jvm = new JobViewModel();
                jvm.CategoryId = item.CategoryId;
                jvm.CategoryName = _db.tblCategories.Where(a => a.CategoryId == item.CategoryId).FirstOrDefault().CategoryName;
                jvm.EmployerId = item.EmployerId;
                jvm.JobId = item.JobId;
                jvm.CompanyName = _db.tblEmployers.Where(d=>d.EmployerId == item.EmployerId).FirstOrDefault().CompanyName;
                jvm.SalaryRange = item.SalaryRange;
                jvm.JobType = item.JobType;
                jvm.JobDetails = item.JobDetails;
                jvm.isFeaturedJob = item.isFeaturedJob;
                jvm.PostedDate = item.PostedDate;
                jvm.ExpiryDate = item.ExpiryDate;

                lst1.Add(jvm);

            }
            return View(lst1);
        }
      
    }

}