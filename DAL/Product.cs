using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    //[Table("ProductMst")]
    //public class Product
    //{
    //    [Key]
    //    public int Id { get; set; }
    //    public string ProductName { get; set; }
    //    public string ProductCode { get; set; }
    //    public int? MaxOrderLimit { get; set; }
    //    public bool Isdeleted { get; set; }
    //    public DateTime? CreatedOn { get; set; }
    //    public int? CreatedBy { get; set; }
    //    public DateTime? ModifiedOn { get; set; }
    //    public int? ModifiedBy { get; set; }
    //    public virtual UOMmaster UOMmaster { get; set; }
    //    public virtual Category Category { get; set; }
    //    public virtual SubCategory SubCategory { get; set; }
    //    [NotMapped]
    //    public string ProductType { get; set; }
    //    //Gets data USP_GetProducts_Items
    //}
    //[NotMapped]
    //public class ItemProduct
    //{
    //    public int Id { get; set; }
    //    public string SKUName { get; set; }
    //    public string SKUCode { get; set; }
    //    public decimal? MaximumOrderLimit { get; set; }
    //    public bool Isdeleted { get; set; }
    //    public virtual string UnitOfMeasurement { get; set; }
    //    public virtual string CaseConversion { get; set; }
    //    public string Type { get; set; }
    //}
    //[NotMapped]
    //public class OrderSave
    //{
    //    public int ProductId { get; set; }
    //    public int Qty { get; set; }
    //    public int storeId { get; set; }
    //    public long OrderheaderId { get; set; }
    //    public int DCID { get; set; }
    //    public int UserId { get; set; }
    //    public string Type { get; set; }
    //}
}
