using EnjoyOurTour.Helpers;
using EnjoyOurTour.Helpers.Services;
using EnjoyOurTour.Models;
using EnjoyOurTour.Models.ViewModel.Admin;
using EnjoyOurTour.Models.ViewModel.Customer;
using EnjoyOurTour.Models.ViewModel.GoodNews;
using EnjoyOurTour.Models.ViewModel.Package;
using EnjoyOurTour.Models.ViewModel.Product;
using EnjoyOurTour.Models.ViewModel.TravelPromotion;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Web.Mvc;
using X.PagedList;

namespace EnjoyOurTour.Controllers
{
    public class CustomerController : Controller
    {
        // GET: Customer
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult TravelPromotion(int? page)
        {
            using (var db = new TourEntities())
            {
                var pageNumber = page ?? 1;
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
                ViewBag.OnePageOfProducts = travelPromotion.ToPagedList(pageNumber, 9);
                return View();
            }
        }

        public ActionResult PointRedemption(int? page)
        {
            using (var db = new TourEntities())
            {
                var pageNumber = page ?? 1;
                List<ProductRedeemViewModel> product = (from data in db.ProductRedeem
                                                  select new ProductRedeemViewModel()
                                                  {
                                                      ProductRedeemId = data.ProductRedeemId,
                                                      ProductRedeemName = data.ProductRedeemName,
                                                      RedeemPoint = data.RedeemPoint,
                                                      ImagePath = data.ImagePath                                                                                                            
                                                  }).ToList();

                ViewBag.OnePageOfProducts = product.ToPagedList(pageNumber, 9);
                return View();
            }
        }

        public ActionResult ViewPointRedemption(int productId)
        {
            using (var db = new TourEntities())
            {
                var product = db.ProductRedeem.Where(x => x.ProductRedeemId == productId).FirstOrDefault();

                ViewPointRedemption pointRedemptionViewModel = new ViewPointRedemption();

                pointRedemptionViewModel.ProductRedeemId = product.ProductRedeemId;
                pointRedemptionViewModel.RedeemPoint = product.RedeemPoint;
                pointRedemptionViewModel.ProductRedeemName = product.ProductRedeemName;
                pointRedemptionViewModel.RedeemPoint = product.RedeemPoint;
                pointRedemptionViewModel.ImagePath = product.ImagePath;                           

                return View(pointRedemptionViewModel);
            }
        }

