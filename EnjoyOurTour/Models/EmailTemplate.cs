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
    
    public partial class EmailTemplate
    {
        public int EmailTemplateId { get; set; }
        public string EmailTitle { get; set; }
        public string EmailContent { get; set; }
        public string CreatedBy { get; set; }
        public System.DateTime CreatedAt { get; set; }
        public string UpdatedBy { get; set; }
        public System.DateTime UpdatedAt { get; set; }
        public string PhotoPath { get; set; }
    }
}
