using EnjoyOurTour.Helpers.Authorization;
using EnjoyOurTour.Helpers.Services;
using EnjoyOurTour.Models;
using EnjoyOurTour.Models.ViewModel.Admin;
using EnjoyOurTour.Models.ViewModel.Home;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using static EnjoyOurTour.Helpers.Authorization.TourAuthorizeAttribute;

namespace EnjoyOurTour.Controllers
{
    [TourAuthorize(Roles = "Admin, Superadmin")]
    public class AdminController : Controller
    {
        // GET: Admin
        public ActionResult Index()
        {
            return RedirectToAction("Customers", "Admin");
        }

        //public ActionResult Users()
        //{
        //    using (var db = new TourEntities())
        //    {
        //        var users = db.User.Select(e => new UserViewModel
        //        {
        //            UserId = e.UserId,
        //            Username = e.Username,
        //            EmailAddress = e.EmailAddress,
        //            RoleId = e.RoleId,
        //            RoleName = db.Role.Where(f => f.RoleId == e.RoleId).Select(f => f.RoleName).FirstOrDefault(),
        //        }).ToList();
        //        return View(users);
        //    }
        //}

        #region Customer
        public ActionResult Customers()
        {
            using (var db = new TourEntities())
            {
                var customers = (from e in db.Customer
                                 join u in db.User on e.UserId equals u.UserId
                                 join a in db.Agent on e.AgentId equals a.AgentId
                                 select new CustomerViewModel()
                                 {
                                     Username = u.Username,
                                     EmailAddress = u.EmailAddress,
                                     CustomerId = e.CustomerId,
                                     CustomerCode = e.CustomerCode,
                                     CustomerName = e.CustomerName,
                                     NRIC = e.NRIC,
                                     BankAccountNumber = e.BankAccountNumber,
                                     PhoneNumber = e.PhoneNumber,
                                     Address = e.Address,
                                     AvailableTVR = e.AvailableTVR,
                                     AvailableAmount = e.AvailableAmount,
                                     AvailablePoint = e.AvailablePoint,
                                     AgentId = e.AgentId,
                                     AgentName = a.AgentName,
                                     IntroducerId = e.IntroducerId,
                                     JoinDate = e.JoinDate,
                                     UserId = e.UserId,
                                 }).ToList();

                return View(customers);
            }
        }

        public ActionResult CustomersTestimony()
        {
            using (var db = new TourEntities())
            {
                var customers = (from e in db.CustomersTestimony
                                 select new CustomersTestimonyViewModel()
                                 {
                                     TestimonyId = e.TestimonyId,
                                     PhotoPath = e.PhotoPath,
                                     Description = e.Description,
                                     CreatedAt = e.CreatedAt,
                                     CreatedBy = e.CreatedBy,
                                     UpdatedAt = e.UpdatedAt,
                                     UpdatedBy = e.UpdatedBy

                                 }).ToList();

                return View(customers);
            }
        }


        public ActionResult AddCustomer()
        {
            return View(new AddCustomerViewModel());
        }

        public ActionResult EditCustomer(int customerId)
        {
            using (var db = new TourEntities())
            {
                var user = db.User_Customer_Adgent.Where(e => e.CustomerId == customerId).FirstOrDefault();
                if (user == null)
                {
                    return new HttpNotFoundResult("Customer not found");
                }
             
                return View(new AddCustomerViewModel()
                {
                    CustomerId = user.CustomerId,
                    Username = user.Username,
                    EmailAddress = user.EmailAddress,
                    CustomerName = user.CustomerName,
                    BankAccountNumber = user.BankAccountNumber,
                    Address = user.Address,
                    AvailableAmount = user.AvailableAmount,
                    AvailablePoint = user.AvailablePoint,
                    AvailableTVR = user.AvailableTVR,                    
                    AgentCode = user.AgentCode,
                    AgentName = user.AgentName,
                    JoinDate = user.JoinDate,
                    JoinDateString = user.JoinDate.ToString("yyyy-MM-dd HH:mm"),
                    NRIC = user.NRIC,
                    PhoneNumber = user.PhoneNumber,
                    UserId = user.UserId,
                    DateOfBirth = user.DateOfBirth,
                    DOBString = user.DateOfBirth.ToString("yyyy-MM-dd HH:mm"),
                    BankName = user.BankName,
                    IntroducerCode = user.IntroduceCode,
                    IntroducerName = user.IntroduceName
                });
            }
        }

        [HttpPost]
        public ActionResult EditCustomer(AddCustomerViewModel model)
        {
            using (var db = new TourEntities())
            {
                if (ModelState.IsValid)
                {
                    var agent = db.Agent.Where(e => e.AgentCode == model.AgentCode).FirstOrDefault();
                    if (agent == null)
                    {
                        ModelState.AddModelError("AgentCode", "Agent Code is not found.");
                        return View(model);
                    }

                    var isUsernameExist = db.User.Any(e => e.Username == model.Username && e.UserId != model.UserId);
                    if (isUsernameExist)
                    {
                        ModelState.AddModelError("Username", "Username is exist.");
                        return View(model);
                    }

                    var customer = db.Customer.Find(model.CustomerId);
                    if (customer == null)
                    {
                        return new HttpNotFoundResult("Customer not found.");
                    }

                    var user = db.User.Find(model.UserId);

                    user.Username = model.Username;
                    user.EmailAddress = model.EmailAddress;
                    customer.CustomerName = model.CustomerName;
                    customer.BankAccountNumber = model.BankAccountNumber;
                    customer.Address = model.Address;
                    customer.AvailableAmount = model.AvailableAmount;
                    customer.AvailablePoint = model.AvailablePoint;
                    customer.AvailableTVR = model.AvailableTVR;
                    customer.IntroducerId = model.IntroducerId;
                    customer.AgentId = agent.AgentId;
                    customer.JoinDate = model.JoinDate;
                    customer.NRIC = model.NRIC;
                    customer.PhoneNumber = model.PhoneNumber;
                    customer.UserId = user.UserId;
                    customer.BankName = model.BankName;
                    customer.DateOfBirth = model.DateOfBirth;

                    db.SaveChanges();
                    return RedirectToAction("Customers", "Admin");
                }
                return View(model);
            }
        }

        public ActionResult DeleteCustomer(int customerId)
        {
            using (var db = new TourEntities())
            {
                var customer = db.Customer.Find(customerId);
                var user = db.User.Find(customer.UserId);
                if (customer == null)
                {
                    return new HttpNotFoundResult("Customer not found.");
                }
                db.Customer.Remove(customer);
                db.User.Remove(user);
                db.SaveChanges();
                return RedirectToAction("Customers", "Admin");
            }
        }

        [HttpPost]
        public JsonResult MemberCashOut(int customerId)
        {
            using (var db = new TourEntities())
            {
                var customer = db.Customer.Where(x => x.CustomerId == customerId).FirstOrDefault();
                string errors = string.Empty;

                if (customer == null)
                {
                    errors = "Unable to find this Member.";
                }

                if (string.IsNullOrEmpty(errors))
                {
                    if (customer.AvailableAmount != 0)
                    {
                        decimal currentAmount = customer.AvailableAmount;
                        customer.AvailableAmount = 0;

                        IntroducerTransaction customerTransaction = new IntroducerTransaction();
                        customerTransaction.IntroducerId = customer.CustomerId;
                        customerTransaction.CreditAmount = 0;
                        customerTransaction.FinalAmount = 0;
                        customerTransaction.DebitAmount = currentAmount;
                        customerTransaction.CreditTVR = 0;
                        customerTransaction.FinalTVR = customer.AvailableTVR;
                        customerTransaction.DebitTVR = 0;
                        customerTransaction.CreditPoint = 0;
                        customerTransaction.FinalPoint = customer.AvailablePoint;
                        customerTransaction.DebitPoint = 0;
                        customerTransaction.CreatedAt = DateTime.Now;
                        customerTransaction.CreatedBy = MetadataServices.GetCurrentUser().Username;

                        db.IntroducerTransaction.Add(customerTransaction);

                        db.SaveChanges();
                    }
                

                    return Json(new { success = true }, JsonRequestBehavior.AllowGet);
                }
            
                else
                {
                    return Json(new { success = false, errors = errors }, JsonRequestBehavior.AllowGet);
                }                            
            }
        }

        public ActionResult AddCustomersTestimony()
        {
            return View(new AddCustomersTestimonyViewModel());
        }


