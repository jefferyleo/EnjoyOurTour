using EnjoyOurTour.Helpers.Authorization;
using EnjoyOurTour.Helpers.Services;
using EnjoyOurTour.Models;
using EnjoyOurTour.Models.ViewModel.Product;
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
    public class ProductController : Controller
    {
        // GET: Product
        public ActionResult ProductRedeem()
        {
            using (var db = new TourEntities())
            {
                List<ProductRedeemViewModel> productRedeem = (from data in db.ProductRedeem
                                                              select new ProductRedeemViewModel()
                                                              {
                                                                  ProductRedeemId = data.ProductRedeemId,
                                                                  ProductRedeemName = data.ProductRedeemName,
                                                                  RedeemPoint = data.RedeemPoint,
                                                                  Stock = data.Stock,
                                                                  ImagePath = data.ImagePath,
                                                                  CreatedBy = data.CreatedBy,
                                                                  CreatedAt = data.CreatedAt,
                                                                  UpdatedBy = data.UpdatedBy,
                                                                  UpdatedAt = data.UpdatedAt
                                                              }).ToList();


                return View(productRedeem);
            }
        }

        public ActionResult AddProductRedeem()
        {
            return View();
        }

        [HttpPost]
        public JsonResult AddProductRedeem(AddProductRedeemViewModel model)
        {
            using (var db = new TourEntities())
            {
                if (ModelState.IsValid)
                {
                    var formsAuthentication = HttpContext.Request.Cookies[FormsAuthentication.FormsCookieName] != null
                    ? FormsAuthentication.Decrypt(
                        HttpContext.Request.Cookies[FormsAuthentication.FormsCookieName].Value)
                    : null;

                    string imageName = System.IO.Path.GetFileName(model.ImagePath.FileName);
                    imageName = MetadataServices.GetDateTimeWithoutSlash() + "-" + imageName;
                    string physicalPath = Server.MapPath("~/Image/Product/" + imageName);
                    model.ImagePath.SaveAs(physicalPath);

                    var newProductRedeemViewModel = new ProductRedeem()
                    {
                        ProductRedeemName = model.ProductRedeemName,
                        RedeemPoint = model.RedeemPoint,
                        Stock = model.Stock,
                        ImagePath = imageName,
                        CreatedBy = MetadataServices.GetCurrentUser().Username,
                        CreatedAt = MetadataServices.GetCurrentDate(),
                        UpdatedBy = MetadataServices.GetCurrentUser().Username,
                        UpdatedAt = MetadataServices.GetCurrentDate()
                    };
                    db.ProductRedeem.Add(newProductRedeemViewModel);
                    db.SaveChanges();
                }
                return Json(new { }, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult EditProductRedeem(int id)
        {
            using (var db = new TourEntities())
            {
                var productRedeem = db.ProductRedeem.Where(e => e.ProductRedeemId == id).FirstOrDefault();

                if (productRedeem == null)
                {
                    return new HttpNotFoundResult("Product Redeem not found");
                }

                return View(new ProductRedeemViewModel()
                {
                    ProductRedeemId = productRedeem.ProductRedeemId,
                    ProductRedeemName = productRedeem.ProductRedeemName,
                    RedeemPoint = productRedeem.RedeemPoint,
                    Stock = productRedeem.Stock,
                    ImagePath = productRedeem.ImagePath,
                    CreatedBy = productRedeem.CreatedBy,
                    CreatedAt = productRedeem.CreatedAt,
                    UpdatedBy = productRedeem.UpdatedBy,
                    UpdatedAt = productRedeem.UpdatedAt
                });
            }
        }


        [HttpPost]
        public JsonResult EditProductRedeem(AddProductRedeemViewModel model)
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
                        var productRedeem = db.ProductRedeem.Find(model.ProductRedeemId);

                        if (model.ImagePath != null)
                        {
                            FileInfo path = new FileInfo(Server.MapPath("~/Image/Product/" + model.ImagePath));
                            path.Delete();

                            string imageName = System.IO.Path.GetFileName(model.ImagePath.FileName);
                            imageName = MetadataServices.GetDateTimeWithoutSlash() + "-" + imageName;
                            string physicalPath = Server.MapPath("~/Image/Product/" + imageName);
                            model.ImagePath.SaveAs(physicalPath);

                            productRedeem.ImagePath = imageName;
                        }

                        productRedeem.ProductRedeemName = model.ProductRedeemName;
                        productRedeem.Stock = model.Stock;
                        productRedeem.RedeemPoint = model.RedeemPoint;
                        productRedeem.UpdatedBy = MetadataServices.GetCurrentUser().Username;
                        productRedeem.UpdatedAt = MetadataServices.GetCurrentDate();

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

        public ActionResult DeleteProductRedeem(int id)
        {
            using (var db = new TourEntities())
            {
                if (ModelState.IsValid)
                {
                    var productRedeem = db.ProductRedeem.Find(id);

                    if (productRedeem == null)
                    {
                        return new HttpNotFoundResult("Product Redeem not found.");
                    }

                    FileInfo path = new FileInfo(Server.MapPath("~/Image/Product/" + productRedeem.ImagePath));
                    path.Delete();

                    db.ProductRedeem.Remove(productRedeem);

                    db.SaveChanges();
                    return RedirectToAction("ProductRedeem", "Product");
                }
                else
                {
                    return View();
                }
            }
        }

    }
}