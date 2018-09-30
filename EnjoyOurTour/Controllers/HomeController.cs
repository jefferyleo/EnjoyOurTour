using EnjoyOurTour.Models;
using EnjoyOurTour.Models.ViewModel.Home;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EnjoyOurTour.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
     {
            using (var db = new TourEntities())
            {
                var banners = (from e in db.Banner
                              where e.IsActive
                              select new BannerViewModel()
                              {
                                  BannerDescription = e.BannerDescription,
                                  BannerImage = e.BannerImage,
                              }).ToList();
                var aboutUs = db.AboutUs.Select(e => new AboutUsViewModel
                {
                    AboutUsTitle = e.AboutUsTitle,
                    AboutUsDescription = e.AboutUsDescription,
                    AboutUsShortDescription = e.AboutUsShortDescription,
                    YoutubeUrl = e.YoutubeLink,
                }).FirstOrDefault();
                var contact = db.ContactUs.Select(e => new ContactUsViewModel()
                {
                    ContactUsTitle = e.ContactUsTitle,
                    CompanyName = e.CompanyName,
                    CompanyAddress = e.CompanyAddress,
                    CompanyPhoneNumber = e.CompanyPhoneNumber,
                    CompanyLatitude = e.CompanyLatitude,
                    CompanyLongitude = e.CompanyLongitude,
                }).FirstOrDefault();
                return View(new HomeViewModel()
                {
                    Banners = banners,
                    AboutUs = aboutUs,
                    ContactUs = contact,
                });
            }
            
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}