        [HttpPost]
        public JsonResult AddCustomersTestimony(AddCustomersTestimonyViewModel model)
        {
            using (var db = new TourEntities())
            {
                if (ModelState.IsValid)
                {
                    var formsAuthentication = HttpContext.Request.Cookies[FormsAuthentication.FormsCookieName] != null
                    ? FormsAuthentication.Decrypt(
                        HttpContext.Request.Cookies[FormsAuthentication.FormsCookieName].Value)
                    : null;

                    string imageName = System.IO.Path.GetFileName(model.PhotoPath.FileName);
                    imageName = MetadataServices.GetDateTimeWithoutSlash() + "-" + imageName;
                    string physicalPath = Server.MapPath("~/Image/CustomersTestimony/" + imageName);
                    model.PhotoPath.SaveAs(physicalPath);

                    var newCustomersTestimony = new CustomersTestimony()
                    {
                        PhotoPath = imageName,
                        Description = model.Description,
                        YoutubeLink = model.YoutubeLink,
                        CreatedBy = MetadataServices.GetCurrentUser().Username,
                        CreatedAt = MetadataServices.GetCurrentDate(),
                        UpdatedBy = MetadataServices.GetCurrentUser().Username,
                        UpdatedAt = MetadataServices.GetCurrentDate()
                    };
                    db.CustomersTestimony.Add(newCustomersTestimony);
                    db.SaveChanges();
                }
                return Json(new { }, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult EditCustomersTestimony(int id)
        {
            using (var db = new TourEntities())
            {
                var testimony = db.CustomersTestimony.Where(e => e.TestimonyId == id).FirstOrDefault();

                if (testimony == null)
                {
                    return new HttpNotFoundResult("Testimony not found");
                }
                return View(new AddCustomersTestimonyViewModel()
                {
                    TestimonyId = testimony.TestimonyId,
                    PhotoPathUpload = testimony.PhotoPath,
                    Description = testimony.Description,
                    YoutubeLink = testimony.YoutubeLink,
                    UpdatedBy = testimony.UpdatedBy,
                    UpdatedAt = testimony.UpdatedAt
                });
            }
        }

        [HttpPost]
        public JsonResult EditCustomersTestimony(AddCustomersTestimonyViewModel model)
        {

            using (var db = new TourEntities())
            {
                var formsAuthentication = HttpContext.Request.Cookies[FormsAuthentication.FormsCookieName] != null
              ? FormsAuthentication.Decrypt(
                  HttpContext.Request.Cookies[FormsAuthentication.FormsCookieName].Value)
              : null;

                if (ModelState.IsValid)
                {
                    var testimony = db.CustomersTestimony.Find(model.TestimonyId);

                    if (model.PhotoPath != null)
                    {
                        FileInfo path = new FileInfo(Server.MapPath("~/Image/CustomersTestimony/" + testimony.PhotoPath));
                        path.Delete();

                        string imageName = System.IO.Path.GetFileName(model.PhotoPath.FileName);
                        imageName = MetadataServices.GetDateTimeWithoutSlash() + "-" + imageName;
                        string physicalPath = Server.MapPath("~/Image/CustomersTestimony/" + imageName);
                        model.PhotoPath.SaveAs(physicalPath);

                        testimony.PhotoPath = imageName;
                    }

                    testimony.Description = model.Description;
                    testimony.YoutubeLink = model.YoutubeLink;
                    testimony.UpdatedBy = MetadataServices.GetCurrentUser().Username;
                    testimony.UpdatedAt = MetadataServices.GetCurrentDate();

                    db.SaveChanges();

                }
                return Json(new { }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public async System.Threading.Tasks.Task<ActionResult> AddCustomer(AddCustomerViewModel model)
        {
            using (var db = new TourEntities())
            {
                if (ModelState.IsValid)
                {
                    var agent = db.Agent.Where(e => e.AgentCode == model.AgentCode).FirstOrDefault();
                    if (agent == null)
                    {
                        ModelState.AddModelError("AgentCode", "Agent Code is not found.");
                        return View(model);
                    }

                    var isUsernameExist = db.User.Any(e => e.Username == model.Username);
                    if (isUsernameExist)
                    {
                        ModelState.AddModelError("Username", "Username is exist.");
                        return View(model);
                    }
                    var password = MetadataServices.GenerateCode(8);
                    var user = new User()
                    {
                        Username = model.Username,
                        EmailAddress = model.EmailAddress,
                        Password = PasswordHash.CreateHash(password),
                        RoleId = TourRoleId.Customer,
                        CreatedAt = MetadataServices.GetCurrentDate(),
                        CreatedBy = MetadataServices.GetCurrentUser().Username,
                        UpdatedAt = MetadataServices.GetCurrentDate(),
                        UpdatedBy = MetadataServices.GetCurrentUser().Username,
                    };
                    db.User.Add(user);
                    db.SaveChanges();
                    var customer = new Customer()
                    {
                        CustomerName = model.CustomerName,
                        BankAccountNumber = model.BankAccountNumber,
                        Address = model.Address,
                        AvailableAmount = model.AvailableAmount,
                        AvailablePoint = model.AvailablePoint,
                        AvailableTVR = model.AvailableTVR,
                        IntroducerId = model.IntroducerId,
                        AgentId = agent.AgentId,
                        JoinDate = model.JoinDate,
                        DateOfBirth = model.DateOfBirth,
                        NRIC = model.NRIC,
                        PhoneNumber = model.PhoneNumber,
                        UserId = user.UserId,
                        CreatedAt = MetadataServices.GetCurrentDate(),
                        CreatedBy = MetadataServices.GetCurrentUser().Username,
                        UpdatedAt = MetadataServices.GetCurrentDate(),
                        UpdatedBy = MetadataServices.GetCurrentUser().Username,
                    };
                    db.Customer.Add(customer);
                    db.SaveChanges();
                    await MetadataServices.SendEmail(model.EmailAddress, "Enjoy Our Tour New Registration", "Hi " + model.Username + ", <br/> Thank you for registering in our platform. <br/> This is your password: " + password + ". Please go to our website login.");
                    return RedirectToAction("Customer", "Admin");
                }
                else
                {
                    return View(model);
                }
            }
        }

        public ActionResult UpdateAdminProfile()
        {
            using (var db = new TourEntities())
            {
                var userId = MetadataServices.GetCurrentUser().UserId;
                var admin = db.User.Where(e => e.UserId == userId).FirstOrDefault();

                if (admin == null)
                {
                    return new HttpNotFoundResult("Admin not found");
                }
                return View(new EditAdminViewModel()
                {
                    UserId = admin.UserId,
                    Username = admin.Username,
                    EmailAddress = admin.EmailAddress,
                    Password = string.Empty
                });
            }
        }


        [HttpPost]
        public JsonResult EditAdmin(EditAdminViewModel model)
        {

            using (var db = new TourEntities())
            {
                var formsAuthentication = HttpContext.Request.Cookies[FormsAuthentication.FormsCookieName] != null
              ? FormsAuthentication.Decrypt(
                  HttpContext.Request.Cookies[FormsAuthentication.FormsCookieName].Value)
              : null;

                if (ModelState.IsValid)
                {
                    var admin = db.User.Find(model.UserId);

                    admin.EmailAddress = model.EmailAddress;
                    admin.UpdatedAt = MetadataServices.GetCurrentDate();
                    admin.UpdatedBy = MetadataServices.GetCurrentUser().Username;

                    db.SaveChanges();

                }
                return Json(new { }, JsonRequestBehavior.AllowGet);
            }
        }



        public ActionResult DeleteCustomersTestimony(int id)
        {
            using (var db = new TourEntities())
            {
                if (ModelState.IsValid)
                {
                    var testimony = db.CustomersTestimony.Find(id);

                    if (testimony == null)
                    {
                        return new HttpNotFoundResult("Customer Testimony not found.");
                    }

                    FileInfo path = new FileInfo(Server.MapPath("~/Image/CustomersTestimony/" + testimony.PhotoPath));
                    path.Delete();

                    db.CustomersTestimony.Remove(testimony);
                    db.SaveChanges();
                    return RedirectToAction("CustomersTestimony", "Admin");
                }
                else
                {
                    return View();
                }
            }
        }



        #endregion

        #region Adgent

        public ActionResult Agent()
        {
            using (var db = new TourEntities())
            {
                var agents = (from data in db.User_Adgent
                              select new AgentViewModel()
                              {
                                  Username = data.Username,
                                  EmailAddress = data.EmailAddress,
                                  AgentId = data.AgentId,
                                  AgentCode = data.AgentCode,
                                  AgentName = data.AgentName,
                                  NRIC = data.NRIC,
                                  BankAccountNumber = data.BankAccountNumber,
                                  PhoneNumber = data.PhoneNumber,
                                  Address = data.Address,
                                  Commission = data.Commission,
                                  Bonus = data.Bonus,
                                  UserId = data.UserId,
                              }).ToList();

                return View(agents);
            }
        }


        public ActionResult AddAgent()
        {
            return View(new AddAgentViewModel());
        }

        [HttpPost]
        public async System.Threading.Tasks.Task<ActionResult> AddAgent(AddAgentViewModel model)
        {
            using (var db = new TourEntities())
            {
                if (ModelState.IsValid)
                {
                    var isUsernameExist = db.User.Any(e => e.Username == model.Username);
                    if (isUsernameExist)
                    {
                        ModelState.AddModelError("Username", "Username is exist.");
                        return View(model);
                    }
                    var password = MetadataServices.GenerateCode(8);
                    var user = new User()
                    {
                        Username = model.Username,
                        EmailAddress = model.EmailAddress,
                        Password = PasswordHash.CreateHash(password),
                        RoleId = TourRoleId.Customer,
                        CreatedAt = MetadataServices.GetCurrentDate(),
                        CreatedBy = MetadataServices.GetCurrentUser().Username,
                        UpdatedAt = MetadataServices.GetCurrentDate(),
                        UpdatedBy = MetadataServices.GetCurrentUser().Username,
                    };
                    db.User.Add(user);
                    db.SaveChanges();
                    var agent = new Agent()
                    {
                        AgentName = model.AgentName,
                        BankName = model.BankName,
                        BankAccountNumber = model.BankAccountNumber,
                        AgentCode = model.AgentCode,
                        Address = model.Address,
                        DOB = model.DOB,
                        Commission = model.Commission,
                        Bonus = model.Bonus,
                        NRIC = model.NRIC,
                        PhoneNumber = model.PhoneNumber,
                        UserId = user.UserId,
                        CreatedAt = MetadataServices.GetCurrentDate(),
                        CreatedBy = MetadataServices.GetCurrentUser().Username,
                        UpdatedAt = MetadataServices.GetCurrentDate(),
                        UpdatedBy = MetadataServices.GetCurrentUser().Username,
                    };
                    db.Agent.Add(agent);
                    db.SaveChanges();
                    await MetadataServices.SendEmail(model.EmailAddress, "Enjoy Our Tour New Registration", "Hi " + model.Username + ", <br/> Thank you for registering in our platform. <br/> This is your password: " + password + ". Please go to our website login.");
                    return RedirectToAction("Agent", "Admin");
                }
                else
                {
                    return View(model);
                }
            }
        }

        public ActionResult EditAgent(int agentId)
        {
            using (var db = new TourEntities())
            {
                var agent = db.Agent.Find(agentId);
                if (agent == null)
                {
                    return new HttpNotFoundResult("Agent not found");
                }
                var user = db.User.Where(e => e.UserId == agent.UserId).FirstOrDefault();
                return View(new AddAgentViewModel()
                {
                    AgentId = agent.AgentId,
                    Username = user.Username,
                    EmailAddress = user.EmailAddress,
                    DOB = agent.DOB,
                    DOBString = agent.DOB.ToString("yyyy-MM-dd HH:mm"),
                    AgentName = agent.AgentName,
                    BankName = agent.BankName,
                    BankAccountNumber = agent.BankAccountNumber,
                    Bonus = agent.Bonus,
                    Commission = agent.Commission,
                    Address = agent.Address,
                    AgentCode = agent.AgentCode,
                    NRIC = agent.NRIC,
                    PhoneNumber = agent.PhoneNumber,
                    UserId = user.UserId,
                });
            }
        }

        [HttpPost]
        public ActionResult EditAgent(AddAgentViewModel model)
        {
            using (var db = new TourEntities())
            {
                if (ModelState.IsValid)
                {

                    var agent = db.Agent.Find(model.AgentId);
                    if (agent == null)
                    {
                        return new HttpNotFoundResult("Agent not found.");
                    }

                    var user = db.User.Find(model.UserId);

                    user.Username = model.Username;
                    user.EmailAddress = model.EmailAddress;
                    agent.AgentName = model.AgentName;
                    agent.AgentCode = model.AgentCode;
                    agent.BankName = model.BankName;
                    agent.BankAccountNumber = model.BankAccountNumber;
                    agent.Address = model.Address;
                    agent.Commission = model.Commission;
                    agent.Bonus = model.Bonus;
                    agent.AgentId = agent.AgentId;
                    agent.DOB = model.DOB;
                    agent.NRIC = model.NRIC;
                    agent.PhoneNumber = model.PhoneNumber;
                    agent.UserId = user.UserId;

                    db.SaveChanges();
                    return RedirectToAction("Agent", "Admin");
                }
                return View(model);
            }
        }

        public ActionResult DeleteAgent(int agentId)
        {
            using (var db = new TourEntities())
            {
                var agent = db.Agent.Find(agentId);
                var user = db.User.Find(agent.UserId);
                if (agent == null)
                {
                    return new HttpNotFoundResult("Customer not found.");
                }
                db.Agent.Remove(agent);
                db.User.Remove(user);
                db.SaveChanges();
                return RedirectToAction("Agent", "Admin");
            }
        }

        #endregion
        public ActionResult Banners()
        {
            using (var db = new TourEntities())
            {
                List<BannerViewModel> banner = (from data in db.Banner
                                                where data.IsActive
                                                select new BannerViewModel()
                                                {
                                                    BannerId = data.BannerId,
                                                    BannerDescription = data.BannerDescription,
                                                    BannerImage = data.BannerImage,
                                                    DateAdded = data.CreatedAt.ToString(),
                                                    CreatedAt = data.CreatedAt,
                                                    IsActive = data.IsActive,
                                                }).ToList();
                foreach (var item in banner)
                {
                    item.DateAdded = String.Format("{0:dd-MM-yyyy}", Convert.ToDateTime(item.DateAdded));
                    if (item.IsActive == true)
                    {
                        item.Status = "Active";
                    }
                    else
                    {
                        item.Status = "De-Active";
                    }
                }
                return View(banner);
            }
        }

        public ActionResult AddBanner()
        {
            return View();
        }

        [HttpPost]
        public JsonResult AddBanner(BannerViewModel model)
        {
            using (var db = new TourEntities())
            {
                if (ModelState.IsValid)
                {
                    var formsAuthentication = HttpContext.Request.Cookies[FormsAuthentication.FormsCookieName] != null
                    ? FormsAuthentication.Decrypt(
                        HttpContext.Request.Cookies[FormsAuthentication.FormsCookieName].Value)
                    : null;
                    Banner banner = new Banner();
                    if (model.BannerDescription == null)
                    {
                        banner.BannerDescription = "";
                    }
                    else
                    {
                        banner.BannerDescription = model.BannerDescription;
                    }
                    string imageName = System.IO.Path.GetFileName(model.Image.FileName);
                    imageName = MetadataServices.GetDateTimeWithoutSlash() + "-" + imageName;
                    string physicalPath = Server.MapPath("~/Image/BannerImage/" + imageName);
                    model.Image.SaveAs(physicalPath);


                    banner.BannerImage = imageName;
                    banner.IsActive = Convert.ToBoolean(model.IsActive);
                    banner.BannerName = model.Image.FileName;
                    banner.CreatedAt = MetadataServices.GetCurrentDate();
                    banner.CreatedBy = MetadataServices.GetCurrentUser().Username;
                    banner.UpdatedAt = MetadataServices.GetCurrentDate();
                    banner.UpdatedBy = MetadataServices.GetCurrentUser().Username;
                    db.Banner.Add(banner);
                    db.SaveChanges();
                }
                return Json(new { }, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult EditBanner(int id)
        {

            using (var db = new TourEntities())
            {
                var banner = db.Banner.Where(e => e.BannerId == id).FirstOrDefault();

                if (banner == null)
                {
                    return new HttpNotFoundResult("Banner not found");
                }
                return View(new BannerViewModel()
                {
                    BannerId = banner.BannerId,
                    BannerDescription = banner.BannerDescription,
                    BannerImage = banner.BannerImage,
                    IsActive = banner.IsActive
                });
            }
        }

        [HttpPost]
        public JsonResult EditBanner(BannerViewModel model)
        {
            try
            {
                using (var db = new TourEntities())
                {
                    if (ModelState.IsValid)
                    {
                        var formsAuthentication = HttpContext.Request.Cookies[FormsAuthentication.FormsCookieName] != null
                        ? FormsAuthentication.Decrypt(
                            HttpContext.Request.Cookies[FormsAuthentication.FormsCookieName].Value)
                        : null;
                        Banner banner = db.Banner.Where(e => e.BannerId == model.BannerId).FirstOrDefault();

                        if (model.BannerDescription == null)
                        {
                            banner.BannerDescription = "";
                        }
                        else
                        {
                            banner.BannerDescription = model.BannerDescription; ;
                        }


                        if (model.Image != null)
                        {
                            FileInfo path = new FileInfo(Server.MapPath("~/Image/BannerImage/" + banner.BannerImage));
                            path.Delete();

                            string imageName = System.IO.Path.GetFileName(model.Image.FileName);
                            imageName = MetadataServices.GetDateTimeWithoutSlash() + "-" + imageName;
                            string physicalPath = Server.MapPath("~/Image/BannerImage/" + imageName);
                            model.Image.SaveAs(physicalPath);
                            banner.BannerImage = imageName;
                            banner.BannerName = model.Image.FileName;
                        }



                        banner.IsActive = Convert.ToBoolean(model.IsActive);
                        banner.UpdatedAt = MetadataServices.GetCurrentDate();
                        banner.UpdatedBy = MetadataServices.GetCurrentUser().Username;

                        db.SaveChanges();
                    }
                    return Json(new { }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public ActionResult DeleteBanner(int id)
        {

            using (var db = new TourEntities())
            {
                var banner = db.Banner.Where(e => e.BannerId == id).FirstOrDefault();

                if (banner == null)
                {
                    return new HttpNotFoundResult("Banner not found");
                }
                else
                {
                    FileInfo path = new FileInfo(Server.MapPath("~/Image/BannerImage/" + banner.BannerImage));
                    path.Delete();

                    db.Banner.Remove(banner);
                    db.SaveChanges();

                }
                List<BannerViewModel> bannerViewModel = (from data in db.Banner
                                                         where data.IsActive
                                                         select new BannerViewModel()
                                                         {
                                                             BannerId = data.BannerId,
                                                             BannerDescription = data.BannerDescription,
                                                             BannerImage = data.BannerImage,
                                                             DateAdded = data.CreatedAt.ToString(),
                                                             CreatedAt = data.CreatedAt,
                                                             IsActive = data.IsActive,
                                                         }).ToList();
                foreach (var item in bannerViewModel)
                {
                    item.DateAdded = String.Format("{0:dd-MM-yyyy}", Convert.ToDateTime(item.DateAdded));
                    if (item.IsActive == true)
                    {
                        item.Status = "Active";
                    }
                    else
                    {
                        item.Status = "De-Active";
                    }
                }
                return View("Banners", bannerViewModel);

            }
        }

        public ActionResult SystemSetting()
        {
            using (var db = new TourEntities())
            {
                var systemSetting = db.SystemSetting.OrderByDescending(x => x.CreadtedDate).FirstOrDefault();

                if (systemSetting == null)
                {
                    return View(new SystemSettingViewModel()
                    {
                        MinAmount = 0,
                        SignUpFees = 0,
                        TVRMutiply = 0,
                        IntroduceIncreaseLimit = 0,
                        AgentCommision = 0,
                        IntroduceNormalCommision = 0,
                        IntroduceIncreaseCommision = 0,
                        AgentBonusCalculateFormula = 0,
                        MemberPointCalculateFormula = 0,
                        IntroduceMaxLimits = 0,
                        AgentTopUpCommision = 0,
                        ProcessingFeesCommision = 0

                    });
                }
                else
                {
                    return View(new SystemSettingViewModel()
                    {
                        MinAmount = systemSetting.MinAmount,
                        SignUpFees = systemSetting.SignUpFees,
                        TVRMutiply = systemSetting.TVRMutiply,
                        IntroduceIncreaseLimit = systemSetting.IntroduceIncreaseLimit,
                        AgentCommision = systemSetting.AgentCommision,
                        IntroduceNormalCommision = systemSetting.IntroduceNormalCommision,
                        IntroduceIncreaseCommision = systemSetting.IntroduceIncreaseCommision,
                        AgentBonusCalculateFormula = systemSetting.AgentCalculationBonus,
                        MemberPointCalculateFormula = systemSetting.PointCalculation,
                        IntroduceMaxLimits = systemSetting.IntroduceMaxLimit,
                        AgentTopUpCommision = systemSetting.AgentTopUpCommission,
                        ProcessingFeesCommision = systemSetting.ProcessingFeesCommision

                    });
                }
            }
        }

        [HttpPost]
        public ActionResult SystemSetting(SystemSettingViewModel model)
        {
            using (var db = new TourEntities())
            {
                if (ModelState.IsValid)
                {

                    var systemSetting = new SystemSetting()
                    {
                        MinAmount = model.MinAmount,
                        TVRMutiply = model.TVRMutiply,
                        SignUpFees = model.SignUpFees,
                        IntroduceIncreaseCommision = model.IntroduceIncreaseCommision,
                        IntroduceMaxLimit = model.IntroduceMaxLimits,
                        IntroduceIncreaseLimit = model.IntroduceIncreaseLimit,
                        IntroduceNormalCommision = model.IntroduceNormalCommision,
                        AgentCommision = model.AgentCommision,
                        AgentCalculationBonus = model.AgentBonusCalculateFormula,
                        PointCalculation = model.MemberPointCalculateFormula,
                        AgentTopUpCommission = model.AgentTopUpCommision,
                        ProcessingFeesCommision = model.ProcessingFeesCommision,
                        CreadtedDate = DateTime.Now,
                        CreatedBY = MetadataServices.GetCurrentUser().Username
                    };
                    db.SystemSetting.Add(systemSetting);
                    db.SaveChanges();
                    return RedirectToAction("Customers", "Admin");
                }
                return View(model);
            }
        }

        public ActionResult AboutUs()
        {
            using (var db = new TourEntities())
            {
                var aboutUs = db.AboutUs.FirstOrDefault();
                if (aboutUs != null)
                {
                    return View(new AboutUsViewModel()
                    {
                        AboutUsId = aboutUs.AboutUsId,
                        AboutUsTitle = System.Security.SecurityElement.Escape(aboutUs.AboutUsTitle),
                        AboutUsDescription = System.Security.SecurityElement.Escape(aboutUs.AboutUsDescription),
                        AboutUsShortDescription = System.Security.SecurityElement.Escape(aboutUs.AboutUsShortDescription),
                        YoutubeUrl = aboutUs.YoutubeLink,
                    });
                }
                else
                {
                    return View(new AboutUsViewModel()
                    {
                        AboutUsId = 0,
                        AboutUsTitle = "",
                        AboutUsDescription = "",
                        AboutUsShortDescription = "",
                        YoutubeUrl = "",
                    });
                }
            }
        }

        [HttpPost]
        public JsonResult AboutUs(AboutUsViewModel model)
        {
            if (ModelState.IsValid)
            {
                using (var db = new TourEntities())
                {
                    var formsAuthentication = HttpContext.Request.Cookies[FormsAuthentication.FormsCookieName] != null
                        ? FormsAuthentication.Decrypt(
                            HttpContext.Request.Cookies[FormsAuthentication.FormsCookieName].Value)
                        : null;

                    var aboutUs = db.AboutUs.FirstOrDefault(e => e.AboutUsId == model.AboutUsId);
                    if (aboutUs != null)
                    {
                        aboutUs.AboutUsTitle = model.AboutUsTitle;
                        aboutUs.AboutUsDescription = model.AboutUsDescription;
                        aboutUs.AboutUsShortDescription = model.AboutUsShortDescription;
                        aboutUs.UpdatedBy = MetadataServices.GetUsername(formsAuthentication.Name ?? "");
                        aboutUs.UpdatedAt = MetadataServices.GetCurrentDate();
                        aboutUs.YoutubeLink = model.YoutubeUrl;

                        db.SaveChanges();
                    }
                    else
                    {
                        var newAboutUs = new AboutUs()
                        {
                            AboutUsTitle = model.AboutUsTitle,
                            AboutUsDescription = model.AboutUsDescription,
                            AboutUsShortDescription = model.AboutUsShortDescription,
                            UpdatedBy = MetadataServices.GetUsername(formsAuthentication.Name ?? ""),
                            UpdatedAt = MetadataServices.GetCurrentDate(),
                            YoutubeLink = model.YoutubeUrl,
                        };
                        db.AboutUs.Add(newAboutUs);
                        db.SaveChanges();
                    }
                    return Json(new { result = "success", message = "" }, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                return Json(new { result = "failed", Errors = ModelState.Errors() }, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult ContactUs()
        {
            using (var db = new TourEntities())
            {
                var contactUs = db.ContactUs.Select(e => new ContactUsViewModel()
                {
                    ContactUsTitle = e.ContactUsTitle,
                    ContactUsId = e.ContactUsId,
                    CompanyName = e.CompanyName,
                    CompanyAddress = e.CompanyAddress,
                    CompanyPhoneNumber = e.CompanyPhoneNumber,
                    CompanyLatitude = e.CompanyLatitude,
                    CompanyLongitude = e.CompanyLongitude,
                    IsContactExist = db.ContactUs.Any(),
                }).FirstOrDefault();
                return View(contactUs);
            }
        }

        [HttpPost]
        public JsonResult ContactUs(ContactUsViewModel model)
        {
            try
            {
                using (var db = new TourEntities())
                {
                    if (ModelState.IsValid)
                    {
                        var formsAuthentication = HttpContext.Request.Cookies[FormsAuthentication.FormsCookieName] != null
                            ? FormsAuthentication.Decrypt(
                                HttpContext.Request.Cookies[FormsAuthentication.FormsCookieName].Value)
                            : null;
                        var contactUs = db.ContactUs.FirstOrDefault();

                        if (contactUs != null) //there's an exist contact us data in db
                        {
                            contactUs.ContactUsTitle = model.ContactUsTitle;
                            contactUs.CompanyName = model.CompanyName;
                            contactUs.CompanyAddress = model.CompanyAddress;
                            contactUs.CompanyPhoneNumber = model.CompanyPhoneNumber;
                            contactUs.CompanyLatitude = contactUs.CompanyLatitude;
                            contactUs.CompanyLongitude = contactUs.CompanyLongitude;
                            contactUs.UpdatedBy = formsAuthentication.Name ?? "";
                            contactUs.UpdatedAt = MetadataServices.GetCurrentDate();
                            db.SaveChanges();
                        }
                        else
                        {
                            var newContact = new ContactUs()
                            {
                                ContactUsTitle = model.ContactUsTitle,
                                CompanyName = model.CompanyName,
                                CompanyAddress = model.CompanyAddress,
                                CompanyPhoneNumber = model.CompanyPhoneNumber,
                                CompanyLatitude = model.CompanyLatitude,
                                CompanyLongitude = model.CompanyLongitude,
                                UpdatedBy = formsAuthentication.Name ?? "",
                                UpdatedAt = MetadataServices.GetCurrentDate(),
                            };
                            db.ContactUs.Add(newContact);
                            db.SaveChanges();
                        }

                        return Json(new { result = "success" }, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        var contact = db.ContactUs.Select(e => new ContactUsViewModel()
                        {
                            ContactUsTitle = e.ContactUsTitle,
                            ContactUsId = e.ContactUsId,
                            CompanyName = e.CompanyName,
                            CompanyAddress = e.CompanyAddress,
                            CompanyPhoneNumber = e.CompanyPhoneNumber,
                            CompanyLatitude = e.CompanyLatitude,
                            CompanyLongitude = e.CompanyLongitude,
                        }).FirstOrDefault();
                        return Json(new { result = "success" }, JsonRequestBehavior.AllowGet);
                    }
                }
            }
            catch (DbEntityValidationException e)
            {
                foreach (var eve in e.EntityValidationErrors)
                {
                    Console.WriteLine("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                        eve.Entry.Entity.GetType().Name, eve.Entry.State);
                    foreach (var ve in eve.ValidationErrors)
                    {
                        Console.WriteLine("- Property: \"{0}\", Error: \"{1}\"",
                            ve.PropertyName, ve.ErrorMessage);
                    }
                }
                throw;
            }

        }


        public ActionResult Transaction()
        {
            using (var db = new TourEntities())
            {
                List<TransactionViewModel> transaction = (from data in db.TransactionView
                                                          select new TransactionViewModel()
                                                          {
                                                              TransactionId = data.TransactionId,
                                                              AgentCodeName = data.AgentCodeName,
                                                              CustomerCodeName = data.CustomerCodeName,
                                                              IntroducerCodeName = data.IntroducerCodeName,
                                                              ReferenceNo = data.ReferenceNo,
                                                              PathForProof = (string.IsNullOrEmpty(data.PathForProof) ? "No Image Upload" : data.PathForProof),
                                                              Amount = data.Amount,
                                                              CurrentTVR = data.CurrentTVR,
                                                              TopUpTVR = data.TopUpTVR,
                                                              RedeemTVR = data.RedeemTVR,
                                                              BalanceTVR = data.BalanceTVR,
                                                              CurrentPoint = data.CurrentPoint,
                                                              PointAdd = data.PointAdd,
                                                              PointRedeem = data.PointRedeem,
                                                              PointBalance = data.PointBalance,
                                                              Remarks = data.Remarks,
                                                              ActivityName = data.ActivityName,
                                                              TransactionStatusName = data.StatusName,
                                                              CreatedAt = data.CreatedAt
                                                          }).ToList();
                return View(transaction);
            }
        }

        public ActionResult UpdateTransaction(int transactionId)
        {
            using (var db = new TourEntities())
            {
                TransactionViewModel transaction = (from data in db.TransactionView
                                                    select new TransactionViewModel()
                                                    {
                                                        TransactionId = data.TransactionId,
                                                        AgentCodeName = data.AgentCodeName,
                                                        CustomerCodeName = data.CustomerCodeName,
                                                        IntroducerCodeName = data.IntroducerCodeName,
                                                        ReferenceNo = data.ReferenceNo,
                                                        PathForProof = (string.IsNullOrEmpty(data.PathForProof) ? "No Image Upload" : data.PathForProof),
                                                        Amount = data.Amount,
                                                        CurrentTVR = data.CurrentTVR,
                                                        TopUpTVR = data.TopUpTVR,
                                                        RedeemTVR = data.RedeemTVR,
                                                        BalanceTVR = data.BalanceTVR,
                                                        CurrentPoint = data.CurrentPoint,
                                                        PointAdd = data.PointAdd,
                                                        PointRedeem = data.PointRedeem,
                                                        PointBalance = data.PointBalance,
                                                        Remarks = data.Remarks,
                                                        ActivityName = data.ActivityName,
                                                        ActivityId = data.ActivityId,
                                                        PackageName = data.PackageName,
                                                        ProductName = data.ProductRedeemName,
                                                        HeadCount = (data.ActivityId == (int)EnjoyOurTour.Helpers.TransactionActivity.RedeemPackage ? data.TravelHeadCount : null),
                                                        TransactionStatus = data.StatusId,
                                                        TransactionStatusName = data.StatusName,
                                                        CreatedAt = data.CreatedAt
                                                    }).Where(x => x.TransactionId == transactionId).FirstOrDefault();
                return View(transaction);
            }
        }

        [HttpPost]
        public async System.Threading.Tasks.Task<ActionResult> UpdateTransaction(TransactionViewModel transactionViewModel, int action)
        {
            using (var db = new TourEntities())
            {
                if (transactionViewModel.ActivityId == (int)EnjoyOurTour.Helpers.TransactionActivity.SignUp && (action == (int)EnjoyOurTour.Helpers.TransactionStatus.Approve || action == (int)EnjoyOurTour.Helpers.TransactionStatus.ApproveWithoutPay))
                {
                    #region SignUpModel
                    var transactionModel = db.UserTransaction.Where(x => x.TransactionId == transactionViewModel.TransactionId).FirstOrDefault();

                    if (transactionModel.TransactionStatus == (int)EnjoyOurTour.Helpers.TransactionStatus.ApproveWithoutPay)
                    {
                        transactionModel.TransactionStatus = (int)EnjoyOurTour.Helpers.TransactionStatus.Approve;                        
                        transactionModel.UpdatedAt = DateTime.Now;
                        transactionModel.UpdatedBy = MetadataServices.GetCurrentUser().Username;

                        db.SaveChanges();
                    }
                    else
                    {
                        var systemSetting = db.SystemSetting.OrderByDescending(x => x.CreadtedDate).FirstOrDefault();

                        transactionModel.TransactionStatus = action;
                        transactionModel.Remarks = transactionViewModel.Remarks;
                        transactionModel.UpdatedAt = DateTime.Now;
                        transactionModel.UpdatedBy = MetadataServices.GetCurrentUser().Username;

                        var customer = db.Customer.Where(x => x.CustomerId == transactionModel.CustomerId).FirstOrDefault();
                        int finalTVR = customer.AvailableTVR + transactionModel.TopUpTVR;
                        int finalPoint = customer.AvailablePoint + transactionModel.PointAdd;

                        var user = MetadataServices.GetCurrentUser();

                        user.IsDeleted = false;
                        user.IsActive = true;

                        customer.AvailableTVR = finalTVR;
                        customer.AvailablePoint = finalPoint;
                        customer.IsDeleted = false;
                        customer.UpdatedAt = DateTime.Now;
                        customer.UpdatedBy = user.Username;
                        customer.JoinDate = DateTime.Now;
                        customer.ExpiredDate = DateTime.Now.AddYears(1);
                        customer.RenewDate = DateTime.Now;
                        customer.UpdatedBy = user.Username;
                        customer.UpdatedAt = DateTime.Now;

                        IntroducerTransaction customerTransaction = new IntroducerTransaction();
                        customerTransaction.IntroducerId = customer.CustomerId;
                        customerTransaction.CreditTVR = transactionModel.TopUpTVR;
                        customerTransaction.FinalTVR = finalTVR;
                        customerTransaction.DebitTVR = 0;
                        customerTransaction.CreditAmount = 0;
                        customerTransaction.FinalAmount = 0;
                        customerTransaction.DebitAmount = 0;
                        customerTransaction.CreditPoint = transactionModel.PointAdd;
                        customerTransaction.FinalPoint = finalPoint;
                        customerTransaction.DebitPoint = 0;
                        customerTransaction.CreatedBy = user.Username;
                        customerTransaction.CreatedAt = DateTime.Now;

                        db.IntroducerTransaction.Add(customerTransaction);

                        db.SaveChanges();

                        var agent = db.Agent.Where(x => x.AgentId == transactionModel.AgentId).FirstOrDefault();

                        if (transactionModel.IntroducerId != 0 && transactionModel.IntroducerId != null)
                        {
                            var introducer = db.Customer.Where(x => x.CustomerId == transactionModel.IntroducerId).FirstOrDefault();

                            decimal introducerComission;

                            if (systemSetting.IntroduceIncreaseLimit >= introducer.IntroduceTimes)
                            {
                                introducerComission = systemSetting.IntroduceNormalCommision;
                            }
                            else
                            {
                                if (introducer.IntroduceTimes <= systemSetting.IntroduceMaxLimit)
                                {
                                    introducerComission = ((introducer.IntroduceTimes - systemSetting.IntroduceIncreaseLimit + 1) * systemSetting.IntroduceIncreaseCommision) + systemSetting.IntroduceNormalCommision;
                                }
                                else
                                {
                                    introducerComission = ((systemSetting.IntroduceMaxLimit - systemSetting.IntroduceIncreaseLimit + 1) * systemSetting.IntroduceIncreaseCommision) + systemSetting.IntroduceNormalCommision;
                                }

                            }

                            decimal introduceramount = systemSetting.IntroduceNormalCommision + introducerComission;

                            decimal introducerFinalAmount = introducer.AvailableAmount + introduceramount;

                            introducer.AvailableAmount = introducerFinalAmount;
                            introducer.UpdatedAt = DateTime.Now;
                            introducer.UpdatedBy = MetadataServices.GetCurrentUser().Username;

                            IntroducerTransaction introducerTransaction = new IntroducerTransaction();
                            introducerTransaction.IntroducerId = introducer.CustomerId;
                            introducerTransaction.CreditAmount = introduceramount;
                            introducerTransaction.FinalAmount = introducerFinalAmount;
                            introducerTransaction.DebitAmount = 0;
                            introducerTransaction.CreditTVR = 0;
                            introducerTransaction.FinalTVR = introducer.AvailableTVR;
                            introducerTransaction.DebitTVR = 0;
                            introducerTransaction.CreditPoint = 0;
                            introducerTransaction.FinalPoint = introducer.AvailablePoint;
                            introducerTransaction.DebitPoint = 0;
                            introducerTransaction.CreatedAt = DateTime.Now;
                            introducerTransaction.CreatedBy = MetadataServices.GetCurrentUser().Username;

                            db.IntroducerTransaction.Add(introducerTransaction);

                            decimal agentCommision = ((((int)transactionModel.Amount / systemSetting.MinAmount) * systemSetting.AgentCommision) + systemSetting.ProcessingFeesCommision) - introduceramount;
                            decimal agentBonus = (((int)transactionModel.Amount / systemSetting.MinAmount) * systemSetting.AgentCalculationBonus);

                            agent.Commission = agent.Commission + agentCommision;
                            agent.Bonus = agent.Bonus + agentBonus;
                            agent.UpdatedAt = DateTime.Now;
                            agent.UpdatedBy = MetadataServices.GetCurrentUser().Username;

                            AgentTransaction agentTransaction = new AgentTransaction();

                            agentTransaction.AgentId = agent.AgentId;
                            agentTransaction.CreditAmount = agentCommision;
                            agentTransaction.FinalAmount = agent.Commission;
                            agentTransaction.CreditBonus = agentBonus;
                            agentTransaction.FinalBonus = agent.Bonus;
                            agentTransaction.CreatedAt = DateTime.Now;
                            agentTransaction.CreatedBy = MetadataServices.GetCurrentUser().Username;

                            db.AgentTransaction.Add(agentTransaction);

                            db.SaveChanges();


                        }
                        else
                        {
                            decimal agentBonus = ((((int)transactionModel.Amount / systemSetting.MinAmount) * systemSetting.AgentCommision) + systemSetting.ProcessingFeesCommision);

                            decimal agentFinalComission = agent.Commission + (systemSetting.AgentCommision);
                            agent.Commission = agentFinalComission;

                            decimal agentFinalBonus = agent.Bonus + agentBonus;
                            agent.Bonus = agentFinalBonus;

                            AgentTransaction agentTransaction = new AgentTransaction();

                            agentTransaction.AgentId = agent.AgentId;
                            agentTransaction.CreditAmount = systemSetting.AgentCommision;
                            agentTransaction.FinalAmount = agentFinalComission;
                            agentTransaction.CreditBonus = agentBonus;
                            agentTransaction.FinalBonus = agentFinalBonus;
                            agentTransaction.CreatedAt = DateTime.Now;
                            agentTransaction.CreatedBy = MetadataServices.GetCurrentUser().Username;

                            db.AgentTransaction.Add(agentTransaction);

                            db.SaveChanges();
                        }

                        var customerUser = db.User.Where(x => x.UserId == customer.UserId).FirstOrDefault();
                        await MetadataServices.SendEmail(customerUser.EmailAddress, "Enjoy Our Tour New Registration Approve", "Hi " + customer.CustomerName + ", <br/> Thank you for registering in our platform. <br/> Your account had been approve and it is active now.");

                    }
                    #endregion
                }

                else if (transactionViewModel.ActivityId == (int)EnjoyOurTour.Helpers.TransactionActivity.TopUp && (action == (int)EnjoyOurTour.Helpers.TransactionStatus.Approve || action == (int)EnjoyOurTour.Helpers.TransactionStatus.ApproveWithoutPay))
                {
                    #region top up transaction
                    var transactionModel = db.UserTransaction.Where(x => x.TransactionId == transactionViewModel.TransactionId).FirstOrDefault();

                    if (transactionModel.TransactionStatus == (int)EnjoyOurTour.Helpers.TransactionStatus.ApproveWithoutPay)
                    {
                        transactionModel.TransactionStatus = (int)EnjoyOurTour.Helpers.TransactionStatus.Approve;
                        transactionModel.UpdatedAt = DateTime.Now;
                        transactionModel.UpdatedBy = MetadataServices.GetCurrentUser().Username;

                        db.SaveChanges();
                    }
                    else
                    {
                        var systemSetting = db.SystemSetting.OrderByDescending(x => x.CreadtedDate).FirstOrDefault();

                        transactionModel.TransactionStatus = action;
                        transactionModel.Remarks = transactionViewModel.Remarks;
                        transactionModel.UpdatedAt = DateTime.Now;
                        transactionModel.UpdatedBy = MetadataServices.GetCurrentUser().Username;


                        var customer = db.Customer.Where(x => x.CustomerId == transactionModel.CustomerId).FirstOrDefault();
                        int finalTVR = customer.AvailableTVR + transactionModel.TopUpTVR;
                        int finalPoint = customer.AvailablePoint + transactionModel.PointAdd;

                        var user = MetadataServices.GetCurrentUser();

                        customer.AvailableTVR = finalTVR;
                        customer.AvailablePoint = finalPoint;
                        customer.UpdatedAt = DateTime.Now;
                        customer.UpdatedBy = user.Username;


                        IntroducerTransaction customerTransaction = new IntroducerTransaction();
                        customerTransaction.IntroducerId = customer.CustomerId;
                        customerTransaction.CreditTVR = transactionModel.TopUpTVR;
                        customerTransaction.FinalTVR = finalTVR;
                        customerTransaction.DebitTVR = 0;
                        customerTransaction.CreditAmount = 0;
                        customerTransaction.FinalAmount = customer.AvailableAmount;
                        customerTransaction.DebitAmount = 0;
                        customerTransaction.CreditPoint = transactionModel.PointAdd;
                        customerTransaction.FinalPoint = finalPoint;
                        customerTransaction.DebitPoint = 0;
                        customerTransaction.CreatedBy = user.Username;
                        customerTransaction.CreatedAt = DateTime.Now;

                        db.IntroducerTransaction.Add(customerTransaction);

                        var agent = db.Agent.Where(x => x.AgentId == transactionModel.AgentId).FirstOrDefault();

                        decimal agentCommision = systemSetting.AgentTopUpCommission;
                        decimal agentBonus = (((int)transactionModel.Amount / systemSetting.MinAmount) * systemSetting.AgentCalculationBonus);

                        decimal agentFinalCommission = agent.Commission + agentCommision;
                        agent.Commission = agentFinalCommission;

                        decimal agentFinalBonus = agent.Bonus + agentBonus;
                        agent.Bonus = agentFinalBonus;
                        agent.UpdatedAt = DateTime.Now;
                        agent.UpdatedBy = MetadataServices.GetCurrentUser().Username;

                        AgentTransaction agentTransaction = new AgentTransaction();

                        agentTransaction.AgentId = agent.AgentId;
                        agentTransaction.CreditAmount = agentCommision;
                        agentTransaction.FinalAmount = agentFinalCommission;
                        agentTransaction.CreditBonus = agentBonus;
                        agentTransaction.FinalBonus = agentFinalBonus;
                        agentTransaction.CreatedAt = DateTime.Now;
                        agentTransaction.CreatedBy = MetadataServices.GetCurrentUser().Username;

                        db.AgentTransaction.Add(agentTransaction);

                        db.SaveChanges();

                        var customerUser = db.User.Where(x => x.UserId == customer.UserId).FirstOrDefault();
                        await MetadataServices.SendEmail(customerUser.EmailAddress, "Enjoy Our Tour Top Up Approve", "Hi " + customer.CustomerName + ", <br/> Your Top Up Request had been approved. <br/> Please check your account.");

                    }
                    #endregion
                }
                else if (transactionViewModel.ActivityId == (int)EnjoyOurTour.Helpers.TransactionActivity.RedeemPackage && (action == (int)EnjoyOurTour.Helpers.TransactionStatus.Approve || action == (int)EnjoyOurTour.Helpers.TransactionStatus.ApproveWithoutPay))
                {
                    #region RedeemPackage
                    var transactionModel = db.UserTransaction.Where(x => x.TransactionId == transactionViewModel.TransactionId).FirstOrDefault();

                    if (transactionModel.TransactionStatus == (int)EnjoyOurTour.Helpers.TransactionStatus.ApproveWithoutPay)
                    {
                        transactionModel.TransactionStatus = (int)EnjoyOurTour.Helpers.TransactionStatus.Approve;
                        transactionModel.UpdatedAt = DateTime.Now;
                        transactionModel.UpdatedBy = MetadataServices.GetCurrentUser().Username;

                        db.SaveChanges();
                    }
                    else
                    {
                        int? finalTVR = transactionModel.TravelHeadCount;
                        var package = db.Package.Where(x => x.PackageId == transactionModel.PackageId).FirstOrDefault();
                        var customer = db.Customer.Where(x => x.CustomerId == transactionModel.CustomerId).FirstOrDefault();
                        if (transactionViewModel.HeadCount < transactionModel.TravelHeadCount)
                        {
                            finalTVR = transactionViewModel.HeadCount * package.TVR;

                            int? differentTVR = (transactionModel.TravelHeadCount * package.TVR) - (transactionViewModel.HeadCount * package.TVR);
                            int finalcalculatdTVR = customer.AvailableTVR + Convert.ToInt16(differentTVR);
                            var user = MetadataServices.GetCurrentUser();

                            customer.AvailableTVR = finalcalculatdTVR;
                            customer.UpdatedAt = DateTime.Now;
                            customer.UpdatedBy = user.Username;

                            var customerTransaction = new IntroducerTransaction()
                            {
                                IntroducerId = customer.CustomerId,
                                CreditTVR = differentTVR,
                                FinalTVR = finalcalculatdTVR,
                                DebitTVR = 0,
                                CreditAmount = 0,
                                FinalAmount = customer.AvailableAmount,
                                DebitAmount = 0,
                                CreditPoint = 0,
                                FinalPoint = customer.AvailablePoint,
                                DebitPoint = 0,
                                CreatedBy = user.Username,
                                CreatedAt = DateTime.Now
                            };

                            db.IntroducerTransaction.Add(customerTransaction);

                        }
                        else if (transactionViewModel.HeadCount > transactionModel.TravelHeadCount)
                        {
                            finalTVR = transactionViewModel.HeadCount * package.TVR;

                            int? differentTVR = (transactionViewModel.HeadCount * package.TVR) - (transactionModel.TravelHeadCount * package.TVR);
                            int finalcalculatdTVR = customer.AvailableTVR + Convert.ToInt16(differentTVR);
                            var user = MetadataServices.GetCurrentUser();

                            customer.AvailableTVR = finalcalculatdTVR;
                            customer.UpdatedAt = DateTime.Now;
                            customer.UpdatedBy = user.Username;

                            var customerTransaction = new IntroducerTransaction()
                            {
                                IntroducerId = customer.CustomerId,
                                CreditTVR = differentTVR,
                                FinalTVR = finalcalculatdTVR,
                                DebitTVR = 0,
                                CreditAmount = 0,
                                FinalAmount = customer.AvailableAmount,
                                DebitAmount = 0,
                                CreditPoint = 0,
                                FinalPoint = customer.AvailablePoint,
                                DebitPoint = 0,
                                CreatedBy = user.Username,
                                CreatedAt = DateTime.Now
                            };

                            db.IntroducerTransaction.Add(customerTransaction);
                        }

                        transactionModel.TravelHeadCount = transactionViewModel.HeadCount;
                        transactionModel.Remarks = transactionViewModel.Remarks;
                        transactionModel.RedeemTVR = Convert.ToInt32(finalTVR);
                        transactionModel.BalanceTVR = transactionModel.CurrentTVR - Convert.ToInt32(finalTVR);
                        transactionModel.TransactionStatus = action;
                        transactionModel.UpdatedAt = DateTime.Now;
                        transactionModel.UpdatedBy = MetadataServices.GetCurrentUser().Username;


                        db.SaveChanges();

                        var customerUser = db.User.Where(x => x.UserId == customer.UserId).FirstOrDefault();
                        await MetadataServices.SendEmail(customerUser.EmailAddress, "Enjoy Our Tour Redeem Package Approve", "Hi " + customer.CustomerName + ", <br/> Your redeem requst for Package (" + package.PackageName + ") had been approved. <br/> Please check your account.");

                    }

                    #endregion
                }
                else if (transactionViewModel.ActivityId == (int)EnjoyOurTour.Helpers.TransactionActivity.RedeemPoint && (action == (int)EnjoyOurTour.Helpers.TransactionStatus.Approve))
                {
                    #region RedeemPoint

                    var transactionModel = db.UserTransaction.Where(x => x.TransactionId == transactionViewModel.TransactionId).FirstOrDefault();
                    var product = db.ProductRedeem.Where(x => x.ProductRedeemId == transactionModel.ProductId).FirstOrDefault();
                    var customer = db.Customer.Where(x => x.CustomerId == transactionModel.CustomerId).FirstOrDefault();

                    transactionModel.TransactionStatus = (int)EnjoyOurTour.Helpers.TransactionStatus.Approve;
                    transactionModel.Remarks = transactionViewModel.Remarks;
                    transactionModel.UpdatedAt = DateTime.Now;
                    transactionModel.UpdatedBy = MetadataServices.GetCurrentUser().Username;

                    db.SaveChanges();


                    var customerUser = db.User.Where(x => x.UserId == customer.UserId).FirstOrDefault();
                    await MetadataServices.SendEmail(customerUser.EmailAddress, "Enjoy Our Tour Rewards Redeem Approve", "Hi " + customer.CustomerName + ", <br/> Your redeem requst for Product (" + product.ProductRedeemName + ") had been approved. <br/> Please check your account.");
                    #endregion
                }
                else if (action == (int)EnjoyOurTour.Helpers.TransactionStatus.Reject)
                {
                    #region Reject 
                    var transactionModel = db.UserTransaction.Where(x => x.TransactionId == transactionViewModel.TransactionId).FirstOrDefault();

                    transactionModel.TransactionStatus = (int)EnjoyOurTour.Helpers.TransactionStatus.Reject;
                    transactionModel.Remarks = transactionViewModel.Remarks;
                    transactionModel.UpdatedAt = DateTime.Now;
                    transactionModel.UpdatedBy = MetadataServices.GetCurrentUser().Username;

                    var customer = db.Customer.Where(x => x.CustomerId == transactionModel.CustomerId).FirstOrDefault();
                    var user = db.User.Where(x => x.UserId == customer.UserId).FirstOrDefault();

                    if (transactionViewModel.ActivityId == (int)EnjoyOurTour.Helpers.TransactionActivity.SignUp)
                    {
                        customer.IsDeleted = true;
                        user.IsDeleted = true;
                        user.IsActive = false;
                    }
                    else if (transactionViewModel.ActivityId == (int)EnjoyOurTour.Helpers.TransactionActivity.RedeemPackage)
                    {
                        int finalTVR = customer.AvailableTVR + transactionModel.RedeemTVR;

                        customer.AvailableTVR = finalTVR;

                        IntroducerTransaction customerTransaction = new IntroducerTransaction();
                        customerTransaction.IntroducerId = customer.CustomerId;
                        customerTransaction.CreditTVR = transactionModel.RedeemTVR;
                        customerTransaction.FinalTVR = finalTVR;
                        customerTransaction.DebitTVR = 0;
                        customerTransaction.CreditAmount = 0;
                        customerTransaction.FinalAmount = customer.AvailableAmount;
                        customerTransaction.DebitAmount = 0;
                        customerTransaction.CreditPoint = 0;
                        customerTransaction.FinalPoint = customer.AvailablePoint;
                        customerTransaction.DebitPoint = 0;
                        customerTransaction.CreatedBy = user.Username;
                        customerTransaction.CreatedAt = DateTime.Now;

                        db.IntroducerTransaction.Add(customerTransaction);
                    }

                    else if (transactionViewModel.ActivityId == (int)EnjoyOurTour.Helpers.TransactionActivity.RedeemPoint)
                    {
                        int finalPoint = customer.AvailablePoint + transactionModel.PointRedeem;

                        customer.AvailablePoint = finalPoint;

                        IntroducerTransaction customerTransaction = new IntroducerTransaction();
                        customerTransaction.IntroducerId = customer.CustomerId;
                        customerTransaction.CreditTVR = 0;
                        customerTransaction.FinalTVR = customer.AvailableTVR;
                        customerTransaction.DebitTVR = 0;
                        customerTransaction.CreditAmount = 0;
                        customerTransaction.FinalAmount = customer.AvailableAmount;
                        customerTransaction.DebitAmount = 0;
                        customerTransaction.CreditPoint = transactionModel.PointRedeem;
                        customerTransaction.FinalPoint = finalPoint;
                        customerTransaction.DebitPoint = 0;
                        customerTransaction.CreatedBy = user.Username;
                        customerTransaction.CreatedAt = DateTime.Now;

                        db.IntroducerTransaction.Add(customerTransaction);
                    }

                    db.SaveChanges();
                    #endregion

                }

                List<TransactionViewModel> transaction = (from data in db.TransactionView
                                                          select new TransactionViewModel()
                                                          {
                                                              TransactionId = data.TransactionId,
                                                              AgentCodeName = data.AgentCodeName,
                                                              CustomerCodeName = data.CustomerCodeName,
                                                              IntroducerCodeName = data.IntroducerCodeName,
                                                              ReferenceNo = data.ReferenceNo,
                                                              PathForProof = (string.IsNullOrEmpty(data.PathForProof) ? "No Image Upload" : data.PathForProof),
                                                              Amount = data.Amount,
                                                              CurrentTVR = data.CurrentTVR,
                                                              TopUpTVR = data.TopUpTVR,
                                                              RedeemTVR = data.RedeemTVR,
                                                              BalanceTVR = data.BalanceTVR,
                                                              CurrentPoint = data.CurrentPoint,
                                                              PointAdd = data.PointAdd,
                                                              PointRedeem = data.PointRedeem,
                                                              PointBalance = data.PointBalance,
                                                              Remarks = data.Remarks,
                                                              ActivityName = data.ActivityName,
                                                              TransactionStatusName = data.StatusName,
                                                              CreatedAt = data.CreatedAt
                                                          }).ToList();
                return View("Transaction", transaction);
            }
        }

        public ActionResult RedeemCouponTransaction()
        {
            using (var db = new TourEntities())
            {
                List<RedeemCouponTransactionViewModel> transaction = (from data in db.RedeemCouponTransactionView
                                                                      select new RedeemCouponTransactionViewModel()
                                                                      {
                                                                          RedeemCouponId = data.RedeemCouponId,
                                                                          CouponCode = data.CouponCode,
                                                                          TVRAmount = data.TVRAmount,
                                                                          CustomerName = data.CustomerName,
                                                                          StatusName = data.StatusName,
                                                                          CustomerId = data.CustomerId,
                                                                          TransactionStatusId = data.TransactionStatus,
                                                                          ImageProof = (string.IsNullOrEmpty(data.ImageProof) ? "No Image Upload" : data.ImageProof),
                                                                      }).ToList();
                return View(transaction);
            }
        }

        public ActionResult UpdateRedeemCouponTransaction(int RedeemCouponId)
        {
            using (var db = new TourEntities())
            {
                RedeemCouponTransactionViewModel transaction = (from data in db.RedeemCouponTransactionView
                                                                select new RedeemCouponTransactionViewModel()
                                                                {
                                                                    RedeemCouponId = data.RedeemCouponId,
                                                                    CouponCode = data.CouponCode,
                                                                    TVRAmount = data.TVRAmount,
                                                                    CustomerName = data.CustomerName,
                                                                    StatusName = data.StatusName,
                                                                    CustomerId = data.CustomerId,
                                                                    TransactionStatusId = data.TransactionStatus,
                                                                    ImageProof = (string.IsNullOrEmpty(data.ImageProof) ? "No Image Upload" : data.ImageProof),
                                                                }).Where(x => x.RedeemCouponId == RedeemCouponId).FirstOrDefault();
                return View(transaction);
            }
        }

        [HttpPost]
        public async System.Threading.Tasks.Task<ActionResult> UpdateRedeemCouponTransaction(RedeemCouponTransactionViewModel transactionViewModel, int action)
        {
            using (var db = new TourEntities())
            {
                var user = MetadataServices.GetCurrentUser();
                if (action == (int)EnjoyOurTour.Helpers.TransactionStatus.Approve)
                {
                    var customer = db.Customer.Where(x => x.CustomerId == transactionViewModel.CustomerId).FirstOrDefault();

                    var couponTransaction = db.RedeemCoupon.Where(x => x.RedeemCouponId == transactionViewModel.RedeemCouponId).FirstOrDefault();

                    int finalTVR = customer.AvailableTVR + couponTransaction.TVRAmount;

                    customer.AvailableTVR = finalTVR;
                    customer.UpdatedAt = DateTime.Now;
                    customer.UpdatedBy = user.Username;

                    IntroducerTransaction customerTransaction = new IntroducerTransaction();
                    customerTransaction.IntroducerId = customer.CustomerId;
                    customerTransaction.DebitAmount = 0;
                    customerTransaction.FinalAmount = customer.AvailableAmount;
                    customerTransaction.CreditAmount = 0;
                    customerTransaction.CreditTVR = couponTransaction.TVRAmount;
                    customerTransaction.FinalTVR = finalTVR;
                    customerTransaction.DebitTVR = 0;
                    customerTransaction.CreditPoint = 0;
                    customerTransaction.FinalPoint = customer.AvailablePoint;
                    customerTransaction.DebitPoint = 0;
                    customerTransaction.CreatedAt = DateTime.Now;
                    customerTransaction.CreatedBy = user.Username;

                    db.IntroducerTransaction.Add(customerTransaction);
                 
                    couponTransaction.TransactionStatus = (int)EnjoyOurTour.Helpers.TransactionStatus.Approve;


                    db.SaveChanges();

                    await MetadataServices.SendEmail(user.EmailAddress, "Enjoy Our Tour Coupon Redeem Approve", "Hi " + customer.CustomerName + ", <br/> Your redeem requst for Coupon (" + couponTransaction.CouponCode + ") had been approved. <br/> Please check your account.");
                }
                else
                {
                    var couponTransaction = db.RedeemCoupon.Where(x => x.RedeemCouponId == transactionViewModel.RedeemCouponId).FirstOrDefault();

                    couponTransaction.TransactionStatus = (int)EnjoyOurTour.Helpers.TransactionStatus.Reject;


                    db.SaveChanges();

                    await MetadataServices.SendEmail(user.EmailAddress, "Enjoy Our Tour Coupon Redeem Rejected", "Hi " +  ", <br/> Your redeem requst for Coupon (" + couponTransaction.CouponCode + ") had been reject. <br/> Please check your account.");

                }

                List<RedeemCouponTransactionViewModel> transaction = (from data in db.RedeemCouponTransactionView
                                                                      select new RedeemCouponTransactionViewModel()
                                                                      {
                                                                          RedeemCouponId = data.RedeemCouponId,
                                                                          CouponCode = data.CouponCode,
                                                                          TVRAmount = data.TVRAmount,
                                                                          CustomerName = data.CustomerName,
                                                                          StatusName = data.StatusName,
                                                                          CustomerId = data.CustomerId,
                                                                          ImageProof = (string.IsNullOrEmpty(data.ImageProof) ? "No Image Upload" : data.ImageProof),
                                                                      }).ToList();
                
                return View("RedeemCouponTransaction", transaction);

            }

        
         
        }
     
    }

    public static class ModelStateHelper
    {
        public static IEnumerable Errors(this ModelStateDictionary modelState)
        {
            if (!modelState.IsValid)
            {
                return modelState.ToDictionary(kvp => kvp.Key,
                        kvp => kvp.Value.Errors
                            .Select(e => e.ErrorMessage).ToArray())
                    .Where(m => m.Value.Count() > 0);
            }
            return null;
        }
    }
}