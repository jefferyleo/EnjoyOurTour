//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace EnjoyOurTour.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class SystemSetting
    {
        public int SystemSettingId { get; set; }
        public int MinAmount { get; set; }
        public int SignUpFees { get; set; }
        public int TVRMutiply { get; set; }
        public int IntroduceIncreaseLimit { get; set; }
        public int AgentCommision { get; set; }
        public int IntroduceNormalCommision { get; set; }
        public int IntroduceIncreaseCommision { get; set; }
        public string CreatedBY { get; set; }
        public Nullable<System.DateTime> CreadtedDate { get; set; }
        public decimal AgentCalculationBonus { get; set; }
        public int PointCalculation { get; set; }
        public int IntroduceMaxLimit { get; set; }
        public int AgentTopUpCommission { get; set; }
        public int ProcessingFeesCommision { get; set; }
    }
}
