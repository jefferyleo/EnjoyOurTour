using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EnjoyOurTour.Models.ViewModel.Customer
{
    public class ViewCustomerTestimonyViewModel
    {
        public int TestimonyId { get; set; }
        public string PhotoPath { get; set; }
        public string Description { get; set; }
        public string YouTubeLink { get; set; }
        public string CreatedBy { get; set; }
        public System.DateTime CreatedAt { get; set; }
        public string UpdatedBy { get; set; }
        public System.DateTime UpdatedAt { get; set; }

    }
}