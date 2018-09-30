using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EnjoyOurTour.Models.ViewModel.Home
{
    public class AboutUsViewModel
    {
        public long AboutUsId { get; set; }

        [Required(ErrorMessage = "Please enter About Us Title.")]
        [AllowHtml]
        public string AboutUsTitle { get; set; }

        [Required(ErrorMessage = "Please enter the About Us Description.")]
        [AllowHtml]
        public string AboutUsDescription { get; set; }

        [Required(ErrorMessage = "Please enter the About Us Short Description.")]
        [AllowHtml]
        public string AboutUsShortDescription { get; set; }
        public string YoutubeUrl { get; set; }
    }
}