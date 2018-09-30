using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EnjoyOurTour.Models.ViewModel.Home
{
    public class HomeViewModel
    {
        public List<BannerViewModel> Banners { get; set; }
        public AboutUsViewModel AboutUs { get; set; }
        public ContactUsViewModel ContactUs { get; set; }
    }
}