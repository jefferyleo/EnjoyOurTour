using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EnjoyOurTour.Models.ViewModel.Product
{
    public class ProductRedeemViewModel
    {
        public int ProductRedeemId { get; set; }
        public string ProductRedeemName { get; set; }
        public int RedeemPoint { get; set; }
        public int Stock { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime UpdatedAt { get; set; }
        public string ImagePath { get; set; }

    }
}