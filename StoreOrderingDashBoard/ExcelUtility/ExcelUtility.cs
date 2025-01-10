using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web;

namespace StoreOrderingDashBoard.ExcelUtility
{
    public class ExcelUtility
    {
        //public static DataTable ExcelDataToDataTable(string filePath, string sheetName, bool hasHeader = true)
        //{
        //    var dt = new DataTable();
        //    var fi = new FileInfo(filePath);
        //    // Check if the file exists
        //    if (!fi.Exists)
        //        throw new Exception("File " + filePath + " Does Not Exists");

        //    ExcelPackage.LicenseContext = OfficeOpenXml.LicenseContext.NonCommercial;
        //    var xlPackage = new ExcelPackage(fi);
        //    // get the first worksheet in the workbook
        //    sheetName = sheetName != "" ? sheetName : xlPackage.Workbook.Worksheets.Select(x => x.Name).First().ToString();
        //    var worksheet = xlPackage.Workbook.Worksheets[sheetName];

        //    dt = worksheet.Cells[1, 1, worksheet.Dimension.End.Row, worksheet.Dimension.End.Column].ToDataTable(c =>
        //    {
        //        c.FirstRowIsColumnNames = true;
        //    });

        //    return dt;

        //}
        //public static List<ExcelInvoiceDetail> ExcelToList(string path, bool hasHeader = true)
        //{

        //    ExcelPackage.LicenseContext = OfficeOpenXml.LicenseContext.NonCommercial;
        //    using (var pck = new ExcelPackage())
        //    {
        //        using (var stream = File.OpenRead(path))
        //        {
        //            pck.Load(stream);
        //        }

        //        var ws = pck.Workbook.Worksheets.First();
        //        DataTable tbl = new DataTable();
        //        ExcelInvoiceDetail orderExcel = new ExcelInvoiceDetail();
        //        List<ExcelInvoiceDetail> invoiceDetails = new List<ExcelInvoiceDetail>();
        //        for (int i = 1; i <= ws.Dimension.End.Column; i++)
        //        {
        //            var firstRowCell = ws.Cells[1, i];
        //            //tbl.Columns.Add(hasHeader ? firstRowCell.Text : $"Column {firstRowCell.Start.Column}");                    
        //            var HeaderName = firstRowCell.Value != null ? firstRowCell.Value.ToString().Trim() : "";


        //            if (HeaderName.Trim().ToUpper() == nameof(orderExcel.Internal_Id).Trim().ToUpper())
        //            {
        //                orderExcel.IndexInternal_Id = i;
        //            }
        //            else if (HeaderName.Trim().ToUpper() == nameof(orderExcel.Invoice_Number).Trim().ToUpper())
        //            {
        //                orderExcel.IndexInvoice_Number = i;
        //            }
        //            else if (HeaderName.Trim().ToUpper() == nameof(orderExcel.Location).Trim().ToUpper())
        //            {
        //                orderExcel.IndexLocation = i;
        //            }
        //            else if (HeaderName.Trim().ToUpper() == nameof(orderExcel.Item).Trim().ToUpper())
        //            {
        //                orderExcel.IndexItem = i;
        //            }
        //            else if (HeaderName.Trim().ToUpper() == nameof(orderExcel.Description).Trim().ToUpper())
        //            {
        //                orderExcel.IndexDescription = i;
        //            }
        //            else if (HeaderName.Trim().ToUpper() == nameof(orderExcel.HSNSAC).Trim().ToUpper())
        //            {
        //                orderExcel.IndexHSNSAC = i;
        //            }
        //            else if (HeaderName.Trim().ToUpper() == nameof(orderExcel.Location).Trim().ToUpper())
        //            {
        //                orderExcel.IndexLocation = i;
        //            }
        //            else if (HeaderName.Trim().ToUpper() == nameof(orderExcel.TaxCode).Trim().ToUpper())
        //            {
        //                orderExcel.IndexTaxCode = i;
        //            }
        //            else if (HeaderName.Trim().ToUpper() == nameof(orderExcel.TaxRate).Trim().ToUpper())
        //            {
        //                orderExcel.IndexTaxRate = i;
        //            }
        //            else if (HeaderName.Trim().ToUpper() == nameof(orderExcel.Amount).Trim().ToUpper())
        //            {
        //                orderExcel.IndexAmount = i;
        //            }
        //            else if (HeaderName.Trim().ToUpper() == nameof(orderExcel.TaxAmount).Trim().ToUpper())
        //            {
        //                orderExcel.IndexTaxAmount = i;
        //            }
        //            else if (HeaderName.Trim().ToUpper() == nameof(orderExcel.GrossAmount).Trim().ToUpper())
        //            {
        //                orderExcel.IndexGrossAmount = i;
        //            }
        //            else if (HeaderName.Trim().ToUpper() == nameof(orderExcel.TaxLiable).Trim().ToUpper())
        //            {
        //                orderExcel.IndexTaxLiable = i;
        //            }
        //            else if (HeaderName.Trim().ToUpper() == nameof(orderExcel.GoodsAndServices).Trim().ToUpper())
        //            {
        //                orderExcel.IndexGoodsAndServices = i;
        //            }
        //            else if (HeaderName.Trim().ToUpper() == nameof(orderExcel.ItemCategory).Trim().ToUpper())
        //            {
        //                orderExcel.IndexItemCategory = i;
        //            }
        //            else if (HeaderName.Trim().ToUpper() == nameof(orderExcel.Quantity).Trim().ToUpper())
        //            {
        //                orderExcel.IndexQuantity = i;
        //            }
        //        }
        //        if (orderExcel.IndexInternal_Id == 0)
        //        {

