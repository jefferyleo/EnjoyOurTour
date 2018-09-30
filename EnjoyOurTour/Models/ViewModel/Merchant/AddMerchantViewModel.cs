using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EnjoyOurTour.Models.ViewModel.Merchant
{
    public class AddMerchantViewModel
    {
        public int MerchantId { get; set; }

        [Required(ErrorMessage = "Enter Merchant Name")]
        public string MerchantName { get; set; }

        [Required(ErrorMessage = "Enter Merchant Register Code")]
        public string MerchantRegisterCode { get; set; }

        [Required(ErrorMessage = "Enter Merchant Phone Number")]
        public string MerchantPhoneNumber { get; set; }

        [Required(ErrorMessage = "Enter Merchant Address")]
        public string MerchantAddress { get; set; }

        public List<SelectListItem> MerchantCategoryId { get; set; }
    
        public int GetMerchantCategoryId { get; set; }

        [Required(ErrorMessage = "Enter Merchant Join Date")]
        public DateTime MerchantJoinDate { get; set; }

        public string displayMerchantJoinDate { get; set; }

        public HttpPostedFileBase MerchantLogoPath { get; set; }

        public string CreatedBy { get; set; }

        public DateTime CreatedAt { get; set; }

        public string UpdatedBy { get; set; }

        public DateTime UpdatedAt { get; set; }
    }
}