using EnjoyOurTour.Helpers.Authorization;
using EnjoyOurTour.Helpers.Services;
using EnjoyOurTour.Models;
using EnjoyOurTour.Models.ViewModel.GoodNews;
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
    public class GoodNewsController : Controller
    {
        // GET: GoodNews
        public ActionResult GoodNews()
        {
            using (var db = new TourEntities())
            {
                List<GoodNewsViewModel> goodNews = (from data in db.GoodNews
                                                  select new GoodNewsViewModel()
                                                  {
                                                      GoodNewsId = data.GoodNewsId,
                                                      GoodNewsTitle = data.GoodNewsTitle,
                                                      Description = data.Description,
                                                      PhotoPath = data.PhotoPath,
                                                      CreatedAt = data.CreatedAt,
                                                      UpdatedAt = data.UpdatedAt
                                                  }).ToList();


                return View(goodNews);
            }
        }

        public ActionResult AddGoodNews()
        {
            return View();
        }

        [HttpPost]
        public JsonResult AddGoodNews(AddGoodNewsViewModel model)
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
                    string physicalPath = Server.MapPath("~/Image/GoodNews/" + imageName);
                    model.PhotoPath.SaveAs(physicalPath);

                    var goodNews = new GoodNews()
                    {
                        GoodNewsTitle = model.GoodNewsTitle,
                        PhotoPath = imageName,
                        Description = model.Description == null ? "" : model.Description,
                        CreatedBy = MetadataServices.GetCurrentUser().Username,
                        CreatedAt = MetadataServices.GetCurrentDate(),
                        UpdatedBy = MetadataServices.GetCurrentUser().Username,
                        UpdatedAt = MetadataServices.GetCurrentDate(),
                    };
                    db.GoodNews.Add(goodNews);
                    db.SaveChanges();
                }
                return Json(new { }, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult EditGoodNews(int id)
        {
            using (var db = new TourEntities())
            {
                var news = db.GoodNews.Where(e => e.GoodNewsId == id).FirstOrDefault();

                if (news == null)
                {
                    return new HttpNotFoundResult("News not found");
                }
                return View(new GoodNewsViewModel()
                {
                    GoodNewsId = news.GoodNewsId,
                    GoodNewsTitle = news.GoodNewsTitle,
                    Description = news.Description
                });
            }
        }

        [HttpPost]
        public JsonResult EditGoodNews(AddGoodNewsViewModel model)
        {
            try
            {
                using (var db = new TourEntities())
                {
                    var formsAuthentication = HttpContext.Request.Cookies[FormsAuthentication.FormsCookieName] != null
                  ? FormsAuthentication.Decrypt(
                      HttpContext.Request.Cookies[FormsAuthentication.FormsCookieName].Value)
                  : null;

                    if (ModelState.IsValid)
                    {
                        var news = db.GoodNews.Find(model.GoodNewsId);

                        if (model.PhotoPath != null)
                        {
                            FileInfo path = new FileInfo(Server.MapPath("~/Image/GoodNews/" + news.PhotoPath));
                            path.Delete();

                            string imageName = System.IO.Path.GetFileName(model.PhotoPath.FileName);
                            imageName = MetadataServices.GetDateTimeWithoutSlash() + "-" + imageName;
                            string physicalPath = Server.MapPath("~/Image/GoodNews/" + imageName);
                            model.PhotoPath.SaveAs(physicalPath);

                            news.PhotoPath = imageName;
                        }

                        news.GoodNewsTitle = model.GoodNewsTitle;
                        news.Description = model.Description;
                        news.UpdatedBy = MetadataServices.GetCurrentUser().Username;
                        news.UpdatedAt = MetadataServices.GetCurrentDate();

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

        public ActionResult DeleteGoodNews(int id)
        {
            using (var db = new TourEntities())
            {
                if (ModelState.IsValid)
                {
                    var news = db.GoodNews.Find(id);

                    if (news == null)
                    {
                        return new HttpNotFoundResult("News not found.");
                    }

                    FileInfo path = new FileInfo(Server.MapPath("~/Image/GoodNews/" + news.PhotoPath));
                    path.Delete();

                    db.GoodNews.Remove(news);

                    db.SaveChanges();
                    return RedirectToAction("GoodNews", "GoodNews");
                }
                else
                {
                    return View();
                }
            }
        }
    }
}