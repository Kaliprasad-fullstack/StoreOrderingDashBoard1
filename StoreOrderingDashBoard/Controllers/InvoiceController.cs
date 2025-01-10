using BAL;
using DAL;
using DbLayer.Service;
using StoreOrderingDashBoard.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace StoreOrderingDashBoard.Controllers
{

    public class InvoiceController : Controller
    {
        private readonly IAuditService _auditService;
        private readonly IWareHouseService _wareHouseService;
        private readonly IOrderService _orderService;
        private readonly IStoreService _storeService;
        private readonly IItemService _itemService;
        private readonly IUserService _userService;

        public InvoiceController(IAuditService AuditService, IWareHouseService wareHouseService, IOrderService orderservice, IStoreService storeService, IItemService itemService, IUserService userService)
        {
            _auditService = AuditService;
            _wareHouseService = wareHouseService;
            _orderService = orderservice;
            _storeService = storeService;
            _itemService = itemService;
            _userService = userService;
        }

        [AdminAuthorization]
        public ActionResult AddDisptchedInformation()
        {
            //var warehouses = _wareHouseService.wareHouseDCs();
            int userid = SessionValues.UserId;
            int custid = Convert.ToInt32(SessionValues.LoggedInCustId);
            List<string> warehouses = _orderService.GetDistinctLocationsFromInvoice(userid, custid);
            //SelectList warehouse = new SelectList(warehouses.Select(x => new { Value = x, Text = x }), "Value", "Text");
            //ViewBag.WareHouse = warehouses;
            return View(warehouses);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [AdminApiAuthorization]
        public ActionResult GetInvoicesforDateDC(DateTime InvoiceDate, string SelectedDCs)
        {
            try
            {
                List<InvoiceHeader> invoiceHeaders = _orderService.GetInvoicesforDateDC(InvoiceDate, SelectedDCs);
                //return Json(data: invoiceHeaders, behavior: JsonRequestBehavior.AllowGet);
                return Json(new { data = invoiceHeaders }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                HelperCls.DebugLog(ex.Message + System.Environment.NewLine + ex.StackTrace);
                throw;
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SaveDisptchInformation(List<InvoiceDispatch> DisptchInformation)
        {
            try
            {
                int userid = SessionValues.UserId;
                if (DisptchInformation != null && DisptchInformation.Count() > 0)
                {
                    //string empiDS = "";
                    foreach (InvoiceDispatch invoice in DisptchInformation)
                    {
                        if (string.IsNullOrEmpty(invoice.InvoiceNo) || string.IsNullOrEmpty(invoice.VehicleNo))
                        {
                            return Json("Unsuccessful");
                        }
                    }
                    foreach (InvoiceDispatch invoice in DisptchInformation)
                    {
                        InvoiceDispatch invoiceDispatch = new InvoiceDispatch
                        {
                            CreatedBy = userid,
                            InvoiceNo = invoice.InvoiceNo,
                            VehicleNo = invoice.VehicleNo,
                            CreatedDate = DateTime.Now
                        };
                        //invoiceDispatch.DispatchDate=invoice.DispatchDate;                        
                        long SheetId = _orderService.AddInvoiceDispatch(invoiceDispatch);
                        if (SheetId == 0)
                            return Json("Unsuccessful");
                    }

                    return Json("Successful");
                }
                else
                {
                    return Json("Unsuccessful");
                }
            }
            catch (Exception ex)
            {
                HelperCls.DebugLog(ex.Message + System.Environment.NewLine + ex.StackTrace);
                return Json("Unsuccessful");
            }
        }

        [HttpGet]
        [AdminAuthorization]
        public ActionResult PODForm()
        {
            int userid = SessionValues.UserId;
            int custid = Convert.ToInt32(SessionValues.LoggedInCustId);
            List<Store> ActiveStores = _storeService.GetStoresByCustomerId(custid);
            return View(ActiveStores);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [AdminApiAuthorization]
        public ActionResult GetItemsforCustomer()
        {
            try
            {
                int custid = Convert.ToInt32(SessionValues.LoggedInCustId);
                List<Item> customeritems = _itemService.GetItemsForCustomer(custid);
                return Json(new { data = customeritems }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                HelperCls.DebugLog(ex.Message + System.Environment.NewLine + ex.StackTrace);
                throw;
            }
        }

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //[AdminApiAuthorization]
        //public ActionResult SavePODInformation(string InvoiceNo, DateTime poddate, string Stores,List<PODResponse> response)
        //{
        //    bool cond = false;
        //    try
        //    {
        //        if (response.Count() > 0 && InvoiceNo != "" && InvoiceNo != null&&Stores!=""&&Stores!=null)
        //        {

        //            var PODHeader = new PODHeader();                    
        //            int userid = SessionValues.UserId;
        //            PODHeader.Invoice_Number = InvoiceNo;
        //            PODHeader.CreatedBy = userid;
        //            PODHeader.PODDate = poddate;
        //            PODHeader.CompanyId = _userService.GetCompanyAssigned(userid);

        //            long pk_PODHeader = _orderService.savePODResponse(PODHeader);
        //            try
        //            {
        //                var orderHeaderAudit = new PODHeader(pk_PODHeader, PODHeader.Invoice_Number, PODHeader.PODDate, PODHeader.CreatedBy);
        //                var auditJavascript = new JavaScriptSerializer();
        //                var OrderAudit = auditJavascript.Serialize(orderHeaderAudit);
        //                _auditService.InsertAudit(HelperCls.GetIPAddress(), OrderAudit, Request.Url.OriginalString.ToString(), userid, null);
        //            }
        //            catch (Exception ex)
        //            {
        //                HelperCls.DebugLog(ex.Message + System.Environment.NewLine + ex.StackTrace);
        //                throw ex;
        //            }
        //            foreach (PODResponse save in response.ToList())
        //            {
        //                PODDetail PODDetail = new PODDetail();
        //                PODDetail.Item = save.Item;
        //                PODDetail.ItemId = save.ItemId;
        //                PODDetail.InvoiceQuantity = save.InvoiceQty;
        //                PODDetail.DeliveredQuantity = save.DeliveredQty;
        //                PODDetail.PODHeaderId = pk_PODHeader;
        //                long pk_orderDetail = _orderService.savePODDetailResponse(PODDetail);
        //                try
        //                {
        //                    var OrderDetailAudit = new PODDetail(pk_orderDetail, PODDetail.InvoiceQuantity, PODDetail.DeliveredQuantity, PODDetail.PODHeaderId);
        //                    var auditJavascript = new JavaScriptSerializer();
        //                    var OrderAudit = auditJavascript.Serialize(OrderDetailAudit);
        //                    _auditService.InsertAudit(HelperCls.GetIPAddress(), OrderAudit, Request.Url.OriginalString.ToString(), userid, null);
        //                }
        //                catch (Exception ex)
        //                {
        //                    HelperCls.DebugLog(ex.Message + System.Environment.NewLine + ex.StackTrace);
        //                    throw ex;
        //                }
        //            }
        //            cond = (pk_PODHeader > 0 ? true : false);
        //            if (cond)
        //            {
        //                List<PODDetail> orderDetails = _orderService.GetPODDetails(pk_PODHeader);
        //                cond = (orderDetails.Count() > 0 ? true : false);
        //                if (!cond)
        //                    _orderService.DeletePODHeader(pk_PODHeader, userid);
        //            }
        //            //if (cond)
        //            //{
        //            //   HelperCls.SendEmail(store.StoreEmailId, "Order Finalize", HelperCls.PopulateBodyFinalizeorder(store.StoreName, pk_PODHeader.ToString()));
        //            //}
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        HelperCls.DebugLog(ex.Message + System.Environment.NewLine + ex.StackTrace);
        //    }
        //    return Json(cond, JsonRequestBehavior.AllowGet);
        //}

        public ActionResult AddDAForVehicle()
        {
            return View();
        }
        public ActionResult LoadDispatchVehicleData(DateTime DispatchDate)
        {
            List<InvoiceDispatch> invoices = _orderService.GetVehiclesDAforDate(DispatchDate);
            List<InvoiceDispatch> invoiceDispatches = (from p in invoices
                                                       select p).GroupBy(x => new { x.VehicleNo, x.InvoiceNo })
                                    .Select(g => g.FirstOrDefault()).ToList();
            return Json(new { data = invoiceDispatches }, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult SaveDAForVehicle(List<VehicleDispatchExcel> vehicleDisbursementList)
        {
            try
            {
                if (vehicleDisbursementList.Count > 0)
                {
                    int userid = SessionValues.UserId;
                    foreach (VehicleDispatchExcel invoice in vehicleDisbursementList)
                    {
                        InvoiceDispatch invoiceDispatch = new InvoiceDispatch
                        {
                            VehicleNo = invoice.VehicleNo,
                            ModifiedDate = DateTime.Now,
                            ModifiedBy = userid,
                            InvoiceNo = invoice.DocNo,
                            //invoiceDispatch.DispatchDate = Convert.ToDateTime(invoice.TransDate);
                            DA = invoice.DA
                        };
                        long SheetId = _orderService.SaveDAForVehicle(invoiceDispatch);
                        if (SheetId == 0)
                            return Json(false, JsonRequestBehavior.AllowGet);
                    }

                    var auditJavascript = new JavaScriptSerializer();
                    var orderAudit = auditJavascript.Serialize(vehicleDisbursementList);
                    _auditService.InsertAudit(HelperCls.GetIPAddress(), orderAudit, Request.Url.OriginalString.ToString(), userid, null);
                    return Json(true, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(false, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception Ex)
            {
                HelperCls.DebugLog("StackTrace: " + Ex.Message + " \n StackTrace: " + Ex.StackTrace);
                return Json(false, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult ImportData()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ImportExcelData(HttpPostedFileBase file, string sp_name)
        {
            try
            {
                string fileName = file.FileName;
                DataTable ds = ExcelUtility.ExcelUtility.ExcelToDataTable(file, true);
                if (ds != null)
                {
                    if (ds.Columns.Contains("Internal ID"))
                    {
                        ds.Columns.Remove("Internal ID");
                    }
                }
                string sysfilename = DateTime.Now.ToString("ddMMMyyyyHHmm", System.Globalization.CultureInfo.InvariantCulture);
                string data = _orderService.ImportData(ds,sp_name, sysfilename, SessionValues.UserId);
            }
            catch(Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
            }
            
            return View("ImportData");
        }
    }
}