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
    
    public partial class Agent
    {
        public int AgentId { get; set; }
        public int AgentCode { get; set; }
        public string AgentName { get; set; }
        public string NRIC { get; set; }
        public System.DateTime DOB { get; set; }
        public string BankName { get; set; }
        public string BankAccountNumber { get; set; }
        public string PhoneNumber { get; set; }
        public string Address { get; set; }
        public decimal Commission { get; set; }
        public decimal Bonus { get; set; }
        public int UserId { get; set; }
        public string CreatedBy { get; set; }
        public System.DateTime CreatedAt { get; set; }
        public string UpdatedBy { get; set; }
        public System.DateTime UpdatedAt { get; set; }
        public bool IsDeleted { get; set; }
    }
}
