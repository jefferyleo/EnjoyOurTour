using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EnjoyOurTour.Models.ViewModel.Package
{
    public class AddPackageViewModel
    {
        public int PackageId { get; set; }

        [Required(ErrorMessage = "Enter Package Name")]
        public string PackageName { get; set; }

        public HttpPostedFileBase PhotoPath { get; set; }
        public HttpPostedFileBase ItineraryFile { get; set; }

        [Required(ErrorMessage = "Enter Description")]
        [AllowHtml]
        public string Description { get; set; }

        [AllowHtml]
        public string IteneraryDetail { get; set; }

        [Required(ErrorMessage = "Enter Amount")]
        public int TVR { get; set; }

        [Required(ErrorMessage = "Enter TVR")]
        public decimal Amount { get; set; }

        public string CreatedBy { get; set; }

        public DateTime CreatedAt { get; set; }

        public string UpdatedBy { get; set; }

        public DateTime UpdatedAt { get; set; }

        public int IsDeleted { get; set; }
    }
}