        //        }
        //        var startRow = hasHeader ? 2 : 1;
        //        for (int rowNum = startRow; rowNum <= ws.Dimension.End.Row; rowNum++)
        //        {
        //            var wsRow = ws.Cells[rowNum, 1, rowNum, ws.Dimension.End.Column];
        //            DataRow row = tbl.Rows.Add();
        //            var import = new ExcelInvoiceDetail();
        //            import.Error = "";
        //            import.Internal_Id = ws.Cells[rowNum, orderExcel.IndexInternal_Id].Value is DBNull ? null : ws.Cells[rowNum, orderExcel.IndexInternal_Id].Text;
        //            import.Invoice_Number = ws.Cells[rowNum, orderExcel.IndexInvoice_Number].Value is DBNull ? null : ws.Cells[rowNum, orderExcel.IndexInvoice_Number].Text;
        //            import.Item = ws.Cells[rowNum, orderExcel.IndexItem].Value is DBNull ? null : ws.Cells[rowNum, orderExcel.IndexItem].Text;
        //            import.Description = ws.Cells[rowNum, orderExcel.IndexDescription].Value is DBNull ? null : ws.Cells[rowNum, orderExcel.IndexDescription].Text;
        //            import.HSNSAC = ws.Cells[rowNum, orderExcel.IndexHSNSAC].Value is DBNull ? null : ws.Cells[rowNum, orderExcel.IndexHSNSAC].Text;
        //            import.Location = ws.Cells[rowNum, orderExcel.IndexLocation].Value is DBNull ? null : ws.Cells[rowNum, orderExcel.IndexLocation].Text;
        //            import.TaxCode = ws.Cells[rowNum, orderExcel.IndexTaxCode].Value is DBNull ? null : ws.Cells[rowNum, orderExcel.IndexTaxCode].Text;
        //            import.TaxRate = ws.Cells[rowNum, orderExcel.IndexTaxRate].Value is DBNull ? null : ws.Cells[rowNum, orderExcel.IndexTaxRate].Text;
        //            import.Amount = ws.Cells[rowNum, orderExcel.IndexAmount].Value is DBNull ? null : ws.Cells[rowNum, orderExcel.IndexAmount].Text;
        //            import.TaxAmount = ws.Cells[rowNum, orderExcel.IndexTaxAmount].Value is DBNull ? null : ws.Cells[rowNum, orderExcel.IndexTaxAmount].Text;
        //            import.GrossAmount = ws.Cells[rowNum, orderExcel.IndexGrossAmount].Value is DBNull ? null : ws.Cells[rowNum, orderExcel.IndexGrossAmount].Text;
        //            import.GoodsAndServices = ws.Cells[rowNum, orderExcel.IndexGoodsAndServices].Value is DBNull ? null : ws.Cells[rowNum, orderExcel.IndexGoodsAndServices].Text;
        //            import.ItemCategory = ws.Cells[rowNum, orderExcel.IndexItemCategory].Value is DBNull ? null : ws.Cells[rowNum, orderExcel.IndexItemCategory].Text;
        //            import.Quantity = ws.Cells[rowNum, orderExcel.IndexQuantity].Value is DBNull ? null : ws.Cells[rowNum, orderExcel.IndexQuantity].Text;

