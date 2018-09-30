using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EnjoyOurTour.Models.ViewModel.Merchant
{
    public class MerchantPromotionViewModel
    {
        public int MerchantPromotionId { get; set; }
        public int MerchantId { get; set; }
        public string MerchantPromotionDetail { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}