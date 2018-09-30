using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace EnjoyOurTour.Models.ViewModel.Merchant
{
    public class AddMerchantCategoryViewModel
    {
        public int MerchantCategoryId { get; set; }

        [Required(ErrorMessage = "Enter Merchant Category Name")]
        public string CategoryName { get; set; }

        public string CreatedBy { get; set; }

        public DateTime CreatedAt { get; set; }

        public string UpdatedBy { get; set; }

        public DateTime UpdatedAt { get; set; }
    }
}