using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EnjoyOurTour.Models.ViewModel.Admin
{
    public class TransactionViewModel
    {
        public int TransactionId { get; set; }
        public string AgentCodeName { get; set; }
        public string CustomerCodeName { get; set; }
        public string IntroducerCodeName { get; set; }
        public string ReferenceNo { get; set; }
        public int ActivityId { get; set; }
        public string ActivityName { get; set; }
        public string PathForProof { get; set; }
        public decimal Amount { get; set; }
        public int CurrentTVR { get; set; }
        public int TopUpTVR { get; set; }
        public int RedeemTVR { get; set; }
        public int BalanceTVR { get; set; }
        public int CurrentPoint { get; set; }
        public int PointAdd { get; set; }
        public int PointRedeem { get; set; }
        public int PointBalance { get; set; }
        public DateTime? ActionDate { get; set; }
        public string Remarks { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime UpdatedAt { get; set; }
        public bool IsDeleted { get; set; }
        public int TransactionStatus { get; set; }
        public string PackageName { get; set; }
        public string ProductName { get; set; }
        public int? HeadCount { get; set; }
        public string TransactionStatusName { get; set; }
    }
}