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
    
    public partial class IntroducerTransaction
    {
        public int IntroducerTransactionId { get; set; }
        public int IntroducerId { get; set; }
        public decimal CreditAmount { get; set; }
        public decimal FinalAmount { get; set; }
        public decimal DebitAmount { get; set; }
        public string CreatedBy { get; set; }
        public System.DateTime CreatedAt { get; set; }
        public Nullable<int> CreditTVR { get; set; }
        public Nullable<int> DebitTVR { get; set; }
        public Nullable<int> FinalTVR { get; set; }
        public Nullable<int> CreditPoint { get; set; }
        public Nullable<int> DebitPoint { get; set; }
        public Nullable<int> FinalPoint { get; set; }
    }
}
