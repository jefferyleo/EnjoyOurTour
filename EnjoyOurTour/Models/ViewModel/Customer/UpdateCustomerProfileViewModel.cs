using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace EnjoyOurTour.Models.ViewModel.Customer
{
    public class UpdateCustomerProfileViewModel
    {

        [Required(ErrorMessage = "Enter Email Address")]
        public string EmailAddress { get; set; }
        public int CustomerId { get; set; }

        public string CustomerCode { get; set; }
        [Required(ErrorMessage = "Enter Customer Name")]
        public string CustomerName { get; set; }
        public string NRIC { get; set; }
        [Required(ErrorMessage = "Enter Bank Name")]
        public string BankName { get; set; }
        [Required(ErrorMessage = "Enter Bank Account Number")]
        public string BankAccountNumber { get; set; }
        [Required(ErrorMessage = "Enter Phone Number")]
        public string PhoneNumber { get; set; }
        [Required(ErrorMessage = "Enter Address")]
        public string Address { get; set; }
        public int AvailableTVR { get; set; }
        public decimal AvailableAmount { get; set; }
        public int AvailablePoint { get; set; }
        public int AgentCode { get; set; }
        public string AgentName { get; set; }
        public string IntroducerCode { get; set; }
        public string IntroducerName { get; set; }
        public System.DateTime JoinDate { get; set; }
        public System.DateTime DateOfBirth { get; set; }
        public System.DateTime RenewDate { get; set; }
        public System.DateTime ExpiredDate { get; set; }

        public int UserId { get; set; }
    }
}