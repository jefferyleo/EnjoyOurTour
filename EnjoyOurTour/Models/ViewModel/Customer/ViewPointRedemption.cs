using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EnjoyOurTour.Models.ViewModel.Customer
{
    public class ViewPointRedemption
    {
        public int ProductRedeemId { get; set; }
        public string ProductRedeemName { get; set; }
        public int RedeemPoint { get; set; }
        public int Stock { get; set; }
        public string ImagePath { get; set; }

    }
}