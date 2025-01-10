using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    [NotMapped]
    public class StoreLogin
    {
        public int Id { get; set; }
        public int PlanId { get; set; }
        public int CustomerId { get; set; }
        public string Password { get; set; }
        public string StoreName { get; set; }
        public int UserId { get; set; }
        public int RoleId { get; set; }
        public string MkrChkrFlag { get; set; }
        public DateTime ModifiedDate { get; set; }
        public string Error { get; set; }
        public string StoreUserName { get; set; }
        public string SessionTimeout { get; set; }
        public string EmailAddress { get; set; }
        //public DateTime LoginTime { get; set; }
        //public DateTime LogOutTime { get; set; }
        //public bool IsLogin { get; set; }
    }
    [NotMapped]
    public class OTPViewModel
    {
        [Required]
        public int UserId { get; set; }
        [Required]
        public string OTP { get; set; }
    }
    [NotMapped]
    public class ReturnResponse
    {
        public bool Status { get; set; }
        public string UserId { get; set; }
    }

    public class UserSession
    {
        public int StoreId { get; set; }
        public int PlanId { get; set; }
        public int CustomerId { get; set; }
        public string Password { get; set; }
        public string StoreName { get; set; }
        public int UserId { get; set; }
        public int RoleId { get; set; }
        public string MkrChkrFlag { get; set; }
        public DateTime ModifiedDate { get; set; }
        public string Error { get; set; }
        public string UserName { get; set; }
        public string SessionTimeout { get; set; }
        public string EmailAddress { get; set; }
        public string OTP { get; set; }
    }
}
