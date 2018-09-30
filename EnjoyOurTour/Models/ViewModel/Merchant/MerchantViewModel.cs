using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EnjoyOurTour.Models.ViewModel.Merchant
{
    public class MerchantViewModel
    {
        public int MerchantId { get; set; }
        public string MerchantName { get; set; }
        public string MerchantRegisterCode { get; set; }
        public string MerchantPhoneNumber { get; set; }
        public string MerchantAddress { get; set; }
        public int MerchantCategoryId { get; set; }
        public DateTime MerchantJoinDate { get; set; }
        public string MerchantLogoPath { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}