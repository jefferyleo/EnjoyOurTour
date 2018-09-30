using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EnjoyOurTour.Models.ViewModel.Admin
{
    public class AgentViewModel
    {
        public string Username { get; set; }
        public string EmailAddress { get; set; }
        public int AgentId { get; set; }
        public int AgentCode { get; set; }
        public string AgentName { get; set; }
        public string NRIC { get; set; }
        public string BankAccountNumber { get; set; }
        public DateTime DOB { get; set; }

        public string BankName { get; set; }
        public string PhoneNumber { get; set; }
        public string Address { get; set; }
        public decimal Commission { get; set; }
        public decimal Bonus { get; set; }

        public int UserId { get; set; }
    }
}