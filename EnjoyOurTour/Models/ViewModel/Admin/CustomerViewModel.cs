using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EnjoyOurTour.Models.ViewModel.Admin
{
    public class CustomerViewModel
    {
        public string Username { get; set; }
        public string EmailAddress { get; set; }
        public int CustomerId { get; set; }        
        public string CustomerCode { get; set; }
        public string CustomerName { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string NRIC { get; set; }
        public string BankName { get; set; }
        public string BankAccountNumber { get; set; }
        public string PhoneNumber { get; set; }
        public string Address { get; set; }
        public int AvailableTVR { get; set; }
        public decimal AvailableAmount { get; set; }
        public int AvailablePoint { get; set; }
        public int AgentId { get; set; }
        public string AgentName { get; set; }
        public Nullable<int> IntroducerId { get; set; }
        public System.DateTime JoinDate { get; set; }
        public int UserId { get; set; }
    }
}