using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace EnjoyOurTour.Models.ViewModel.Admin
{
    public class AddAgentViewModel
    {
        [Required(ErrorMessage = "Enter Username")]
        public string Username { get; set; }
        [Required(ErrorMessage = "Enter Email Address")]
        public string EmailAddress { get; set; }
        public int AgentId { get; set; }

        public int AgentCode { get; set; }
        [Required(ErrorMessage = "Enter Customer Name")]
        public string AgentName { get; set; }
        [Required(ErrorMessage = "Enter NRIC")]
        public string NRIC { get; set; }
        [Required(ErrorMessage = "Enter Bank Account Number")]
        public string BankAccountNumber { get; set; }
        [Required(ErrorMessage = "Enter Date Of Birth")]
        public System.DateTime DOB { get; set; }
        public string DOBString { get; set; }
        [Required(ErrorMessage = "Enter Bank Name")]
        public string BankName { get; set; }
        [Required(ErrorMessage = "Enter Phone Number")]
        public string PhoneNumber { get; set; }
        [Required(ErrorMessage = "Enter Address")]
        public string Address { get; set; }
        [Required(ErrorMessage = "Enter Bonus")]
        public decimal Bonus { get; set; }
        [Required(ErrorMessage = "Enter Commission")]
        public decimal Commission { get; set; }
        public int UserId { get; set; }
    }
}