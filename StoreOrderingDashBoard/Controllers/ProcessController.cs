using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using DbLayer.Service;
using StoreOrderingDashBoard.Models;
using DAL;
using System.IO;
using System.Data;
using System.Web.Script.Serialization;
using BAL;
using System.Text;

namespace StoreOrderingDashBoard.Controllers
{
    public class ProcessController : Controller
    {
        private readonly IOrderService _orderService;
        private readonly IReportService _reportService;
        private readonly IUserService _userService;
        private readonly IWareHouseService _wareHouseService;
        private readonly IAuditService _auditService;
        public ProcessController(IOrderService orderService, IReportService reportService, IUserService userService, IWareHouseService wareHouseService, IAuditService auditService)
        {
            _orderService = orderService;
            _reportService = reportService;
            _userService = userService;
            _wareHouseService = wareHouseService;
            _auditService = auditService;
        }

        // GET: Process
        [AdminAuthorization]
        public ActionResult FinilizeList()
        {
            int custid = Convert.ToInt32(SessionValues.LoggedInCustId);
            int userid =SessionValues.UserId;
            //commented by swapnil at 23Aug2018
            //var finilizeOrder = _reportService.GetFinilizeOrder(custid);
            var finilizeOrder = _reportService.ProcessedData(false, true,false,false,false, custid, userid);
            // var finilizeOrder = _orderService.GetFinilisedForProcess();
            List<ProcessedCls> processeds = new List<ProcessedCls>();
            foreach (DetailReport header in finilizeOrder)
            {
                ProcessedCls processedCls = new ProcessedCls();
                processedCls.Id = header.Id;
                processedCls.OrderDate = header.OrderDate;
                processedCls.StoreName = header.StoreName;
                processedCls.StoreCode = header.StoreCode;
                processedCls.DCID = header.DCID;
                processedCls.FinilizedOrderno = header.OrderNo;
                processedCls.IsThreshold = (_orderService.GetOrderHeader(header.Id).orderDetails.Where(x => x.Item.MaximumOrderLimit < x.Quantity).Count() > 0 ? true : false);
                processedCls.DcName = header.DcName;
                processedCls.LocationName = header.LocationName;
                processedCls.Quantity = header.Quantity;
                //  processedCls.IsThreshold = (header.orderDetails.Where(x => x.Item.MaximumOrderLimit < x.Quantity).Count()>0?true:false);
                processeds.Add(processedCls);
            }
            ViewBag.FinilizeOrder = processeds;
            ViewBag.WareHouse = _wareHouseService.wareHouseDCs();
            return View();
        }

        [AdminAuthorization]
        public ActionResult FinaliseItemlist(long orderheaderid,int dcid)
        {
            var itemlist = _orderService.GetOrderDetails(orderheaderid);

            ViewBag.OrderList = itemlist;
            ViewData["DCID"] = dcid;
            
            ViewBag.WareHouse = _wareHouseService.wareHouseDCs();
            return PartialView("FinaliseItemlist");
        }

