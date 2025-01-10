using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace StoreOrderingDashBoard.Models
{
    public class Login
    {
        [Required(ErrorMessage = "please enter username")]
        public string Username { get; set; }

        public string Password { get; set; }
        public string ForgotAnswers { get; set; }
        public string Expires { get; set; }
    }
}