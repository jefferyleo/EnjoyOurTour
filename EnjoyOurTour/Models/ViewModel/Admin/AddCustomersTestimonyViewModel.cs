using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EnjoyOurTour.Models.ViewModel.Admin
{
    public class AddCustomersTestimonyViewModel
    {
        public int TestimonyId { get; set; }
        
        public HttpPostedFileBase PhotoPath { get; set; }

        public string PhotoPathUpload { get; set; }

        [Required(ErrorMessage = "Please enter Description.")]
        [AllowHtml]
        public string Description { get; set; }

        public string YoutubeLink { get; set; }

        public string CreatedBy { get; set; }

        public System.DateTime CreatedAt { get; set; }

        public string UpdatedBy { get; set; }

        public System.DateTime UpdatedAt { get; set; }
    }
}