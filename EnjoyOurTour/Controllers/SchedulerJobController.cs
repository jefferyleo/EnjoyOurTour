using EnjoyOurTour.Helpers.Services;
using EnjoyOurTour.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EnjoyOurTour.Controllers
{
    public class SchedulerJobController : Controller
    {
        // GET: SchedulerJob
        [HttpGet]
        public async System.Threading.Tasks.Task<JsonResult> SendBirthdayWishes()
        {
            try
            {
                using (var db = new TourEntities())
                {
                    var currentDate = MetadataServices.GetCurrentDate();
                    var customerBirthday = db.Customer.Where(e => e.DateOfBirth.Month.Equals(currentDate.Month) && e.DateOfBirth.Day.Equals(currentDate.Day)).ToList();
                    foreach (var customer in customerBirthday)
                    {
                        var user = db.User.FirstOrDefault(e => e.UserId == customer.UserId);
                        if (user == null)
                        {
                            continue;
                        }
                        MetadataServices.SendWhatsappMessage("Hi "+ customer.CustomerName +", Happy Birthday to you.",customer.PhoneNumber); ;
                        await MetadataServices.SendEmail(user.EmailAddress, "Wishes from EnjoyOurTour", "Hi " + customer.CustomerName + ", <br/><br/>Happy Birthday to you!");
                    }
                    return Json(new { status = "success" }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                return Json(new { status = "failed"}, JsonRequestBehavior.AllowGet);
                throw;
            }
        }
    }
}