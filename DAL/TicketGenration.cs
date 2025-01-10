using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace DAL
{
    [Table("Tickets")]
    public class TicketGenration
    {
        [Key]
        public int TicketId { get; set; }
        //[Required(ErrorMessage = "TicketType is required.")]
        //public string TicketType { get; set; }
        [Required(ErrorMessage = "Ticket Type is required.")]
        [ForeignKey("TicketType")]
        public int TicketTypeId { get; set; }
        public virtual TicketType TicketType { get; set; }
        [Required(ErrorMessage = "Ticket Description is required.")]
        [MaxLength(300, ErrorMessage = "Ticket Description cannot be greater than 300 characters")]
        public string Description { get; set; }
        [Required(ErrorMessage = "Ticket Raised By is required.")]
        public string TicketRaisedBy { get; set; }
        [Required(ErrorMessage = "OrderNo is required.")]
        public string OrderNo { get; set; }
        // public DateTime? CreatedDate { get; set; }
        private DateTime? _date;


        //[Required]
        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? CreatedDate { get; set; }
        public int CreatedBy { get; set; }
        //  [Required(ErrorMessage = "TicketType is required.")]

        [RegularExpression(@"^\d{10}$", ErrorMessage = "Please enter a valid 10-digit phone number.")]
        public string ContactNo { get; set; }
        //public TicketGenration()
        //{
        //    // Set default values here
        //    TicketStatus = "Open";
        //}
        [DefaultValue("Open")]
        public string TicketStatus { get; set; } = "Open";
        public DateTime? ClosedDate { get; set; } 

        [NotMapped]
        public List<HttpPostedFileBase> ImageFile { get; set; }
        [NotMapped]
        public List<TicketImages> TicketImages { get; set; }
     
        //[ForeignKey("StoreMst")]
        public int StoreId { get; set; }
        //public virtual Store StoreMst { get; set; }
        //[ForeignKey("CustomerMst")]
        public int CustId { get; set; }
        //public virtual Customer CustomerMst { get; set; }
        public bool IsDeleted { get; set; }

        public string AdminRemark { get; set; }
        //public int TicketStatus { get; set; }

        //public virtual ICollection<Item> Items { get; set; }
    }
    [NotMapped]
    public class TicketDetails
    {
        public int TicketId { get; set; }
        public int TicketTypeId { get; set; }
        public string TicketType { get; set; }
        public string Description { get; set; }
        public string TicketRaisedBy { get; set; }
        public string OrderNo { get; set; }
        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? CreatedDate { get; set; }
        public int CreatedBy { get; set; }
        public string ContactNo { get; set; }
        public string TicketStatus { get; set; }
        public string TicketPendingResolveDays { get; set; }
        public int StoreId { get; set; }
        public string StoreCode { get; set; }
        public string StoreName { get; set; }
        public int CustId { get; set; }       
        public string CustomerName { get; set; }       
        public string AdminRemark { get; set; }
        public List<TicketImages> TicketImages { get; set; }
    }
    [NotMapped]
    public class TicketImages
    {
        public int TicketId { get; set; }
        public int Id { get; set; }
        public string ImagePath { get; set; }
        public string OriginalFileName { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }

    }
}
