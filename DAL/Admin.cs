using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    [NotMapped]
    public class Admin
    {
        public Admin()
        {

        }

        public Admin(int id,string name,int planId)
        {
            Id = id;
            Name = name;
            PlanId = planId;
        }
        public int Id { get; set; }
        public string Name { get; set; }
        public int PlanId { get; set; }
        public int CustomerId { get; set; }
        public int UserId { get; set; }
        public string StoreUserName { get; set; }
        public string SessionTimeout { get; set; }
        public string EmailAddress { get; set; }
    }
}
