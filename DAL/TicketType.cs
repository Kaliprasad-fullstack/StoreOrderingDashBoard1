using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public class TicketType
    {
        [Key]
        public int TicketTypeId { get; set; }
        public string TicketTypeName { get; set; }
        public virtual ICollection<TicketGenration> Tickets { get; set; }
    }
    public enum TicketStatus
    {
        Open, Resolved,Closed
    }
}
