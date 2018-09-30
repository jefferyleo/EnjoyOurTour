using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EnjoyOurTour.Models.ViewModel.GoodNews
{
    public class GoodNewsViewModel
    {
        public int GoodNewsId { get; set; }
        public string GoodNewsTitle { get; set; }
        public string PhotoPath { get; set; }
        public string Description { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime UpdatedAt { get; set; }

    }
}