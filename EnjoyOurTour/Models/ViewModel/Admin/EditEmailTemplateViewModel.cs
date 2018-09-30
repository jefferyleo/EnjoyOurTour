using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EnjoyOurTour.Models.ViewModel.Admin
{
    public class EditEmailTemplateViewModel
    {
        public int EmailTemplateId { get; set; }
        public string EmailTemplateTitle { get; set; }
        public string EmailTitle { get; set; }
        [AllowHtml]
        public string EmailContent { get; set; }
        public string CreatedBy { get; set; }
        public HttpPostedFileBase PhotoPath { get; set; }
        public string UploadPhotoPath { get; set; }
        public DateTime CreatedAt { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime UpdatedAt { get; set; }

    }
}