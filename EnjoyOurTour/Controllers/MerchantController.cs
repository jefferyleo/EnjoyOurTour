using EnjoyOurTour.Helpers.Authorization;
using EnjoyOurTour.Helpers.Services;
using EnjoyOurTour.Models;
using EnjoyOurTour.Models.ViewModel.Merchant;
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
    public class MerchantController : Controller
    {
        // GET: Merchant
        public ActionResult Merchant()
        {
            using (var db = new TourEntities())
            {
                List<MerchantViewModel> merchant = (from data in db.Merchant
                                                    select new MerchantViewModel()
                                                    {
                                                        MerchantId = data.MerchantId,
                                                        MerchantName = data.MerchantName,
                                                        MerchantRegisterCode = data.MerchantRegisterCode,
                                                        MerchantPhoneNumber = data.MerchantPhoneNumber,
                                                        MerchantAddress = data.MerchantAddress,
                                                        MerchantCategoryId = data.MerchantCategoryId,
                                                        MerchantJoinDate = data.MerchantJoinDate,
                                                        MerchantLogoPath = data.MerchantLogoPath,
                                                        CreatedAt = data.CreatedAt,
                                                        UpdatedAt = data.UpdatedAt
                                                    }).ToList();


                return View(merchant);
            }
        }

        public ActionResult MerchantCategory()
        {
            using (var db = new TourEntities())
            {
                List<MerchantCategoryViewModel> merchantCategory = (from data in db.MerchantCategory
                                                    select new MerchantCategoryViewModel()
                                                    {
                                                        MerchantCategoryId = data.MerchantCategoryId,
                                                        CategoryName = data.CategoryName,
                                                        CreatedAt = data.CreatedAt
                                                    }).ToList();


                return View(merchantCategory);
            }
        }

        public ActionResult MerchantPromotion()
        {
            using (var db = new TourEntities())
            {
                List<AddMerchantPromotionViewModel> merchantPromotion = (from data in db.MerchantPromotion
                                                                    from merchantData in db.Merchant
                                                                    where data.MechantId == merchantData.MerchantId
                                                                    select new AddMerchantPromotionViewModel()
                                                                    {
                                                                        MerchantPromotionId=data.MerchantPromotionId,
                                                                        MerchantId = data.MechantId,
                                                                        MerchantName= merchantData.MerchantName,
                                                                        MerchantPromotionDetail = data.MerchantPromotionDetail,
                                                                        CreatedAt = data.CreatedAt
                                                                    }).ToList();


                return View(merchantPromotion);
            }
        }

        public ActionResult AddMerchantCategory()
        {
                return View();
        }

        public ActionResult AddMerchantPromotion()
        {
            AddMerchantPromotionViewModel addMerchantPromoViewModel = new AddMerchantPromotionViewModel();

            List<int> merchantId= new List<int>();

            using (var db = new TourEntities())
            {
                addMerchantPromoViewModel.MerchantIdList = db.Merchant.Select(e => new SelectListItem()
                {
                    Text = e.MerchantName.ToString(),
                    Value = e.MerchantId.ToString(),
                }).ToList();
            }

            return View(addMerchantPromoViewModel);
        }

        public ActionResult AddMerchant()
        {
            AddMerchantViewModel addMerchantVideModel = new AddMerchantViewModel();

            List<int> merchantCategory = new List<int>();

            using (var db = new TourEntities())
            {
                addMerchantVideModel.MerchantCategoryId = db.MerchantCategory.Select(e => new SelectListItem()
                {
                    Text = e.CategoryName.ToString(),
                    Value = e.MerchantCategoryId.ToString(),
                }).ToList();
            }

            return View(addMerchantVideModel);
        }

        [HttpPost]
        public JsonResult AddMerchantCategory(AddMerchantCategoryViewModel model)
        {
            using (var db = new TourEntities())
            {
                if (ModelState.IsValid)
                {
                    var formsAuthentication = HttpContext.Request.Cookies[FormsAuthentication.FormsCookieName] != null
                    ? FormsAuthentication.Decrypt(
                        HttpContext.Request.Cookies[FormsAuthentication.FormsCookieName].Value)
                    : null;

                    var newMerchantCategory = new MerchantCategory()
                    {
                        MerchantCategoryId = model.MerchantCategoryId,
                        CategoryName = model.CategoryName,
                        CreatedBy = MetadataServices.GetCurrentUser().Username,
                        CreatedAt = MetadataServices.GetCurrentDate(),
                        UpdatedBy = MetadataServices.GetCurrentUser().Username,
                        UpdatedAt = MetadataServices.GetCurrentDate()
                    };
                    db.MerchantCategory.Add(newMerchantCategory);
                    db.SaveChanges();
                }
                return Json(new { }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public JsonResult AddMerchantPromotion(AddMerchantPromotionViewModel model)
        {
            using (var db = new TourEntities())
            {
                if (ModelState.IsValid)
                {
                    var formsAuthentication = HttpContext.Request.Cookies[FormsAuthentication.FormsCookieName] != null
                    ? FormsAuthentication.Decrypt(
                        HttpContext.Request.Cookies[FormsAuthentication.FormsCookieName].Value)
                    : null;

                    var newMerchantPromotion = new MerchantPromotion()
                    {
                        MechantId = model.MerchantId,
                        MerchantPromotionDetail = model.MerchantPromotionDetail,
                        CreatedBy = MetadataServices.GetCurrentUser().Username,
                        CreatedAt = MetadataServices.GetCurrentDate(),
                        UpdatedBy = MetadataServices.GetCurrentUser().Username,
                        UpdatedAt = MetadataServices.GetCurrentDate()
                    };
                    db.MerchantPromotion.Add(newMerchantPromotion);
                    db.SaveChanges();
                }
                return Json(new { }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public JsonResult AddMerchant(AddMerchantViewModel model)
        {
            using (var db = new TourEntities())
            {
                if (ModelState.IsValid)
                {
                    var formsAuthentication = HttpContext.Request.Cookies[FormsAuthentication.FormsCookieName] != null
                    ? FormsAuthentication.Decrypt(
                        HttpContext.Request.Cookies[FormsAuthentication.FormsCookieName].Value)
                    : null;

                    string imageName = System.IO.Path.GetFileName(model.MerchantLogoPath.FileName);
                    imageName = MetadataServices.GetDateTimeWithoutSlash() + "-" + imageName;
                    string physicalPath = Server.MapPath("~/Image/Merchant/" + imageName);
                    model.MerchantLogoPath.SaveAs(physicalPath);

                    var newMerchant = new Merchant()
                    {
                        MerchantId = model.MerchantId,
                        MerchantName = model.MerchantName,
                        MerchantRegisterCode = model.MerchantRegisterCode,
                        MerchantPhoneNumber = model.MerchantPhoneNumber,
                        MerchantAddress = model.MerchantAddress,
                        MerchantCategoryId = model.GetMerchantCategoryId,
                        MerchantJoinDate = model.MerchantJoinDate,
                        MerchantLogoPath = imageName,
                        CreatedBy = MetadataServices.GetCurrentUser().Username,
                        CreatedAt = MetadataServices.GetCurrentDate(),
                        UpdatedBy = MetadataServices.GetCurrentUser().Username,
                        UpdatedAt = MetadataServices.GetCurrentDate()
                    };
                    db.Merchant.Add(newMerchant);
                    db.SaveChanges(); 
                }
                return Json(new { }, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult EditMerchantCategory(int id)
        {
            using (var db = new TourEntities())
            {
                var merchant = db.MerchantCategory.Where(e => e.MerchantCategoryId == id).FirstOrDefault();

                if (merchant == null)
                {
                    return new HttpNotFoundResult("Merchant Category not found");
                }

                return View(new AddMerchantCategoryViewModel()
                {
                    MerchantCategoryId = merchant.MerchantCategoryId,
                    CategoryName = merchant.CategoryName
                });
            }
        }

        public ActionResult EditMerchantPromotion(int id)
        {
            using (var db = new TourEntities())
            {
                var merchantPromo = db.MerchantPromotion.Where(e => e.MerchantPromotionId == id).FirstOrDefault();

                if (merchantPromo == null)
                {
                    return new HttpNotFoundResult("Merchant Category not found");
                }

                return View(new AddMerchantPromotionViewModel()
                {
                    MerchantPromotionId = merchantPromo.MerchantPromotionId,
                    MerchantId = merchantPromo.MechantId,
                    MerchantIdList = db.Merchant.Select(e => new SelectListItem()
                    {
                        Text = e.MerchantName.ToString(),
                        Value = e.MerchantId.ToString(),
                    }).ToList(),
                    MerchantPromotionDetail = merchantPromo.MerchantPromotionDetail
                });
            }
        }

        public ActionResult EditMerchant(int id)
        {
            using (var db = new TourEntities())
            {
                var merchant = db.Merchant.Where(e => e.MerchantId == id).FirstOrDefault();

                if (merchant == null)
                {
                    return new HttpNotFoundResult("Merchant not found");
                }

                List<int> merchantCategory = new List<int>();

                return View(new AddMerchantViewModel()
                {
                    MerchantId = merchant.MerchantId,
                    MerchantName = merchant.MerchantName,
                    MerchantRegisterCode = merchant.MerchantRegisterCode,
                    MerchantPhoneNumber = merchant.MerchantPhoneNumber,
                    MerchantAddress = merchant.MerchantAddress,
                    MerchantCategoryId = db.MerchantCategory.Select(e => new SelectListItem()
                    {
                        Text = e.CategoryName.ToString(),
                        Value = e.MerchantCategoryId.ToString(),
                    }).ToList(),
                    GetMerchantCategoryId = merchant.MerchantCategoryId,
                    displayMerchantJoinDate = merchant.MerchantJoinDate.ToString("yyyy-MM-dd HH:mm")
                });
            }
        }

        [HttpPost]
        public JsonResult EditMerchantCategory(AddMerchantCategoryViewModel model)
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
                        var merchant = db.MerchantCategory.Find(model.MerchantCategoryId);

                        merchant.MerchantCategoryId = model.MerchantCategoryId;
                        merchant.CategoryName = model.CategoryName;
                        merchant.UpdatedBy = MetadataServices.GetCurrentUser().Username;
                        merchant.UpdatedAt = MetadataServices.GetCurrentDate();

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


        [HttpPost]
        public JsonResult EditMerchantPromotion(AddMerchantPromotionViewModel model)
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
                        var merchantPromo = db.MerchantPromotion.Find(model.MerchantPromotionId);

                        merchantPromo.MechantId = model.MerchantId;
                        merchantPromo.MerchantPromotionDetail = model.MerchantPromotionDetail;
                        merchantPromo.UpdatedBy = MetadataServices.GetCurrentUser().Username;
                        merchantPromo.UpdatedAt = MetadataServices.GetCurrentDate();

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

        [HttpPost]
        public JsonResult EditMerchant(AddMerchantViewModel model)
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
                        var merchant = db.Merchant.Find(model.MerchantId);

                        if (model.MerchantLogoPath != null)
                        {
                            FileInfo path = new FileInfo(Server.MapPath("~/Image/Merchant/" + merchant.MerchantLogoPath));
                            path.Delete();

                            string imageName = System.IO.Path.GetFileName(model.MerchantLogoPath.FileName);
                            imageName = MetadataServices.GetDateTimeWithoutSlash() + "-" + imageName;
                            string physicalPath = Server.MapPath("~/Image/Merchant/" + imageName);
                            model.MerchantLogoPath.SaveAs(physicalPath);

                            merchant.MerchantLogoPath = imageName;
                        }

                        merchant.MerchantId = model.MerchantId;
                        merchant.MerchantName = model.MerchantName;
                        merchant.MerchantRegisterCode = model.MerchantRegisterCode;
                        merchant.MerchantPhoneNumber = model.MerchantPhoneNumber;
                        merchant.MerchantAddress = model.MerchantAddress;
                        merchant.MerchantCategoryId = model.GetMerchantCategoryId;
                        merchant.MerchantJoinDate = model.MerchantJoinDate;
                        merchant.UpdatedBy = MetadataServices.GetCurrentUser().Username;
                        merchant.UpdatedAt = MetadataServices.GetCurrentDate();

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

        public ActionResult DeleteMerchantCategory(int id)
        {
            using (var db = new TourEntities())
            {
                if (ModelState.IsValid)
                {
                    var merchantCategory = db.MerchantCategory.Find(id);

                    if (merchantCategory == null)
                    {
                        return new HttpNotFoundResult("Merchant Category not found.");
                    }

                    db.MerchantCategory.Remove(merchantCategory);

                    db.SaveChanges();
                    return RedirectToAction("MerchantCategory", "Merchant");
                }
                else
                {
                    return View();
                }
            }
        }

        public ActionResult DeleteMerchantPromotion(int id)
        {
            using (var db = new TourEntities())
            {
                if (ModelState.IsValid)
                {
                    var merchantPromo = db.MerchantPromotion.Find(id);

                    if (merchantPromo == null)
                    {
                        return new HttpNotFoundResult("Merchant Promotion not found.");
                    }

                    db.MerchantPromotion.Remove(merchantPromo);

                    db.SaveChanges();
                    return RedirectToAction("MerchantPromotion", "Merchant");
                }
                else
                {
                    return View();
                }
            }
        }

        public ActionResult DeleteMerchant(int id)
        {
            using (var db = new TourEntities())
            {
                if (ModelState.IsValid)
                {
                    var merchant = db.Merchant.Find(id);

                    if (merchant == null)
                    {
                        return new HttpNotFoundResult("Merchant not found.");
                    }

                    FileInfo path = new FileInfo(Server.MapPath("~/Image/Merchant/" + merchant.MerchantLogoPath));
                    path.Delete();

                    db.Merchant.Remove(merchant);

                    db.SaveChanges();
                    return RedirectToAction("Merchant", "Merchant");
                }
                else
                {
                    return View();
                }
            }
        }
    }
}