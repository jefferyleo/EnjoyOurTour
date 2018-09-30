using EnjoyOurTour.Helpers.Authorization;
using EnjoyOurTour.Helpers.Services;
using EnjoyOurTour.Models;
using EnjoyOurTour.Models.ViewModel.TravelPromotion;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace EnjoyOurTour.Controllers
{
    [TourAuthorize(Roles = "Admin, Superadmin")]
    public class TravelPromotionController : Controller
    {
        // GET: TravelPromotion
        public ActionResult TravelPromotion()
        {
            using (var db = new TourEntities())
            {
                List<TravelPromotionViewModel> travelPromotion = (from data in db.TravelPromotion                                                
                                                select new TravelPromotionViewModel()
                                                {
                                                    TravelPromotionId = data.TravelPromotionId,
                                                    TravelPromotionTitle = data.TravelPromotionTitle,
                                                    Description = data.Description,
                                                    PhotoPath = data.PhotoPath,
                                                    CreatedAt = data.CreatedAt,                                                    
                                                    UpdatedAt = data.UpdatedAt                                                                                
                                                }).ToList();      
                return View(travelPromotion);
            }
        }

        public ActionResult AddTravelPromotion()
        {
            return View();
        }

        [HttpPost]
        public JsonResult AddTravelPromotion(TravelPromotionViewModel model)
        {
            using (var db = new TourEntities())
            {
                if (ModelState.IsValid)
                {
                    var formsAuthentication = HttpContext.Request.Cookies[FormsAuthentication.FormsCookieName] != null
                    ? FormsAuthentication.Decrypt(
                        HttpContext.Request.Cookies[FormsAuthentication.FormsCookieName].Value)
                    : null;
                    TravelPromotion travelPromotion = new TravelPromotion();
                    if (model.Description == null)
                    {
                        travelPromotion.Description = "";
                    }
                    else
                    {
                        travelPromotion.Description = model.Description;
                    }
                    string imageName = System.IO.Path.GetFileName(model.Image.FileName);
                    imageName = MetadataServices.GetDateTimeWithoutSlash() + "-" + imageName;
                    string physicalPath = Server.MapPath("~/Image/TravelPromotionImage/" + imageName);
                    model.Image.SaveAs(physicalPath);

                    travelPromotion.TravelPromotionTitle = model.TravelPromotionTitle;
                    travelPromotion.PhotoPath = imageName;                                        
                    travelPromotion.CreatedAt = MetadataServices.GetCurrentDate();
                    travelPromotion.CreatedBy = MetadataServices.GetCurrentUser().Username;
                    travelPromotion.UpdatedAt = MetadataServices.GetCurrentDate();
                    travelPromotion.UpdatedBy = MetadataServices.GetCurrentUser().Username;
                    db.TravelPromotion.Add(travelPromotion);
                    db.SaveChanges();
                }
                return Json(new { }, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult EditTravelPromotion(int id)
        {

            using (var db = new TourEntities())
            {
                var tpromotion = db.TravelPromotion.Where(e => e.TravelPromotionId == id).FirstOrDefault();

                if (tpromotion == null)
                {
                    return new HttpNotFoundResult("Travel Promotion not found");
                }
                return View(new TravelPromotionViewModel()
                {
                    TravelPromotionId = tpromotion.TravelPromotionId,
                    TravelPromotionTitle = tpromotion.TravelPromotionTitle,
                    Description = tpromotion.Description
                });
            }
        }

        [HttpPost]
        public JsonResult EditTravelPromotion(TravelPromotionViewModel model)
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
                        TravelPromotion travelPromotion = db.TravelPromotion.Where(e => e.TravelPromotionId == model.TravelPromotionId).FirstOrDefault();

                        if (model.Description == null)
                        {
                            travelPromotion.Description = "";
                        }
                        else
                        {
                            travelPromotion.Description = model.Description;
                        }


                        if (model.Image != null)
                        {
                            FileInfo path = new FileInfo(Server.MapPath("~/Image/TravelPromotionImage/" + travelPromotion.PhotoPath));
                            path.Delete();

                            string imageName = System.IO.Path.GetFileName(model.Image.FileName);
                            imageName = MetadataServices.GetDateTimeWithoutSlash() + "-" + imageName;
                            string physicalPath = Server.MapPath("~/Image/TravelPromotionImage/" + imageName);
                            model.Image.SaveAs(physicalPath);
                            travelPromotion.PhotoPath = imageName;
                        }

                        travelPromotion.TravelPromotionTitle = model.TravelPromotionTitle;
                        travelPromotion.UpdatedAt = MetadataServices.GetCurrentDate();
                        travelPromotion.UpdatedBy = MetadataServices.GetCurrentUser().Username;
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


        public ActionResult DeleteTravelPromotion(int id)
        {

            using (var db = new TourEntities())
            {
                var tpromotion = db.TravelPromotion.Where(e => e.TravelPromotionId == id).FirstOrDefault();

                if (tpromotion == null)
                {
                    return new HttpNotFoundResult("Travel Promotion not found");
                }
                else
                {
                    FileInfo path = new FileInfo(Server.MapPath("~/Image/TravelPromotionImage/" + tpromotion.PhotoPath));
                    path.Delete();

                    db.TravelPromotion.Remove(tpromotion);
                    db.SaveChanges();

                }
                    List<TravelPromotionViewModel> travelPromotion = (from data in db.TravelPromotion
                                                                      select new TravelPromotionViewModel()
                                                                      {
                                                                          TravelPromotionId = data.TravelPromotionId,
                                                                          Description = data.Description,
                                                                          PhotoPath = data.PhotoPath,
                                                                          CreatedAt = data.CreatedAt,
                                                                          UpdatedAt = data.UpdatedAt
                                                                      }).ToList();
               return View("TravelPromotion", travelPromotion);
                
            }
        }
    }
}