using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EnjoyOurTour.Models.ViewModel.Customer
{
    public class TopUpViewModel
    {
        public int CustomerId { get; set; }
        public int AgentCode { get; set; }
        public string AgentName { get; set; }
        public int AvailableTVR { get; set; }
        public decimal AvailableAmount { get; set; }
        public int AvailablePoint { get; set; }
        public HttpPostedFileBase Image { get; set; }
        public List<SelectListItem> TopUpAmount { get; set; }
        [Required(ErrorMessage = "Enter Date of Birth")]
        public string TopUpAmountValue { get; set; }
        public string NRIC { get; set; }
        public string CustomerName { get; set; }
        public int OtherAmount { get; set; }
        public int UserId { get; set; }
    }
}