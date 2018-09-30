using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EnjoyOurTour.Models.ViewModel.Product
{
    public class AddProductRedeemViewModel
    {
        public int ProductRedeemId { get; set; }

        [Required(ErrorMessage = "Enter Product Redeem Name")]
        public string ProductRedeemName { get; set; }

        [Required(ErrorMessage = "Enter Redeem Point")]
        public int RedeemPoint { get; set; }

        [Required(ErrorMessage = "Enter Stock Count")]
        public int Stock { get; set; }

        public string CreatedBy { get; set; }

        public DateTime CreatedAt { get; set; }

        public string UpdatedBy { get; set; }

        public DateTime UpdatedAt { get; set; }

        public HttpPostedFileBase ImagePath { get; set; }
    }
}