using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EnjoyOurTour.Models.ViewModel.Home
{
    public class ContactUsViewModel
    {

        public int ContactUsId { get; set; }
        [AllowHtml]
        [Required(ErrorMessage = "Please enter Contact Us Title.")]
        public string ContactUsTitle { get; set; }
        [AllowHtml]
        [Required(ErrorMessage = "Please enter Company Name.")]
        public string CompanyName { get; set; }
        [AllowHtml]
        [Required(ErrorMessage = "Please enter Company Address.")]
        public string CompanyAddress { get; set; }
        [AllowHtml]
        [Required(ErrorMessage = "Please enter Company Phone Number.")]
        public string CompanyPhoneNumber { get; set; }
        public double CompanyLatitude { get; set; }
        public double CompanyLongitude { get; set; }
        public bool IsContactExist { get; set; }
        public string UpdatedBy { get; set; }
        public System.DateTime UpdatedAt { get; set; }
    }
}