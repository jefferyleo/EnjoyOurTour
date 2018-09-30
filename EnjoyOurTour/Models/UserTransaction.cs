//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace EnjoyOurTour.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class UserTransaction
    {
        public int TransactionId { get; set; }
        public int AgentId { get; set; }
        public int CustomerId { get; set; }
        public Nullable<int> IntroducerId { get; set; }
        public string ReferenceNo { get; set; }
        public int ActivityId { get; set; }
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
        public Nullable<System.DateTime> ActionDate { get; set; }
        public string Remarks { get; set; }
        public string CreatedBy { get; set; }
        public System.DateTime CreatedAt { get; set; }
        public string UpdatedBy { get; set; }
        public System.DateTime UpdatedAt { get; set; }
        public bool IsDeleted { get; set; }
        public int TransactionStatus { get; set; }
        public Nullable<int> PackageId { get; set; }
        public Nullable<int> TravelHeadCount { get; set; }
        public Nullable<int> ProductId { get; set; }
    }
}
