using EnjoyOurTour.Helpers.Authorization;
using EnjoyOurTour.Helpers.Services;
using EnjoyOurTour.Models;
using EnjoyOurTour.Models.ViewModel.Account;
using EnjoyOurTour.Models.ViewModel.Home;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using EnjoyOurTour.Helpers;
using static EnjoyOurTour.Helpers.Authorization.TourAuthorizeAttribute;
using EnjoyOurTour.Models.ViewModel.Admin;

namespace EnjoyOurTour.Controllers
{
    public class AccountController : Controller
    {
        // GET: Account
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult UpdatePassword()
        {
            UserViewModel userViewModel = new UserViewModel();
            return View(userViewModel);
        }


        [AllowAnonymous]
        public ActionResult SignUp()
        {
            SignUpViewModel signupViewModel = new SignUpViewModel();
            using (var db = new TourEntities())
            {              
                List<string> topUpAmount = new List<string>();

                int? minTopUpAmount = db.SystemSetting.OrderByDescending(x => x.CreadtedDate).FirstOrDefault().MinAmount;

                if (minTopUpAmount != null)
                {                    
                    topUpAmount.Add(minTopUpAmount.ToString());

                    for (int i = 1; i <= 4; i++)
                    {
                        minTopUpAmount += minTopUpAmount;
                        topUpAmount.Add(minTopUpAmount.ToString());
                    }

                    topUpAmount.Add("Other");
                }

                signupViewModel.TopUpAmount = topUpAmount.ToList().Select(e => new SelectListItem()
                                                    {
                                                        Text = e.ToString(),
                                                        Value = e.ToString(),
                                                    }).ToList();
            }

              
            return View(signupViewModel);
        }


