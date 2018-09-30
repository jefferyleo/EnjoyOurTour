using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EnjoyOurTour.Models.ViewModel.Package
{
    public class PackageViewModel
    {
        public int PackageId { get; set; }
        public string PackageName { get; set; }
        public string PhotoPath { get; set; }
        public string FilePath { get; set; }
        public string Description { get; set; }
        public string IteneraryDetail { get; set; }
        public int TVR { get; set; }
        public decimal Amount { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime UpdatedAt { get; set; }
        public int IsDeleted { get; set; }

    }
}