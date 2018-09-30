using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EnjoyOurTour.Models.ViewModel.TravelPromotion
{
    public class TravelPromotionViewModel
    {
        public long TravelPromotionId { get; set; }
        [Required]
        [AllowHtml]
        public string TravelPromotionTitle { get; set; }
        public HttpPostedFileBase Image { get; set; }
        public string PhotoPath { get; set; }

        [AllowHtml]
        public string Description { get; set; }       
        public string CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}