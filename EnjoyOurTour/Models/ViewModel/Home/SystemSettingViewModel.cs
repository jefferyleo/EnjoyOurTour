using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace EnjoyOurTour.Models.ViewModel.Admin
{
    public class SystemSettingViewModel
    {
        [Required(ErrorMessage = "Please enter Min amount for Top Up")]
        public int  MinAmount { get; set; }
        [Required(ErrorMessage = "Please enter sign up fees.")]
        public int SignUpFees { get; set; }
        [Required(ErrorMessage = "Please enter TVR multiply fomular")]
        public int TVRMutiply { get; set; }
        [Required(ErrorMessage = "Please enter Introduce Increase Limit")]      
        public int IntroduceIncreaseLimit { get; set; }
        [Required(ErrorMessage = "Please enter Introduce Max Limit")]
        public int IntroduceMaxLimits { get; set; }
        [Required(ErrorMessage = "Please enter Agent Commission")]
        public int  AgentCommision { get; set; }
        [Required(ErrorMessage = "Please enter Introduce Normal Commission")]
        public int IntroduceNormalCommision { get; set; }
        [Required(ErrorMessage = "Please enter Introduce Increase Commision")]
        public int IntroduceIncreaseCommision { get; set; }
        [Required(ErrorMessage = "Please enter Agent Bonus Calculate Formula")]
        public decimal AgentBonusCalculateFormula { get; set; }

        [Required(ErrorMessage = "Please enter Member Point Calculate Formula")]
        public int MemberPointCalculateFormula { get; set; }
        [Required(ErrorMessage = "Please enter Agent Top Up Commision")]
        public int AgentTopUpCommision { get; set; }
        [Required(ErrorMessage = "Please enter Processing Fees Commision")]
        public int ProcessingFeesCommision { get; set; }

    }
}