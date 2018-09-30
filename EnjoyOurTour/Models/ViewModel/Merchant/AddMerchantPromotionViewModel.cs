using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EnjoyOurTour.Models.ViewModel.Merchant
{
    public class AddMerchantPromotionViewModel
    {
        public int MerchantPromotionId { get; set; }

        public int MerchantId { get; set; }

        public List<SelectListItem> MerchantIdList { get; set; }

        public string MerchantName { get; set; }

        [Required(ErrorMessage = "Enter Merchant Promotion Detail")]
        [AllowHtml]
        public string MerchantPromotionDetail { get; set; }

        public string CreatedBy { get; set; }

        public DateTime CreatedAt { get; set; }

        public string UpdatedBy { get; set; }

        public DateTime UpdatedAt { get; set; }
    }
}