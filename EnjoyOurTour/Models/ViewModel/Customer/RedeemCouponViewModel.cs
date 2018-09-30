using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EnjoyOurTour.Models.ViewModel.Customer
{
    public class RedeemCouponViewModel
    {        
        public string CouponCode { get; set; }
        public int Amount { get; set; }
        public HttpPostedFileBase Image { get; set; }
    }
}