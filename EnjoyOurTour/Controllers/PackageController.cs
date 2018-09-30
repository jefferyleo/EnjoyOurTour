using EnjoyOurTour.Helpers.Authorization;
using EnjoyOurTour.Helpers.Services;
using EnjoyOurTour.Models;
using EnjoyOurTour.Models.ViewModel.Package;
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
    public class PackageController : Controller
    {
        // GET: Package
        public ActionResult Package()
        {
            using (var db = new TourEntities())
            {
                List<PackageViewModel> package = (from data in db.Package
                                                  where data.IsDeleted == false
                                                  select new PackageViewModel()
                                                  {
                                                      PackageId = data.PackageId,
                                                      PackageName = data.PackageName,
                                                      Description = data.Description,
                                                      IteneraryDetail = data.IteneraryDetail,
                                                      Amount = data.Amount,
                                                      TVR = data.TVR,
                                                      PhotoPath = data.PhotoPath,
                                                      CreatedAt = data.CreatedAt,
                                                      UpdatedAt = data.UpdatedAt
                                                  }).ToList();


                return View(package);
            }
        }

        public ActionResult AddPackage()
        {
            return View();
        }

        [HttpPost]
        public JsonResult AddPackage(AddPackageViewModel model)
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
                    string physicalPath = Server.MapPath("~/Image/Package/" + imageName);
                    model.PhotoPath.SaveAs(physicalPath);

                    string fileName = System.IO.Path.GetFileName(model.ItineraryFile.FileName);
                    fileName = MetadataServices.GetDateTimeWithoutSlash() + "-" + fileName;
                    string filePath = Server.MapPath("~/Files/" + fileName);
                    model.ItineraryFile.SaveAs(filePath);

                    var newPackage = new Package()
                    {
                        PackageName = model.PackageName,
                        PhotoPath = imageName,
                        FilePath = fileName,
                        Description = model.Description == null ? "" : model.Description,
                        IteneraryDetail = model.IteneraryDetail == null ? "" : model.IteneraryDetail,
                        TVR = model.TVR,
                        Amount = model.Amount,
                        CreatedBy = MetadataServices.GetCurrentUser().Username,
                        CreatedAt = MetadataServices.GetCurrentDate(),
                        UpdatedBy = MetadataServices.GetCurrentUser().Username,
                        UpdatedAt = MetadataServices.GetCurrentDate(),
                        IsDeleted = false
                    };
                    db.Package.Add(newPackage);
                    db.SaveChanges();
                }
                return Json(new { }, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult EditPackage(int id)
        {
            using (var db = new TourEntities())
            {
                var package = db.Package.Where(e => e.PackageId == id).FirstOrDefault();

                if (package == null)
                {
                    return new HttpNotFoundResult("Package not found");
                }
                return View(new PackageViewModel()
                {
                    PackageId = package.PackageId,
                    PackageName = package.PackageName,
                    Description = package.Description,
                    IteneraryDetail = package.IteneraryDetail,
                    Amount = package.Amount,
                    TVR = package.TVR
                });
            }
        }

        [HttpPost]
        public JsonResult EditPackage(AddPackageViewModel model)
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
                        var package = db.Package.Find(model.PackageId);

                        if (model.PhotoPath != null)
                        {
                            FileInfo path = new FileInfo(Server.MapPath("~/Image/Package/" + package.PhotoPath));
                            path.Delete();

                            string imageName = System.IO.Path.GetFileName(model.PhotoPath.FileName);
                            imageName = MetadataServices.GetDateTimeWithoutSlash() + "-" + imageName;
                            string physicalPath = Server.MapPath("~/Image/Package/" + imageName);
                            model.PhotoPath.SaveAs(physicalPath);

                            package.PhotoPath = imageName;
                        }

                        if (model.ItineraryFile != null)
                        {

                            if (!string.IsNullOrEmpty(package.FilePath))
                            {
                                FileInfo path = new FileInfo(Server.MapPath("~/Files/" + package.FilePath));
                                path.Delete();
                            }

                            string fileName = System.IO.Path.GetFileName(model.ItineraryFile.FileName);
                            fileName = MetadataServices.GetDateTimeWithoutSlash() + "-" + fileName;
                            string filePath = Server.MapPath("~/Files/" + fileName);
                            model.ItineraryFile.SaveAs(filePath);

                            package.FilePath = fileName;

                        }

                        package.PackageName = model.PackageName;
                        package.Description = model.Description;
                        package.IteneraryDetail = (string.IsNullOrEmpty(model.IteneraryDetail)? "" : model.IteneraryDetail);
                        package.TVR = model.TVR;
                        package.Amount = model.Amount;
                        package.UpdatedBy = MetadataServices.GetCurrentUser().Username;
                        package.UpdatedAt = MetadataServices.GetCurrentDate();

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

        public ActionResult DeletePackage(int id)
        {
            using (var db = new TourEntities())
            {
                if (ModelState.IsValid)
                {
                    var package = db.Package.Find(id);

                    if (package == null)
                    {
                        return new HttpNotFoundResult("Package not found.");
                    }

                    FileInfo path = new FileInfo(Server.MapPath("~/Image/Package/" + package.PhotoPath));
                    path.Delete();


                    package.IsDeleted = true;

                    db.SaveChanges();
                    return RedirectToAction("Package", "Package");
                }
                else
                {
                    return View();
                }
            }
        }
    }
}