        [HttpPost]
        [AdminApiAuthorization]
        public JsonResult ProcessedOrder(List<OrderSave> order)
        {
            int affectedrow = 0;
            foreach (OrderSave save in order)
            {
                var orderheader = _orderService.GetOrderHeader(save.OrderheaderId);
                orderheader.IsProcessed = true;
                orderheader.ProcessedDate = DateTime.Now;
                orderheader.DCID = save.DCID;
                orderheader.ProcessedUserId =SessionValues.UserId;
                affectedrow = _orderService.UpdateOrderHeader(orderheader);
                try
                {
                    var orderHeaderAudit = new OrderHeader(orderheader.Id, orderheader.IsProcessed, orderheader.IsOrderStatus, orderheader.DraftOrderno, orderheader.Finilizeorderno, orderheader.OrderDate, orderheader.ProcessedDate, orderheader.isOrderEmailSent, orderheader.StoreId, orderheader.ProcessedUserId, orderheader.DCID, orderheader.Isdeleted, orderheader.orderDetails,null,null,null);
                    var auditJavascript = new JavaScriptSerializer();
                    var OrderAudit = auditJavascript.Serialize(orderHeaderAudit);
                    _auditService.InsertAudit(HelperCls.GetIPAddress(), OrderAudit, Request.Url.OriginalString.ToString(),SessionValues.UserId, null);
                }
                catch (Exception ex)
                {
                    HelperCls.DebugLog(ex.Message + System.Environment.NewLine + ex.StackTrace);
                    throw ex;
                }
            }
            bool cond = affectedrow > 0 ? true : false;
            return Json(cond, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [AdminApiAuthorization]
        public JsonResult ProcessOrderOneByOne(List<OrderSave> order)
        {
            long headerid = order.FirstOrDefault().OrderheaderId;
            int DCID = order.FirstOrDefault().DCID;
            var orderheader = _orderService.GetOrderHeader(headerid);
            orderheader.IsProcessed = true;
            orderheader.ProcessedDate = DateTime.Now;
            orderheader.DCID = DCID;
            orderheader.ProcessedUserId =SessionValues.UserId;
            int affectedrow = _orderService.UpdateOrderHeader(orderheader);
            try
            {
                var orderHeaderAudit = new OrderHeader(orderheader.Id, orderheader.IsProcessed, orderheader.IsOrderStatus, orderheader.DraftOrderno, orderheader.Finilizeorderno, orderheader.OrderDate, orderheader.ProcessedDate, orderheader.isOrderEmailSent, orderheader.StoreId, orderheader.ProcessedUserId, orderheader.DCID, orderheader.Isdeleted, orderheader.orderDetails,null,null,null);
                var auditJavascript = new JavaScriptSerializer();
                var OrderAudit = auditJavascript.Serialize(orderHeaderAudit);
                _auditService.InsertAudit(HelperCls.GetIPAddress(), OrderAudit, Request.Url.OriginalString.ToString(),SessionValues.UserId, null);
            }
            catch (Exception ex)
            {
                HelperCls.DebugLog(ex.Message + System.Environment.NewLine + ex.StackTrace);
                throw ex;
            }
            foreach (OrderSave save in order)
            {
                var detail = _orderService.GetOrderDetail(headerid, save.ProductId);
                if(detail!=null)
                {
                    detail.Quantity = Convert.ToInt32(save.Qty);
                    affectedrow = _orderService.UpdateOrderDetails(detail);
                    try
                    {
                        var OrderDetailAudit = new OrderDetail(detail.Id, detail.Quantity, detail.ItemId, detail.OrderHeaderId, null);
                        var auditJavascript = new JavaScriptSerializer();
                        var orderDetailAudit = auditJavascript.Serialize(OrderDetailAudit);
                        _auditService.InsertAudit(HelperCls.GetIPAddress(), orderDetailAudit, Request.Url.OriginalString.ToString(), null,SessionValues.UserId);
                    }
                    catch (Exception ex)
                    {
                        HelperCls.DebugLog(ex.Message + System.Environment.NewLine + ex.StackTrace);
                        throw ex;
                    }
                }
                
            }
            bool cond = affectedrow > 0 ? true : false;
            return Json(true, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [AdminApiAuthorization]
        public JsonResult DeleteItem(long orderheaderid, int itemid,string retVal)
        {
            var orderdetail = _orderService.GetOrderDetail(orderheaderid, itemid);
            int affectedrow = _orderService.deleteOrderDetail(orderdetail);
            try
            {
                var OrderDetailAudit = new OrderDetail(orderdetail.Id, orderdetail.Quantity, orderdetail.ItemId, orderdetail.OrderHeaderId,retVal);
                var auditJavascript = new JavaScriptSerializer();
                var orderDetailAudit = auditJavascript.Serialize(OrderDetailAudit);
                _auditService.InsertAudit(HelperCls.GetIPAddress(), orderDetailAudit, Request.Url.OriginalString.ToString(), SessionValues.UserId!= null ?SessionValues.UserId : (int?)null, SessionValues.StoreId!= null ? SessionValues.StoreId : (int?)null);
            }
            catch (Exception ex)
            {
                HelperCls.DebugLog(ex.Message + System.Environment.NewLine + ex.StackTrace);
                throw ex;
            }
            bool cond = affectedrow > 0 ? true : false;
            return Json(cond, JsonRequestBehavior.AllowGet);
        }

        [AdminAuthorization]
        public ActionResult Export()
        {
            var startDate = DateTime.Now.Date.AddDays(-1);
            var endDate = DateTime.Now.Date;

            string sd = startDate.Date.ToString("yyyy-MM-dd");
            string ed = endDate.Date.ToString("yyyy-MM-dd");
            ViewData["StartDate"] = sd;
            ViewData["EndDate"] = ed;
            return View();
        }

        [AdminAuthorization]
        public ActionResult ExportToExcel(string FromDate, string ToDate)
        {


            //var sb = new StringBuilder();
            StringWriter sw = new StringWriter();
            sw.WriteLine("Order #,Expected Delivery Date,Department,Class,Status,Location,Division,SO Approver Partner,ITEM CODE,ITEM NAME,ORDER QUANTITY,eBizDownloadStatus,Store Code,Indent No,Amount,Indent Date,Location,TRANSACTION TYPE,PLACE OF SUPPLY");
            // You can write sql query according your need  

            DateTime FromDate1 = Convert.ToDateTime(FromDate);
            DateTime ToDate1 = Convert.ToDateTime(ToDate);
            int custid = Convert.ToInt32(SessionValues.LoggedInCustId);/*,{3},{4},{5},{6},{7},{8},{9},{10},{11},{12},{13},{14},{15},{16},{17},{18}*/
            int userid =SessionValues.UserId;
            var customers = _orderService.netSuitUploadsExcel(FromDate1, ToDate1, custid, userid);
            foreach (var item in customers)
            {
                sw.WriteLine(string.Format("\"{0}\",\"{1}\",\"{2}\",\"{3}\",\"{4}\",\"{5}\",\"{6}\",\"{7}\",\"{8}\",\"{9}\",\"{10}\",\"{11}\",\"{12}\",\"{13}\",\"{14}\",\"{15}\",\"{16}\",\"{17}\",\"{18}\"",
                     item.Order,
                     DateTime.Now.ToString("dd/MM/yyyy"),
                     item.Department, 
                     item.Class,
                     "Pending Approval",
                     item.Location,
                     item.Division,
                     item.SOApproverPartner,
                     item.code,
                     item.desc,
                     item.qty,
                     "Edit",
                     item.Store_Name,
                     item.PO,
                     1,
                     item.PO_Date,
                     item.Location,
                     item.Transactiontype,
                     item.Placeofsupply
                ));
            }

            var filename = "SalesOrderUpload" + DateTime.Now.ToString("dd/MM/yyyy") + ".csv";
            var response = System.Web.HttpContext.Current.Response;
            response.BufferOutput = true;
            
            response.Clear();
            response.ClearHeaders();
            response.ContentEncoding = Encoding.Unicode;
            response.AddHeader("content-disposition", "attachment;filename=" + filename);
            response.AddHeader("content-disposition", "attachment;filename=" + filename);
            response.ContentType = "text/csv";
            response.Write(sw.ToString());
            response.End();
            return RedirectToAction("Export", "Process");
            
        }

        [AdminApiAuthorization]
        [HttpPost]
        public JsonResult DeleteOrder(long Id, string reason)
        {
            bool cond = (_orderService.DeleteFinalisedOrder(Id, reason,SessionValues.UserId) > 0 ? true : false);
            var Updateorder = "{orderheaderid:" + Id + ",reason:" + reason + "}";
            _auditService.InsertAudit(HelperCls.GetIPAddress(), Updateorder, Request.Url.OriginalString.ToString(), SessionValues.UserId!= null ?SessionValues.UserId : (int?)null, null);
            return Json(cond, JsonRequestBehavior.AllowGet);
        }

        [AdminApiAuthorization]
        [HttpPost]
        public JsonResult UpdateItemQuantity(long orderheaderid, int itemid, int quantity)
        {
            bool cond = (_orderService.UpdateOrder(orderheaderid, itemid, quantity,SessionValues.UserId) > 0 ? true : false);
            var Updateorder = "{orderheaderid:" + orderheaderid + ",itemid:" + itemid + ",quantity:" + quantity + "}";
            _auditService.InsertAudit(HelperCls.GetIPAddress(), Updateorder, Request.Url.OriginalString.ToString(), SessionValues.UserId!= null ?SessionValues.UserId : (int?)null, null);
            return Json(cond, JsonRequestBehavior.AllowGet);
        }
    }
}