        [HttpPost]
        public JsonResult SignUp(SignUpViewModel model)
        {
            int amount = 0;
            using (var db = new TourEntities())
            {
                var agent = db.Agent.FirstOrDefault(e => e.AgentCode == model.AgentCode);
                if (agent == null)
                {
                    ModelState.AddModelError("AgentCode", "Agent Code is not found.");                    
                }

                var introducer = new Customer();
                if (!string.IsNullOrEmpty(model.IntroducerCode))
                {
                    introducer = db.Customer.FirstOrDefault(e => e.CustomerCode == model.IntroducerCode);
                    if (introducer == null)
                    {
                        ModelState.AddModelError("IntroducerCode", "Introducer Code is not found.");
                    }
                }                
                var systemSetting = db.SystemSetting.OrderByDescending(x => x.CreadtedDate).FirstOrDefault();

                if (model.TopUpAmountValue == "Other")
                {
                    if ((model.OtherAmount % systemSetting.MinAmount) != 0 || model.OtherAmount == 0)
                    {
                        ModelState.AddModelError("OtherAmount", "Other Amount not multiply of " + systemSetting.MinAmount);
                    }
                    else
                    {
                        amount = model.OtherAmount;
                    }
                }
                else
                {
                    amount = Convert.ToInt32(model.TopUpAmountValue);
                }

                var userWithNRIC = db.Customer.Where(x => x.NRIC == model.NRIC).FirstOrDefault();

                if (userWithNRIC != null)
                {
                    ModelState.AddModelError("NRIC", "NRIC Duplication");
                }

                var userWithUsername = db.User.Where(x => x.Username == model.Username).FirstOrDefault();

                if (userWithUsername != null)
                {
                    ModelState.AddModelError("Username", "Username Duplication");
                }

                if (ModelState.IsValid)
                {
                    var user = new User()
                    {
                        Username = model.Username,
                        EmailAddress = model.EmailAddress,
                        Password = PasswordHash.CreateHash(model.Password),
                        RoleId = TourRoleId.Customer,
                        CreatedAt = MetadataServices.GetCurrentDate(),
                        CreatedBy = model.Username,
                        UpdatedAt = MetadataServices.GetCurrentDate(),
                        UpdatedBy = model.Username,
                        IsActive = false,
                        IsDeleted = false,
                    };
                    db.User.Add(user);
                    db.SaveChanges();

                    string customerCode = model.NRIC.GetLast(6);
                    var customer = new Customer()
                    {
                        CustomerName = model.CustomerName,
                        NRIC = model.NRIC,
                        CustomerCode = customerCode,
                        BankAccountNumber = model.BankAccountNumber,
                        PhoneNumber = model.PhoneNumber,
                        Address = model.Address,
                        AvailableTVR = 0,
                        AvailableAmount = 0,
                        AvailablePoint = 0,
                        AgentId = agent.AgentId,
                        IntroducerId = introducer.CustomerId,
                        JoinDate = MetadataServices.GetCurrentDate(),
                        UserId = user.UserId,
                        CreatedAt = MetadataServices.GetCurrentDate(),
                        CreatedBy = model.Username,
                        UpdatedAt = MetadataServices.GetCurrentDate(),
                        UpdatedBy = model.Username,
                        DateOfBirth = model.DateOfBirth,
                        RenewDate = MetadataServices.GetCurrentDate(),
                        ExpiredDate = MetadataServices.GetCurrentDate(),
                        BankName = model.BankName,
                        IsDeleted = false
                    };
                    db.Customer.Add(customer);
                    db.SaveChanges();               

                    var customerNew = db.Customer.FirstOrDefault(e => e.CustomerCode == customerCode);
                    

                    string imageName = "";

                    if (model.Image != null)
                    {
                        imageName = System.IO.Path.GetFileName(model.Image.FileName);
                        imageName = MetadataServices.GetDateTimeWithoutSlash() + "-" + imageName;
                        string physicalPath = Server.MapPath("~/Image/TransactionImage/" + imageName);
                        model.Image.SaveAs(physicalPath);
                    }

                    var userTransaction = new UserTransaction()
                    {
                        AgentId = agent.AgentId,
                        CustomerId = customerNew.CustomerId,
                        IntroducerId = customerNew.IntroducerId,
                        ReferenceNo = MetadataServices.GenerateCode(6),
                        ActivityId = (int) Helpers.TransactionActivity.SignUp,
                        PathForProof = imageName,
                        Amount = 0,
                        CurrentTVR = 0,
                        TopUpTVR = systemSetting.TVRMutiply * amount,
                        RedeemTVR = 0,
                        BalanceTVR = 0,
                        CurrentPoint = 0,
                        PointAdd = ((amount / systemSetting.MinAmount) * systemSetting.PointCalculation),
                        PointRedeem = 0,
                        PointBalance = 0,
                        ActionDate = null,
                        Remarks = "",
                        CreatedAt = MetadataServices.GetCurrentDate(),
                        CreatedBy = model.Username,
                        UpdatedAt = MetadataServices.GetCurrentDate(),
                        UpdatedBy = model.Username,
                        IsDeleted = false,
                        TransactionStatus = (int) TransactionStatus.Pending
                    };
                    db.UserTransaction.Add(userTransaction);
                    db.SaveChanges();

                    return Json(new { success = true },  JsonRequestBehavior.AllowGet);
                }
                else
                {
                    List<string> errors = new List<string>();

                    foreach (ModelState modelState in ViewData.ModelState.Values)
                    {
                        foreach (ModelError error in modelState.Errors)
                        {
                            errors.Add(string.IsNullOrEmpty(error.ErrorMessage) ? error.Exception.ToString() : error.ErrorMessage);
                        }
                    }
                    return Json(new { success = false, errors = errors }, JsonRequestBehavior.AllowGet);
                }
               
            }
            
        }

