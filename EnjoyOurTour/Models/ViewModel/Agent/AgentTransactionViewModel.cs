using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EnjoyOurTour.Models.ViewModel.Agent
{
    public class AgentTransactionViewModel
    {
        public int AgentTransactionId { get; set; }
        public int AgentId { get; set; }
        public decimal CreditAmount { get; set; }
        public decimal FinalAmount { get; set; }
        public decimal CreditBonus { get; set; }
        public decimal FinalBonus { get; set; }
        public decimal DebitAmount { get; set; }
        public decimal DebitBonus { get; set; }
        public string CreatedBy { get; set; }
        public System.DateTime CreatedAt { get; set; }
    }
}