        //            //foreach (var cell in wsRow)
        //            //{
        //            //    // Check for DBNull before converting
        //            //    row[cell.Start.Column - 1] = cell.Value is DBNull ? null : cell.Text;
        //            //}
        //            invoiceDetails.Add(import);
        //        }

        //        return invoiceDetails;
        //    }
        //}
        public static DataTable ExcelToDataTable(HttpPostedFileBase file, bool hasHeader = true)
        {
            byte[] fileBytes = new byte[file.ContentLength];
            var data = file.InputStream.Read(fileBytes, 0, Convert.ToInt32(file.ContentLength));
            //ExcelPackage.LicenseContext = OfficeOpenXml.LicenseContext.NonCommercial;
            using (var pck = new ExcelPackage(file.InputStream))
            {
                //using (var stream = File.OpenRead(path))
                //{
                //    pck.Load(stream);
                //}

                var ws = pck.Workbook.Worksheets.First();
                DataTable tbl = new DataTable();
                for (int i = 1; i <= ws.Dimension.End.Column; i++)
                {
                    var firstRowCell = ws.Cells[1, i];
                    tbl.Columns.Add(hasHeader ? firstRowCell.Text : $"Column {firstRowCell.Start.Column}");
                }
                var startRow = hasHeader ? 2 : 1;
                for (int rowNum = startRow; rowNum <= ws.Dimension.End.Row; rowNum++)
                {
                    //var wsRow = ws.Cells[rowNum, 1, rowNum, ws.Dimension.End.Column];
                    //DataRow row = tbl.Rows.Add();
                    //foreach (var cell in wsRow)
                    //{
                    //    if(cell.Text== "TXRS9TD242500293")
                    //    {

                    //    }
                    //    // Check for DBNull before converting
                    //    row[cell.Start.Column - 1] = cell.Value is DBNull ? null : cell.Value.ToString();
                    //}
                    //var wsRow = ws.Cells[rowNum, 1, rowNum, ws.Dimension.End.Column];
                    DataRow row = tbl.Rows.Add();
                    for (int col = 1; col <= ws.Dimension.End.Column; col++)
                    {
                        // Check for DBNull before converting
                        var cell = ws.Cells[rowNum, col];
                        //if (cell.Text == "TXRS9TD242500293")
                        //{
                        //}
                        var headercell = ws.Cells[1, col];
                        if (headercell.Text.Contains("Date"))
                        {
                            string datevalue="";

                            try
                            {
                                DateTime d = BAL.HelperCls.FromExcelStringDate(cell.Value.ToString());
                                datevalue = d.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);
                            }
                            catch (Exception e)
                            {
                                datevalue= cell.Value.ToString();
                            }
                            row[col - 1] = cell.Value is DBNull ? null : cell.Text;
                        }
                        else
                            row[col - 1] = cell.Value is DBNull ? null : cell.Text;
                    }
                }

                return tbl;
            }
        }


    }
    //public class ExcelInvoiceDetail
    //{
    //    public string? Internal_Id { get; set; }
    //    public int IndexInternal_Id { get; set; }
    //    public string? Invoice_Number { get; set; }
    //    public int IndexInvoice_Number { get; set; }
    //    public string? Item { get; set; }
    //    public int IndexItem { get; set; }
    //    public string? Description { get; set; }
    //    public int IndexDescription { get; set; }
    //    public string? HSNSAC { get; set; }
    //    public int IndexHSNSAC { get; set; }
    //    public string? Location { get; set; }
    //    public int IndexLocation { get; set; }
    //    public string? TaxCode { get; set; }
    //    public int IndexTaxCode { get; set; }
    //    public string? TaxRate { get; set; }
    //    public int IndexTaxRate { get; set; }
    //    public string? Amount { get; set; }
    //    public int IndexAmount { get; set; }
    //    public string? TaxAmount { get; set; }
    //    public int IndexTaxAmount { get; set; }
    //    public string? GrossAmount { get; set; }
    //    public int IndexGrossAmount { get; set; }
    //    public string? TaxLiable { get; set; }
    //    public int IndexTaxLiable { get; set; }
    //    public string? GoodsAndServices { get; set; }
    //    public int IndexGoodsAndServices { get; set; }
    //    public string? ItemCategory { get; set; }
    //    public int IndexItemCategory { get; set; }
    //    public string? Quantity { get; set; }
    //    public int IndexQuantity { get; set; }
    //    public string Error { get; set; }
    //}
}