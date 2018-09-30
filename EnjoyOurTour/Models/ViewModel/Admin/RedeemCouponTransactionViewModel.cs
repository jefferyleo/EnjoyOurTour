using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EnjoyOurTour.Models.ViewModel.Admin
{
    public class RedeemCouponTransactionViewModel
    {
        public int RedeemCouponId { get; set; }
        public string CouponCode { get; set; }
        public int TVRAmount { get; set; }
        public int TransactionStatusId { get; set; }
        public string StatusName { get; set; }
        public int CustomerId { get; set; }
        public string CustomerName { get; set; }
        public string ImageProof { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}