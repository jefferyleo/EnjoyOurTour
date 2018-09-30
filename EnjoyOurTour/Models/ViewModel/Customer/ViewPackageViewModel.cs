using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EnjoyOurTour.Models.ViewModel.Customer
{
    public class ViewPackageViewModel
    {
        public int PackageId { get; set; }
        public string PackageName { get; set; }
        public string PhotoPath { get; set; }
        public string FilePath { get; set; }
        public string Description { get; set; }
        public string IteneraryDetail { get; set; }
        public int TVR { get; set; }
        public decimal Amount { get; set; }
        public List<SelectListItem> PersonAmount { get; set; }
        public string PersonAmountValue { get; set; }

        public string OtherPersonAmountValue { get; set; }

        public HttpPostedFileBase Image { get; set; }

    }
}