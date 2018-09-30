using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace EnjoyOurTour.Models.ViewModel.Agent
{
    public class EditAgentProfileViewModel
    {
        [Required(ErrorMessage = "Enter Email Address")]
        public string EmailAddress { get; set; }
        public int AgentId { get; set; }
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
        public int UserId { get; set; }
    }
}