        [HttpPost]
        public JsonResult UpdatePassword(UserViewModel model)
        {
            string error="";

            using (var db = new TourEntities())
            {
                var formsAuthentication = HttpContext.Request.Cookies[FormsAuthentication.FormsCookieName] != null
              ? FormsAuthentication.Decrypt(
                  HttpContext.Request.Cookies[FormsAuthentication.FormsCookieName].Value)
              : null;

                if (model.CurrentPassword != null && model.Password != null)
                {
                    var user = db.User.Find(MetadataServices.GetCurrentUser().UserId);

                    if (PasswordHash.ValidatePassword(model.CurrentPassword, user.Password))
                    {
                        user.Password = PasswordHash.CreateHash(model.Password);
                        user.UpdatedAt = MetadataServices.GetCurrentDate();
                        user.UpdatedBy = MetadataServices.GetCurrentUser().Username;

                        db.SaveChanges();
                    }
                    else
                    {
                        error = "Invalid Current Password.";
                    }
                }
                else
                {
                    error = "Invalid Current Password.";
                }

                return Json(new { error=error }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public JsonResult GetAgentName(int agentCode)
        {
            string returnCode;
            using (var db = new TourEntities())
            {
                var agent = db.Agent.FirstOrDefault(e => e.AgentCode == agentCode);

                if (agent == null)
                {
                    returnCode = "Cant find this Agent";
                }
                else
                {
                    returnCode = agent.AgentName;
                }
            }
            return Json(new { returnCode }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetIntroducerName(string introducerCode)
        {
            string returnCode;
            using (var db = new TourEntities())
            {
                var customer = db.Customer.FirstOrDefault(e => e.CustomerCode == introducerCode);

                if (customer == null)
                {
                    returnCode = "Cant find this Agent";
                }
                else
                {
                    returnCode = customer.CustomerName;
                }
            }
            return Json(new { returnCode }, JsonRequestBehavior.AllowGet);
        }

        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View(new LoginViewModel());
        }

        [AllowAnonymous]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(LoginViewModel model)
        {
            using (var db = new TourEntities())
            {
                var password = db.User.Where(e => e.Username == model.Username).Select(e => e.Password).FirstOrDefault();
                if (password == null)
                {
                    ViewBag.LoginStatus = "Your Username or Password is wrong.";
                    return View(model);
                }
                var isPasswordCorrect = PasswordHash.ValidatePassword(model.Password, password);

                if (isPasswordCorrect)
                {
                    var user = db.User.FirstOrDefault(e => e.Username == model.Username);
                    var role = db.Role.FirstOrDefault(e => e.RoleId.Equals(user.RoleId));
                    FormsAuthentication.SetAuthCookie(model.Username, true);
                    ModelState.Remove("Password");

                    var authTicket = new FormsAuthenticationTicket(
                        1, // version
                        user.UserId.ToString(), // user name
                        DateTime.Now, // created
                        DateTime.Now.AddMinutes(20), // expires
                        true, // persistent?
                        role.RoleName // can be used to store roles
                        );

                    string encryptedTicket = FormsAuthentication.Encrypt(authTicket);

                    var authCookie = new HttpCookie(FormsAuthentication.FormsCookieName, encryptedTicket);
                    HttpContext.Response.Cookies.Add(authCookie);

                    if (role.RoleName == TourRole.Admin)
                    {
                        return RedirectToAction("Index","Admin");
                    }
                    else if (role.RoleName == TourRole.Superadmin)
                    {
                        return RedirectToAction("Index", "Admin");
                    }
                    else if (role.RoleName == TourRole.Agent)
                    {
                        return RedirectToAction("AgentTransaction", "Agent");
                    }
                    else if (role.RoleName == TourRole.Customer)
                    {
                        return RedirectToAction("TravelPromotion", "Customer");
                    }
                    else
                    {
                        return RedirectToAction("Login", "Account");
                    }
                }
                else
                {
                    ViewBag.LoginStatus = "Your Username or Password is wrong.";
                    return View(model);
                }
            }
        }

        public ActionResult ViewDetail()
        {
            using (var db = new TourEntities())
            {
                User user = MetadataServices.GetCurrentUser();                
                var role = db.Role.FirstOrDefault(e => e.RoleId.Equals(user.RoleId));
                

                if (role.RoleName == TourRole.Admin)
                {
                    return RedirectToAction("Index", "Admin");
                }
                else if (role.RoleName == TourRole.Superadmin)
                {
                    return RedirectToAction("Index", "Admin");
                }
                else if (role.RoleName == TourRole.Agent)
                {
                    return RedirectToAction("AgentTransaction", "Agent");
                }
                else if (role.RoleName == TourRole.Customer)
                {
                    return RedirectToAction("TravelPromotion", "Customer");
                }
                else
                {
                    return RedirectToAction("Login", "Account");
                }
            }
        }

        [TourAuthorize(Roles = "Admin, Superadmin, Agent, Customer")]
        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Index", "Home");
        }
    }

}