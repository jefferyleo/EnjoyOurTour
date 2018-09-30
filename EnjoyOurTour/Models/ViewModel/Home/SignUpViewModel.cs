using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EnjoyOurTour.Models.ViewModel.Home
{
    public class SignUpViewModel
    {
        [Required(ErrorMessage = "Enter Username")]
        public string Username { get; set; }
        [Required(ErrorMessage = "Enter Email Address")]
        public string EmailAddress { get; set; }
        [Required(ErrorMessage = "Enter Password")]
        public string Password { get; set; }
        [Required(ErrorMessage = "Enter Confirm Password")]
        [System.ComponentModel.DataAnnotations.Compare("Password",ErrorMessage = "Please enter the same password")]
        public string ConfirmPassword { get; set; }
        public int CustomerId { get; set; }

        public string CustomerCode { get; set; }
        [Required(ErrorMessage = "Enter Name")]
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
        [Required(ErrorMessage = "Enter Agent Code")]
        public int AgentCode { get; set; }
        public string IntroducerName { get; set; }
        public string IntroducerCode { get; set; }
        public System.DateTime JoinDate { get; set; }
        [Required(ErrorMessage = "Enter Date of Birth")]
        public System.DateTime DateOfBirth { get; set; }
        public HttpPostedFileBase Image { get; set; }
        public List<SelectListItem> TopUpAmount { get; set; }
        public string TopUpAmountValue { get; set; }
        public int OtherAmount { get; set; }
        public int UserId { get; set; }
    }
}