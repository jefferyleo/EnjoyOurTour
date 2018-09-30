using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace EnjoyOurTour.Models.ViewModel.Account
{
    public class LoginViewModel
    {
        public long UserId { get; set; }

        [Required(ErrorMessage = "Username Required", AllowEmptyStrings = false)]
        public string Username { get; set; }

        [Required(ErrorMessage = "Password Required", AllowEmptyStrings = false)]
        public string Password { get; set; }
        public long RoleId { get; set; }
    }
}