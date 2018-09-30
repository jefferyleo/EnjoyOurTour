using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace EnjoyOurTour.Models.ViewModel.Admin
{
    public class AddCustomerViewModel
    {
        [Required(ErrorMessage = "Enter Username")]
        public string Username { get; set; }
        [Required(ErrorMessage = "Enter Email Address")]
        public string EmailAddress { get; set; }
        public int CustomerId { get; set; }

        public string CustomerCode { get; set; }
        [Required(ErrorMessage = "Enter Customer Name")]
        public string CustomerName { get; set; }
        [Required(ErrorMessage = "Enter NRIC")]
        public string NRIC { get; set; }
        [Required(ErrorMessage = "Enter Bank Name")]
        public string BankName { get; set; }
        [Required(ErrorMessage = "Enter Bank Account Number")]
        public string BankAccountNumber { get; set; }
        [Required(ErrorMessage = "Enter Phone Number")]
        public string PhoneNumber { get; set; }
        [Required(ErrorMessage = "Enter Address")]
        public string Address { get; set; }
        [Required(ErrorMessage = "Enter Available TVR")]
        public int AvailableTVR { get; set; }
        [Required(ErrorMessage = "Enter Available Amount")]
        public decimal AvailableAmount { get; set; }
        [Required(ErrorMessage = "Enter Available Point")]
        public int AvailablePoint { get; set; }
        [Required(ErrorMessage = "Enter Agent Code")]
        public int AgentCode { get; set; }
        public string AgentName { get; set; }
        public string IntroducerCode { get; set; }
        public string IntroducerName { get; set; }
        public Nullable<int> IntroducerId { get; set; }
        [Required(ErrorMessage = "Enter Join Date")]
        public System.DateTime JoinDate { get; set; }
        public string JoinDateString { get; set; }
        public System.DateTime DateOfBirth { get; set; }
        public string DOBString { get; set; }
        public int UserId { get; set; }
    }
}