using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EnjoyOurTour.Models.ViewModel.Home
{
    public class BannerViewModel
    {
        public long BannerId { get; set; }
        public HttpPostedFileBase Image { get; set; }
        public string BannerImage { get; set; }

        [AllowHtml]
        public string BannerDescription { get; set; }
        public Boolean IsActive { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime UpdatedAt { get; set; }
        public Boolean IsDeleted { get; set; }
        public string DateAdded { get; set; }
        public string Status { get; set; }
        public string BannerImageName { get; set; }
    }
}