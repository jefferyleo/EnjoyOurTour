using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EnjoyOurTour.Models.ViewModel.GoodNews
{
    public class AddGoodNewsViewModel
    {
        public int GoodNewsId { get; set; }

        [Required(ErrorMessage = "Enter News Title")]
        public string GoodNewsTitle { get; set; }

        public HttpPostedFileBase PhotoPath { get; set; }

        [Required(ErrorMessage = "Enter Description")]
        [AllowHtml]
        public string Description { get; set; }

        public string CreatedBy { get; set; }

        public DateTime CreatedAt { get; set; }

        public string UpdatedBy { get; set; }

        public DateTime UpdatedAt { get; set; }
    }
}