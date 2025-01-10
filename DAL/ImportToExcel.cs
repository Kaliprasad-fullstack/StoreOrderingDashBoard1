using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    [NotMapped]
    public class ImportToExcel
    {
        public int Id { get; set; }

        public string StoreCode { get; set; }
        public int StoreColumnId { get; set; }

        public string SkuCode { get; set; }
        public int SKUColumnId { get; set; }

        public string Qty { get; set; }
        public int QtyColumnId { get; set; }

        public string Location { get; set; }
        public string Error { get; set; }

        public DateTime? OrderDate { get; set; }
        public int OrderDateColumnId { get; set; }

        public string OrderReferenceNo { get; set; }
        public int OrderReferenceID { get; set; }

        public string OrderPlacedDate { get; set; }
        public string OrderType { get; set; }
        public string City { get; set; }
        public string LastOrderDate { get; set; }
        public string LateDeliveryDate { get; set; }
        public string UniqueReferenceID { get; set; }
        public string PackSize { get; set; }
        public string OrderedQty { get; set; }
        public string CaseSize { get; set; }
        public string Cases { get; set; }
        public string State { get; set; }
        public string FromDC { get; set; }
        public string New_Build { get; set; }
        public string Focus { get; set; }
        public string Item_Type { get; set; }
        public string Item_sub_Type { get; set; }
        public string TakenDays { get; set; }
        public string Slab { get; set; }

        public int OrderPlacedDateID { get; set; }
        public int OrderTypeID { get; set; }
        public int CityID { get; set; }
        public int LastOrderDateID { get; set; }
        public int LateDeliveryDateID { get; set; }
        public int UniqueReferenceIDID { get; set; }
        public int PackSizeID { get; set; }
        public int OrderedQtyID { get; set; }
        public int CaseSizeID { get; set; }
        public int CasesID { get; set; }
        public int StateID { get; set; }
        public int FromDCID { get; set; }
        public int New_BuildID { get; set; }
        public int FocusID { get; set; }
        public int Item_TypeID { get; set; }
        public int Item_sub_TypeID { get; set; }
        public int TakenDaysID { get; set; }
        public int SlabID { get; set; }
    }
    [NotMapped]
    public class VehicleDispatchExcel
    {
        public string SupplyType { get; set; }
        public string SubType { get; set; }
        public string DocType { get; set; }
        public string DocNo { get; set; }
        public DateTime? DocDate { get; set; }
        public string TransactionType { get; set; }
        public string From_OtherPartyName { get; set; }
        public string From_GSTIN { get; set; }
        public string From_Address1 { get; set; }
        public string From_Address2 { get; set; }
        public string From_Place { get; set; }
        public string DispatchPinCode { get; set; }
        public string Bill_From_State { get; set; }
        public string Distpatch_From_State { get; set; }
        public string To_OtherPartyName { get; set; }
        public string To_GSTIN { get; set; }
        public string To_Address1 { get; set; }
        public string To_Address2 { get; set; }
        public string To_Place { get; set; }
        public string ShipTo_PinCode { get; set; }
        public string Bill_To_State { get; set; }
        public string ShipTo_State { get; set; }

        public string Product { get; set; }
        public string Description { get; set; }
        public string HSN { get; set; }
        public string Unit { get; set; }
        public string Qty { get; set; }
        public string AssembleValue { get; set; }
        public string TaxRate { get; set; }
        public string CGSTAmount { get; set; }
        public string SGSTAmount { get; set; }
        public string IGSTAmount { get; set; }
        public string CESSAmount { get; set; }
        public string CESSAmountNonAdvol { get; set; }
        public string Other { get; set; }
        public string TotalInvoiceValue { get; set; }
        public string TransMode { get; set; }
        public string Distance { get; set; }
        public string TransName { get; set; }
        public string TransId { get; set; }
        public string TransDOCNo { get; set; }
        public string TransDate { get; set; }
        public string VehicleNo { get; set; }
        public string VehicleType { get; set; }
        public string Error { get; set; }
        public string DA { get; set; }
    }
    //[NotMapped]
    //public class Parameters
    //{
    //    public string ParameterName { get; set; }
    //    public string HeaderName { get; set; }
    //    public virtual ICollection<Customer> Customers { get; set; }
    //}
    //[NotMapped]
    //public class OrderFormatExcel
    //{
    //    []
    //    public int CustomerId { get; set; }
    //    //public int OrderType { get; set; }
    //    public List<Parameters> ExcelHeaders { get; set; }
    //    public virtual Customer Customer { get; set; }
    //}
    [NotMapped]
    public class OrderExcelFormatsCustomer
    {
        public IList<OrderFormatExcel> RootObject { get; set; }
    }
}
