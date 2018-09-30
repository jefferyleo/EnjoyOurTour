using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace EnjoyOurTour.Models.ViewModel.Admin
{
	public class EditAdminViewModel
	{
        public int UserId { get; set; }

        public string Username { get; set; }

        [Required(ErrorMessage = "Enter Email Address")]
        public string EmailAddress { get; set; }

        public string Password { get; set; }

        public int RoleId { get; set; }

        public string RoleName { get; set; }
    }
}