        [HttpPost]
        public JsonResult RedeemPoint(ViewPointRedemption model)
        {
            try
            {
                using (var db = new TourEntities())
                {
                    var product = db.ProductRedeem.Where(x => x.ProductRedeemId == model.ProductRedeemId).FirstOrDefault();
                    if (product == null)
                    {
                        ModelState.AddModelError("ProductRedeemId", "Product Redeem is not found.");
                    }

                    var user = MetadataServices.GetCurrentUser();
                    var customer = db.Customer.Where(x => x.UserId == user.UserId).FirstOrDefault();

                    if (customer.AvailablePoint < model.RedeemPoint)
                    {
                        ModelState.AddModelError("RedeemPoint", "Not Enough Redeem Point.");
                    }


                    if (ModelState.IsValid)
                    {
                        int balancePoint = (customer.AvailablePoint - product.RedeemPoint);

                        var userTransaction = new UserTransaction()
                        {
                            AgentId = customer.AgentId,
                            CustomerId = customer.CustomerId,
                            IntroducerId = customer.IntroducerId,
                            ReferenceNo = MetadataServices.GenerateCode(6),
                            ActivityId = (int)Helpers.TransactionActivity.RedeemPoint,
                            Amount = customer.AvailableAmount,
                            PathForProof = string.Empty,
                            CurrentTVR = customer.AvailableTVR,
                            TopUpTVR = 0,
                            RedeemTVR = 0,
                            BalanceTVR = 0,
                            CurrentPoint = customer.AvailablePoint,
                            PointAdd = 0,
                            PointRedeem = product.RedeemPoint,
                            PointBalance = balancePoint,
                            ActionDate = null,
                            Remarks = "",
                            ProductId = model.ProductRedeemId,
                            CreatedAt = MetadataServices.GetCurrentDate(),
                            CreatedBy = user.Username,
                            UpdatedAt = MetadataServices.GetCurrentDate(),
                            UpdatedBy = user.Username,
                            IsDeleted = false,
                            TransactionStatus = (int)TransactionStatus.Pending
                        };

                        db.UserTransaction.Add(userTransaction);

                        var customerTransaction = new IntroducerTransaction()
                        {
                            IntroducerId = customer.CustomerId,
                            CreditTVR = 0,
                            FinalTVR = customer.AvailableTVR,
                            DebitTVR = 0,
                            CreditAmount = 0,
                            FinalAmount = customer.AvailableAmount,
                            DebitAmount = 0,
                            CreditPoint = 0,
                            FinalPoint = balancePoint,
                            DebitPoint = product.RedeemPoint,
                            CreatedBy = user.Username,
                            CreatedAt = DateTime.Now
                        };

                        db.IntroducerTransaction.Add(customerTransaction);

                        customer.AvailablePoint = balancePoint;

                        db.SaveChanges();
                        return Json(new { success = true }, JsonRequestBehavior.AllowGet);
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

     
        public ActionResult Package(int? page)
        {
            using (var db = new TourEntities())
            {
                var pageNumber = page ?? 1;
                List<PackageViewModel> package = (from data in db.Package where data.IsDeleted == false
                                                                  select new PackageViewModel()
                                                                  {
                                                                      PackageId = data.PackageId,
                                                                      PackageName = data.PackageName,
                                                                      Description = data.Description,
                                                                      PhotoPath = data.PhotoPath,
                                                                      FilePath = data.FilePath,
                                                                      TVR = data.TVR,
                                                                      Amount = data.Amount
                                                                  }).ToList();

                ViewBag.OnePageOfProducts = package.ToPagedList(pageNumber, 9);
                return View();
            }
        }

        public ActionResult ViewPackageDetail(int packageId)
        {
            using (var db = new TourEntities())
            {
                var package = db.Package.Where(x => x.PackageId == packageId).FirstOrDefault();

                ViewPackageViewModel packageViewModel = new ViewPackageViewModel();

                packageViewModel.PackageId = package.PackageId;
                packageViewModel.PackageName = package.PackageName;
                packageViewModel.Description = package.Description;
                packageViewModel.IteneraryDetail = package.IteneraryDetail;
                packageViewModel.PhotoPath = package.PhotoPath;
                packageViewModel.FilePath = package.FilePath;
                packageViewModel.TVR = package.TVR;
                packageViewModel.Amount = package.Amount;

                List<string> personAmount = new List<string>();

                for (int i = 1; i <= 5; i++)
                {
                    personAmount.Add(i.ToString());
                }

                personAmount.Add("Other");

                packageViewModel.PersonAmount = personAmount.ToList().Select(e => new SelectListItem()
                {
                    Text = e.ToString(),
                    Value = e.ToString(),
                }).ToList();

                return View(packageViewModel);
            }
        }

        public FileResult DownloadPDF(string file)
        {
            string path = AppDomain.CurrentDomain.BaseDirectory + "Files/";
            byte[] fileBytes = System.IO.File.ReadAllBytes(path + file);
            string fileName = file;
            return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, fileName);
        }

        [HttpPost]
        public JsonResult RedeemPackage(ViewPackageViewModel model)
        {
            try
            {
                using (var db = new TourEntities())
                {
                    int amountHeadCount = 0;

                    if (model.PersonAmountValue == "Other")
                    {
                        if (Convert.ToInt32(model.OtherPersonAmountValue) == 0 || model.OtherPersonAmountValue == null)
                        {
                            ModelState.AddModelError("OtherPersonAmountValue", "Other Headcount amount cannot be 0 or empty");
                        }
                        else
                        {
                            amountHeadCount = Convert.ToInt32(model.OtherPersonAmountValue);
                        }
                    }
                    else
                    {
                        amountHeadCount = Convert.ToInt32(model.PersonAmountValue);
                    }

                    var package = db.Package.FirstOrDefault(e => e.PackageId == model.PackageId);
                    if (package == null)
                    {
                        ModelState.AddModelError("PackageId", "Package is not found.");
                    }

                    var user = MetadataServices.GetCurrentUser();
                    var customer = db.Customer.Where(x => x.UserId == user.UserId).FirstOrDefault();

                    decimal totalAmount = amountHeadCount * package.Amount;
                    int totalTVR = amountHeadCount * package.TVR;

                    if (customer.AvailableTVR < totalTVR)
                    {
                        ModelState.AddModelError("TVR", "Your current available is not enough. Please proceed to top up.");
                    }

                    if (ModelState.IsValid)
                    {
                        string imageName = "";

                        if (model.Image != null)
                        {
                            imageName = System.IO.Path.GetFileName(model.Image.FileName);
                            imageName = MetadataServices.GetDateTimeWithoutSlash() + "-" + imageName;
                            string physicalPath = Server.MapPath("~/Image/TransactionImage/" + imageName);
                            model.Image.SaveAs(physicalPath);
                        }

                        int balanceTVR = (customer.AvailableTVR - totalTVR);
                        var userTransaction = new UserTransaction()
                        {
                            AgentId = customer.AgentId,
                            CustomerId = customer.CustomerId,
                            IntroducerId = customer.IntroducerId,
                            ReferenceNo = MetadataServices.GenerateCode(6),
                            ActivityId = (int)Helpers.TransactionActivity.RedeemPackage,
                            PathForProof = imageName,
                            Amount = customer.AvailableAmount,
                            CurrentTVR = customer.AvailableTVR,
                            TopUpTVR = 0,
                            RedeemTVR = totalTVR,
                            BalanceTVR = balanceTVR,
                            CurrentPoint = customer.AvailablePoint,
                            PointAdd = 0,
                            PointRedeem = 0,
                            PointBalance = 0,
                            ActionDate = null,
                            Remarks = "",
                            PackageId = model.PackageId,
                            TravelHeadCount = amountHeadCount,
                            CreatedAt = MetadataServices.GetCurrentDate(),
                            CreatedBy = user.Username,
                            UpdatedAt = MetadataServices.GetCurrentDate(),
                            UpdatedBy = user.Username,
                            IsDeleted = false,
                            TransactionStatus = (int)TransactionStatus.Pending
                        };

                        db.UserTransaction.Add(userTransaction);

                        var customerTransaction = new IntroducerTransaction()
                        {
                            IntroducerId = customer.CustomerId,
                            CreditTVR = 0,
                            FinalTVR = balanceTVR,
                            DebitTVR = totalTVR,
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

                        customer.AvailableTVR = customer.AvailableTVR - totalTVR;

                        db.SaveChanges();

                        return Json(new { success = true }, JsonRequestBehavior.AllowGet);
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

        public ActionResult CustomerTestimony(int? page)
        {
            using (var db = new TourEntities())
            {
                var pageNumber = page ?? 1;
                List<CustomersTestimonyViewModel> testimony = (from data in db.CustomersTestimony                                                  
                                                  select new CustomersTestimonyViewModel()
                                                  {
                                                      TestimonyId = data.TestimonyId,
                                                      PhotoPath = data.PhotoPath,
                                                      Description = data.Description,
                                                  }).ToList();

                ViewBag.OnePageOfProducts = testimony.ToPagedList(pageNumber, 9);
                return View();
            }
        }


        public ActionResult ViewCustomerTestimony(int testimonyId)
        {
            using (var db = new TourEntities())
            {
                var testimony = db.CustomersTestimony.Where(x => x.TestimonyId == testimonyId).FirstOrDefault();

                ViewCustomerTestimonyViewModel testimonyViewModel = new ViewCustomerTestimonyViewModel();

                testimonyViewModel.TestimonyId = testimony.TestimonyId;                
                testimonyViewModel.Description = testimony.Description;
                testimonyViewModel.YouTubeLink = testimony.YoutubeLink;
                testimonyViewModel.PhotoPath = testimony.PhotoPath;

                return View(testimonyViewModel);
            }
        }

        public ActionResult GoodNews(int? page)
        {
            using (var db = new TourEntities())
            {
                var pageNumber = page ?? 1;
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

                ViewBag.OnePageOfProducts = goodNews.ToPagedList(pageNumber, 9);
                return View();
            }
        }

        public ActionResult IntroducerTransaction(int userId)
        {
            using (var db = new TourEntities())
            {
                var customer = db.Customer.Where(x => x.UserId == userId).FirstOrDefault();

                List<IntroducerTransactionViewModel> introducerTransaction = (from data in db.IntroducerTransaction
                                                                              where data.IntroducerId == customer.CustomerId
                                                                              select new IntroducerTransactionViewModel()
                                                                              {
                                                                                  IntroducerTransactionId = data.IntroducerTransactionId,
                                                                                  IntroducerId = data.IntroducerId,
                                                                                  CreditAmount = data.CreditAmount,
                                                                                  FinalAmount = data.FinalAmount,
                                                                                  DebitAmount = data.DebitAmount,
                                                                                  CreditTVR = data.CreditTVR,
                                                                                  DebitTVR = data.DebitTVR,
                                                                                  FinalTVR = data.FinalTVR,
                                                                                  CreditPoint = data.CreditPoint,
                                                                                  DebitPoint = data.DebitPoint,
                                                                                  FinalPoint = data.FinalPoint,
                                                                                  CreatedAt = data.CreatedAt,
                                                                                  CreatedBy = data.CreatedBy
                                                                              }).ToList();
                return View(introducerTransaction);
            }
        }

        public ActionResult UpdateCustomerProfile(int userId)
        {
            using (var db = new TourEntities())
            {

                var customer = db.User_Customer_Adgent.Where(x => x.UserId == userId).FirstOrDefault();

                UpdateCustomerProfileViewModel customerViewModel = new UpdateCustomerProfileViewModel();

                customerViewModel.NRIC = customer.NRIC;
                customerViewModel.CustomerId = customer.CustomerId;
                customerViewModel.CustomerName = customer.CustomerName;
                customerViewModel.EmailAddress = customer.EmailAddress;
                customerViewModel.BankAccountNumber = customer.BankAccountNumber;
                customerViewModel.BankName = customer.BankName;
                customerViewModel.PhoneNumber = customer.PhoneNumber;
                customerViewModel.Address = customer.Address;
                customerViewModel.DateOfBirth = customer.DateOfBirth;
                customerViewModel.CustomerCode = customer.CustomerCode;
                customerViewModel.RenewDate = customer.RenewDate;
                customerViewModel.ExpiredDate = customer.ExpiredDate;
                customerViewModel.AvailableAmount = customer.AvailableAmount;
                customerViewModel.AvailablePoint = customer.AvailablePoint;
                customerViewModel.AvailableTVR = customer.AvailableTVR;
                customerViewModel.AgentCode = customer.AgentCode;
                customerViewModel.AgentName = customer.AgentName;
                customerViewModel.IntroducerCode = customer.IntroduceCode;
                customerViewModel.IntroducerName = customer.IntroduceName;


                return View(customerViewModel);
            }
        }

        [HttpPost]
        public JsonResult UpdateCustomerProfile(UpdateCustomerProfileViewModel model)
        {
            using (var db = new TourEntities())
            {
                var userWithNRIC = db.Customer.Where(x => x.NRIC == model.NRIC && x.CustomerId != model.CustomerId).FirstOrDefault();

                if (userWithNRIC != null)
                {
                    ModelState.AddModelError("NRIC", "NRIC Duplication");
                }

                if (ModelState.IsValid)
                {
                    var customer = db.Customer.Where(x => x.CustomerId == model.CustomerId).FirstOrDefault();

                    customer.CustomerName = model.CustomerName;
                    customer.NRIC = model.NRIC;
                    customer.PhoneNumber = model.PhoneNumber;
                    customer.Address = model.Address;
                    customer.BankAccountNumber = model.BankAccountNumber;
                    customer.BankName = model.BankName;
                    customer.DateOfBirth = model.DateOfBirth;

                    var user = db.User.Where(x => x.UserId == customer.UserId).FirstOrDefault();
                    user.EmailAddress = model.EmailAddress;

                    db.SaveChanges();


                    return Json(new { success = true }, JsonRequestBehavior.AllowGet);
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
                    return Json(new { success = false, issue = model, errors = errors }, JsonRequestBehavior.AllowGet);
                }

            }
        }


        public ActionResult TopUpCustomer()
        {
            using (var db = new TourEntities())
            {

                TopUpViewModel topUpViewModel = new TopUpViewModel();
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

                topUpViewModel.TopUpAmount = topUpAmount.ToList().Select(e => new SelectListItem()
                {
                    Text = e.ToString(),
                    Value = e.ToString(),
                }).ToList();

                User user = MetadataServices.GetCurrentUser();

                var customer_Agent = db.User_Customer_Adgent.Where(x => x.UserId == user.UserId).FirstOrDefault();

                topUpViewModel.UserId = user.UserId;
                topUpViewModel.AgentCode = customer_Agent.AgentCode;
                topUpViewModel.AgentName = customer_Agent.AgentName;
                topUpViewModel.AvailableAmount = customer_Agent.AvailableAmount;
                topUpViewModel.AvailableTVR = customer_Agent.AvailableTVR;
                topUpViewModel.AvailablePoint = customer_Agent.AvailablePoint;
                topUpViewModel.NRIC = customer_Agent.NRIC;
                topUpViewModel.CustomerName = customer_Agent.CustomerName;


                return View(topUpViewModel);
            }
        }

        [HttpPost]
        public JsonResult TopUpCustomer(TopUpViewModel topUpVieModel)
        {
            using (var db = new TourEntities())
            {
                int amount = 0;

                var systemSetting = db.SystemSetting.OrderByDescending(x => x.CreadtedDate).FirstOrDefault();

                if (topUpVieModel.TopUpAmountValue == "Other")
                {
                    if ((topUpVieModel.OtherAmount % systemSetting.MinAmount) != 0 || topUpVieModel.OtherAmount == 0)
                    {
                        ModelState.AddModelError("OtherAmount", "Other Amount not multiply of " + systemSetting.MinAmount);
                    }
                    else
                    {
                        amount = topUpVieModel.OtherAmount;
                    }
                }
                else
                {
                    amount = Convert.ToInt32(topUpVieModel.TopUpAmountValue);
                }

                if (ModelState.IsValid)
                {
                    var user = MetadataServices.GetCurrentUser();
                    var customer = db.Customer.Where(x => x.UserId == user.UserId).FirstOrDefault();

                    string imageName = "";

                    if (topUpVieModel.Image != null)
                    {
                        imageName = System.IO.Path.GetFileName(topUpVieModel.Image.FileName);
                        imageName = MetadataServices.GetDateTimeWithoutSlash() + "-" + imageName;
                        string physicalPath = Server.MapPath("~/Image/TransactionImage/" + imageName);
                        topUpVieModel.Image.SaveAs(physicalPath);
                    }

                    var userTransaction = new UserTransaction()
                    {
                        AgentId = customer.AgentId,
                        CustomerId = customer.CustomerId,
                        IntroducerId = customer.IntroducerId,
                        ReferenceNo = MetadataServices.GenerateCode(6),
                        ActivityId = (int)Helpers.TransactionActivity.TopUp,
                        PathForProof = imageName,
                        Amount = amount,
                        CurrentTVR = customer.AvailableTVR,
                        TopUpTVR = systemSetting.TVRMutiply * amount,
                        RedeemTVR = 0,
                        BalanceTVR = 0,
                        CurrentPoint = customer.AvailablePoint,
                        PointAdd = ((amount / systemSetting.MinAmount) * systemSetting.PointCalculation),
                        PointRedeem = 0,
                        PointBalance = 0,
                        ActionDate = null,
                        Remarks = "",
                        CreatedAt = MetadataServices.GetCurrentDate(),
                        CreatedBy = MetadataServices.GetCurrentUser().Username,
                        UpdatedAt = MetadataServices.GetCurrentDate(),
                        UpdatedBy = MetadataServices.GetCurrentUser().Username,
                        IsDeleted = false,
                        TransactionStatus = (int)TransactionStatus.Pending
                    };        


                    db.UserTransaction.Add(userTransaction);
                    db.SaveChanges();
                    return Json(new { success = true }, JsonRequestBehavior.AllowGet);
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

        public ActionResult RedeemCoupon()
        {
            using (var db = new TourEntities())
            {

                RedeemCouponViewModel redeemCouponViewModel = new RedeemCouponViewModel();

                return View(redeemCouponViewModel);
            }
        }


        [HttpPost]
        public JsonResult RedeemCoupon(RedeemCouponViewModel model)
        {
            using (var db = new TourEntities())
            {
                var couponTransaction = db.RedeemCoupon.Where(x => x.CouponCode == model.CouponCode).FirstOrDefault();

                if (couponTransaction != null)
                {
                    ModelState.AddModelError("CouponCode", "Coupon Code had been redeem before.");
                }

                if (ModelState.IsValid)
                {
                    string imageName = "";
                    RedeemCoupon newCoupon = new RedeemCoupon();

                    if (model.Image != null)
                    {
                        imageName = System.IO.Path.GetFileName(model.Image.FileName);
                        imageName = MetadataServices.GetDateTimeWithoutSlash() + "-" + imageName;
                        string physicalPath = Server.MapPath("~/Image/RedeemCoupon/" + imageName);
                        model.Image.SaveAs(physicalPath);
                    }

                    var currentUser = MetadataServices.GetCurrentUser();

                    var customer = db.Customer.Where(x => x.UserId == currentUser.UserId).FirstOrDefault();

                    newCoupon.CouponCode = model.CouponCode;
                    newCoupon.TVRAmount = model.Amount;
                    newCoupon.ImageProof = imageName;
                    newCoupon.CustomerId = customer.CustomerId;
                    newCoupon.CreatedBy = currentUser.Username;
                    newCoupon.CreatedAt = DateTime.Now;
                    newCoupon.UpdatedAt = DateTime.Now;
                    newCoupon.UpdatedBy = currentUser.Username;
                    newCoupon.TransactionStatus = (int)EnjoyOurTour.Helpers.TransactionStatus.Pending;

                    db.RedeemCoupon.Add(newCoupon);
                    db.SaveChanges();

                    return Json(new { success = true }, JsonRequestBehavior.AllowGet);
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
    }
}