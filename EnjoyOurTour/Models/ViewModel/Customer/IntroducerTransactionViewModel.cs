using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EnjoyOurTour.Models.ViewModel.Customer
{
    public class IntroducerTransactionViewModel
    {
        public int IntroducerTransactionId { get; set; }
        public int IntroducerId { get; set; }
        public decimal CreditAmount { get; set; }
        public decimal FinalAmount { get; set; }
        public decimal DebitAmount { get; set; }
        public string CreatedBy { get; set; }
        public int? CreditTVR { get; set; }
        public int? DebitTVR { get; set; }
        public int? FinalTVR { get; set; }
        public int? CreditPoint { get; set; }
        public int? DebitPoint { get; set; }
        public int? FinalPoint { get; set; }
        public System.DateTime CreatedAt { get; set; }
    }
}