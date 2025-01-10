//using Dapper;
using DbLayer.Service;
using DbLayer.Repository;
using DAL;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Dapper;
using System.Reflection;

namespace OrderConfirmationMailService
{
    public class DBLayer
    {
        public SqlConnection con;
        //To Handle connection related activities      
        private void connection()
        {
            string constr = ConfigurationManager.ConnectionStrings["StoreConnectionString"].ToString();
            con = new SqlConnection(constr);

        }
        public static OrderConfirmationRepository _orderConfirmationRepository;
        public DBLayer()
        {
            _orderConfirmationRepository = new OrderConfirmationRepository();
        }
        public void GetOrderGenerated(DateTime time, int FreQuency)
        {
            try
            {
                DebugLog("TriggeredTime: " + time.ToString() + "Frequency: " + FreQuency);
                var customerEmailTypes = GetCustomersMailcontentType(time, FreQuency);
                // Order Generated
                foreach (CustomerEmailMaster EmailType in customerEmailTypes.Where(x => x.EmailTypeId == 1).ToList())
                {
                    string orderInfomation = "Your Order has been placed.";
                    List<OrderMailContent> OrderMailContent = GetCustomerMailContentfromcontentType(time, FreQuency, EmailType.CustId, EmailType.EmailTypeId);
                    var GroupedOrders = OrderMailContent.GroupBy(x => new { x.CustId, x.GroupId }).ToList();
                    foreach (var order in GroupedOrders)
                    {
                        //var StoreIDs = esc.Select(x => x.StoreId).ToList();                        
                        var List = order.ToList();
                        var groupid = List.FirstOrDefault().GroupId;
                        var EmailList = order.Select(x => x.StoreEmailId).Distinct().ToList();
                        //var EmailList = "vedang.thakur@rkfoodland.com,nikhil.kadam@rkfoodland.com";
                        string mailSubject = "Store Order Placed";
                        string Store_Code = List.FirstOrDefault().Store_Code;
                        string filename = "OrderConfirmationEmail.html";
                        var OrderID = List.FirstOrDefault().OrderId;
                        string OrderDate = List.FirstOrDefault().Store_Order_Date.ToString("dd-MMM-yyyy");
                        string mailcontent = GenerateOrderTemplate(order.ToList(), OrderID, Store_Code, filename, OrderDate);
                        string mailList = String.Join(",", EmailList);
                        var DocType = "ORDER";
                        bool IsMailSent = false;
                        if (mailcontent != null && mailcontent != "")
                        {
                            IsMailSent = BAL.HelperCls.SendEmail(mailList, mailSubject, mailcontent);
                        }
                        var CustId = List.FirstOrDefault().CustId;

                        var docId = List.FirstOrDefault().DocumentId;
                        AddMailLog(CustId, groupid, IsMailSent, mailList, EmailType.EmailTypeId, DocType, docId);
                    }
                }
                // Order Invoiced
                foreach (CustomerEmailMaster EmailType in customerEmailTypes.Where(x => x.EmailTypeId == 3).ToList())
                {
                    string orderInfomation = "Your Order is Invoiced.";
                    List<OrderMailContent> OrderMailContent = GetCustomerMailContentfromcontentType(time, FreQuency, EmailType.CustId, EmailType.EmailTypeId);
                    var GroupedOrders = OrderMailContent.GroupBy(x => new { x.CustId, x.GroupId, x.Store_Order_Date, x.DocumentId }).ToList();
                    foreach (var order in GroupedOrders)
                    {
                        //var StoreIDs = esc.Select(x => x.StoreId).ToList();
                        var List = order.ToList();
                        //var EmailList = order.Select(x => x.StoreEmailId).Distinct().ToList();
                        var EmailList = "vedang.thakur@rkfoodland.com,nikhil.kadam@rkfoodland.com";
                        string mailSubject = "Store Order Invoiced";
                        string filename = "InvoiceConfirmationEmail.html";
                        string Store_Code = List.FirstOrDefault().Store_Code;
                        string mailcontent = "";// GenerateOrderTemplate(order.ToList(), orderInfomation,Store_Code, filename);
                        string mailList = String.Join(",", EmailList);
                        var DocType = "INVOICE";
                        bool IsMailSent = false;
                        if (mailcontent != null || mailcontent != "")
                        {
                            //IsMailSent = BAL.HelperCls.SendEmail(mailList, mailSubject, mailcontent);
                        }
                        var CustId = List.FirstOrDefault().CustId;
                        var groupid = List.FirstOrDefault().GroupId;
                        var docId = List.FirstOrDefault().DocumentId;
                        AddMailLog(CustId, groupid, IsMailSent, mailList, EmailType.EmailTypeId, DocType, docId);
                    }
                }
                //Order Deleted
                foreach (CustomerEmailMaster EmailType in customerEmailTypes.Where(x => x.EmailTypeId == 6).ToList())
                {
                    string orderInfomation = "Your Order Items are Deleted.";
                    List<OrderMailContent> OrderMailContent = GetCustomerMailContentfromcontentType(time, FreQuency, EmailType.CustId, EmailType.EmailTypeId);
                    var GroupedOrders = OrderMailContent.GroupBy(x => new { x.StoreId }).ToList();
                    foreach (var order in GroupedOrders)
                    {
                        var List = order.ToList();
                        //var groupid = List.FirstOrDefault().GroupId;
                        var EmailList = order.Select(x => x.StoreEmailId).Distinct().ToList();
                        //var EmailList = "vedang.thakur@rkfoodland.com,nikhil.kadam@rkfoodland.com";
                        string mailSubject = "Store Order Items are Deleted";
                        string Store_Code = List.FirstOrDefault().Store_Code;
                        string filename = "OrderItemsDeleted.html";
                        var OrderID = "";//List.FirstOrDefault().OrderId;
                        string OrderDate = "";// List.FirstOrDefault().Store_Order_Date.ToString("dd-MMM-yyyy");
                        //string Remark = List.FirstOrDefault().Remark;
                        string mailcontent = GenerateDeleteOrderTemplate(order.ToList(), OrderID, Store_Code, filename, OrderDate);
                        string mailList = String.Join(",", EmailList);
                        var DocType = "DEL-ORDER";
                        bool IsMailSent = false;
                        if (mailcontent != null || mailcontent != "")
                        {
                            IsMailSent = BAL.HelperCls.SendEmail(mailList, mailSubject, mailcontent);
                        }
                        //var CustId = List.FirstOrDefault().CustId;

                        //var docId = List.FirstOrDefault().DocumentId;
                        foreach (var ordr in List)
                        {
                            AddMailLog(ordr.CustId, ordr.GroupId, IsMailSent, mailList, EmailType.EmailTypeId, DocType, ordr.DocumentId);
                        }
                    }
                }
                //Order Canceled
                foreach (CustomerEmailMaster EmailType in customerEmailTypes.Where(x => x.EmailTypeId == 7).ToList())
                {
                    string orderInfomation = "Your Order Items are Deleted.";
                    List<OrderMailContent> OrderMailContent = GetCustomerMailContentfromcontentType(time, FreQuency, EmailType.CustId, EmailType.EmailTypeId);
                    var GroupedOrders = OrderMailContent.GroupBy(x => new { x.StoreId }).ToList();
                    foreach (var order in GroupedOrders)
                    {
                        var List = order.ToList();
                        //var groupid = List.FirstOrDefault().GroupId;
                        var EmailList = order.Select(x => x.StoreEmailId).Distinct().ToList();
                        //var EmailList = "vedang.thakur@rkfoodland.com,nikhil.kadam@rkfoodland.com";
                        string mailSubject = "Store Order Items are Closed";
                        string Store_Code = List.FirstOrDefault().Store_Code;
                        string filename = "OrderItemsClosed.html";
                        var OrderID = "";//List.FirstOrDefault().OrderId;
                        string OrderDate = "";// List.FirstOrDefault().Store_Order_Date.ToString("dd-MMM-yyyy");
                        //string Remark = List.FirstOrDefault().Remark;
                        string mailcontent = GenerateCloseOrderTemplate(order.ToList(), OrderID, Store_Code, filename, OrderDate);
                        string mailList = String.Join(",", EmailList);
                        var DocType = "CLOS-ORDER";
                        bool IsMailSent = false;
                        if (mailcontent != null || mailcontent != "")
                        {
                            IsMailSent = BAL.HelperCls.SendEmail(mailList, mailSubject, mailcontent);
                        }
                        //var CustId = List.FirstOrDefault().CustId;

                        //var docId = List.FirstOrDefault().DocumentId;
                        foreach (var ordr in List)
                        {
                            AddMailLog(ordr.CustId, ordr.GroupId, IsMailSent, mailList, EmailType.EmailTypeId, DocType, ordr.DocumentId);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                DebugLog(ex.Message + System.Environment.NewLine + ex.StackTrace);
            }
        }

        private string GenerateCloseOrderTemplate(List<OrderMailContent> list, string orderID, string store_Code, string FileName, string orderDate)
        {
            string body = string.Empty;
            string Tbody = "";
            try
            {
                if (FileName == null)
                    FileName = "OrderConfirmationEmail.html";
                var path = Path.Combine(System.IO.Path.GetDirectoryName(Assembly.GetEntryAssembly().Location), "Template");// .MapPath("~");
                var filepath = path + "\\" + FileName;
                foreach (OrderMailContent content in list)
                {
                    Tbody += @"<tr>" + //"<td class='Store_Order_Date'><p>" + content.Store_Order_Date.ToString("dd-MMM-yyyy") + "</p></td>" +
                        "<td class='OrderDate'><p>" + content.Store_Order_Date.ToString("dd-MMM-yyyy") + "</p></td>" +
                        "<td class='OrderId'><p>" + content.OrderId + "</p></td>" +
                                    "<td class='Item_Code'><p>" + content.Item_Code + "</p></td>" +
                                    "<td class='Item_Desc'><p>" + content.Item_Desc + "</p></td>" +
                                    "<td class='Ordered_Qty'><p>" + content.Ordered_Qty + "</p></td>" +
                                    "<td class='Remark'><p>" + content.Remark + "</p></td>" +
                        "</tr>";
                }
                using (StreamReader reader = new StreamReader(filepath))
                {
                    body = reader.ReadToEnd();
                }
                body = body.Replace("{TableBody}", Tbody);
                body = body.Replace("{Store_Code}", store_Code);
            }
            catch (Exception ex)
            {
                DebugLog(ex.Message + ex.StackTrace);
            }
            return body;
        }

        private string GenerateDeleteOrderTemplate(List<OrderMailContent> list, string orderID, string store_Code, string FileName, string orderDate)
        {
            string body = string.Empty;
            string Tbody = "";
            try
            {
                if (FileName == null)
                    FileName = "OrderConfirmationEmail.html";
                var path = Path.Combine(System.IO.Path.GetDirectoryName(Assembly.GetEntryAssembly().Location), "Template");// .MapPath("~");
                var filepath = path + "\\" + FileName;
                foreach (OrderMailContent content in list)
                {
                    Tbody += @"<tr>" + //"<td class='Store_Order_Date'><p>" + content.Store_Order_Date.ToString("dd-MMM-yyyy") + "</p></td>" +
                        "<td class='OrderDate'><p>" + content.Store_Order_Date.ToString("dd-MMM-yyyy") + "</p></td>" +
                        "<td class='OrderId'><p>" + content.OrderId + "</p></td>" +
                                    "<td class='Item_Code'><p>" + content.Item_Code + "</p></td>" +
                                    "<td class='Item_Desc'><p>" + content.Item_Desc + "</p></td>" +
                                    "<td class='Ordered_Qty'><p>" + content.Ordered_Qty + "</p></td>" +
                                    "<td class='Remark'><p>" + content.Remark + "</p></td>" +
                        "</tr>";
                }
                using (StreamReader reader = new StreamReader(filepath))
                {
                    body = reader.ReadToEnd();
                }
                body = body.Replace("{TableBody}", Tbody);
                body = body.Replace("{Store_Code}", store_Code);
            }
            catch (Exception ex)
            {
                DebugLog(ex.Message + ex.StackTrace);
            }
            return body;
        }

        private string GenerateOrderTemplate(List<OrderMailContent> list, string orderInfomation, string Store_Code, string FileName, string OrderDate)
        {
            string body = string.Empty;
            string Tbody = "";
            try
            {
                if (FileName == null)
                    FileName = "OrderConfirmationEmail.html";
                var path = Path.Combine(System.IO.Path.GetDirectoryName(Assembly.GetEntryAssembly().Location), "Template");// .MapPath("~");
                var filepath = path + "\\" + FileName;
                foreach (OrderMailContent content in list)
                {
                    Tbody += @"<tr>" + //"<td class='Store_Order_Date'><p>" + content.Store_Order_Date.ToString("dd-MMM-yyyy") + "</p></td>" +
                                    "<td class='Item_Code'><p>" + content.Item_Code + "</p></td>" +
                                    "<td class='Item_Desc'><p>" + content.Item_Desc + "</p></td>" +
                                    "<td class='Ordered_Qty'><p>" + content.Ordered_Qty + "</p></td>" +
                        "</tr>";
                }
                using (StreamReader reader = new StreamReader(filepath))
                {
                    body = reader.ReadToEnd();
                }
                body = body.Replace("{TableBody}", Tbody);
                body = body.Replace("{Order_ID}", orderInfomation);
                body = body.Replace("{Order_Date}", OrderDate);
                body = body.Replace("{Store_Code}", Store_Code);
            }
            catch (Exception ex)
            {
                DebugLog(ex.Message + ex.StackTrace);
            }
            return body;
        }

        private List<OrderMailContent> GetCustomerMailContentfromcontentType(DateTime TriggerTime, int freQuency, int custId, int? emailTypeId)
        {
            List<OrderMailContent> mailContents = SQLGetCustomerMailContentfromcontentType(TriggerTime, freQuency, custId, emailTypeId);
            return mailContents;
        }

        public List<CustomerEmailMaster> GetCustomersMailcontentType(DateTime TriggerTime, int Frequency)
        {
            List<CustomerEmailMaster> EscalationMatrix = SQLGetCustomersMailcontentType(TriggerTime, Frequency);
            return EscalationMatrix;
        }
        public static bool DebugLog(string ErrorMessage, string LogFile = "OrderMailServiceError")
        {
            try
            {
                string fileName = "";
                string lsVar = null;
                string LogFilePath = "C:";
                //var filepath = LogFilePath + "\\ErrorLog\\";
                if (String.IsNullOrEmpty(LogFilePath)) return false;

                LogFilePath = LogFilePath + "\\QuickOrder\\OrderConfirmation\\";
                lsVar = DateTime.Now.ToString("yyyy-MM-dd");
                lsVar = lsVar.Replace("/", "");

                LogFilePath = LogFilePath.Trim();
                if (!Directory.Exists(LogFilePath))
                {
                    Directory.CreateDirectory(LogFilePath);
                }
                fileName = Path.Combine(LogFilePath, LogFile + "_" + lsVar + ".log");

                StreamWriter sw = null;
                sw = System.IO.File.AppendText(fileName);
                sw.WriteLine(">>>>>>>>>>>> Start >>>>>>>>>>>>" + System.Environment.NewLine);
                sw.WriteLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " --> " + ErrorMessage + System.Environment.NewLine);
                sw.WriteLine(">>>>>>>>>>>> .End. >>>>>>>>>>>>" + System.Environment.NewLine);
                sw.Flush();
                sw.Close();
                sw = null;
            }
            catch (Exception ex)
            {

            }
            return true;
        }

        public static List<CustomerEmailMaster> SQLGetCustomersMailcontentType(DateTime TriggerTime, int Frequency)
        {
            try
            {
                SqlConnection con1 = new SqlConnection(ConfigurationManager.ConnectionStrings["StoreConnectionString"].ToString());
                con1.Open();
                var CustomerEmailMaster = con1.Query<CustomerEmailMaster>(string.Format("exec GetCustomersMailcontentType @TriggeredDatetime='{0}',@Frequency='{1}'", TriggerTime.ToString("yyyy-MM-dd HH:mm:ss"), Frequency), commandTimeout: 240).ToList();
                con1.Close();
                return CustomerEmailMaster;
            }
            catch (Exception ex)
            {
                DebugLog(ex.Message + System.Environment.NewLine + ex.StackTrace);
                return new List<CustomerEmailMaster>();
            }
        }
        public List<OrderMailContent> SQLGetCustomerMailContentfromcontentType(DateTime TriggerTime, int frequency, int custId, int? emailTypeId)
        {
            try
            {
                SqlConnection con1 = new SqlConnection(ConfigurationManager.ConnectionStrings["StoreConnectionString"].ToString());
                con1.Open();
                var OrderMailContent = con1.Query<OrderMailContent>(string.Format("exec USP_GetMailDataforDatetimeEmailType @TriggeredDatetime='{0}', @EmailType='{1}',@CustId='{2}',@Frequency='{3}'", TriggerTime.ToString("yyyy-MM-dd HH:mm:ss"), emailTypeId, custId, frequency),
                    commandTimeout: 240).ToList();
                con1.Close();
                return OrderMailContent;
            }
            catch (Exception ex)
            {
                DebugLog(ex.Message + System.Environment.NewLine + ex.StackTrace);
                return new List<OrderMailContent>();
            }
        }
        public long AddMailLog(int custId, long groupid, bool isMailSent, string mailList, int? emailTypeId, string DOC_TYPE, long? DocumentId)
        {
            try
            {
                SqlConnection con1 = new SqlConnection(ConfigurationManager.ConnectionStrings["StoreConnectionString"].ToString());
                con1.Open();
                var OrderMailContent = con1.Query<long>(string.Format("exec USP_AddOrderMailLog @CustId={0}, @GroupId={1},@IsMailSent={2},@MailList='{3}',@EmailTypeId={4},@Doc_Type='{5}',@Document_Id={6}", custId, groupid, isMailSent, mailList, emailTypeId, DOC_TYPE, DocumentId),
                    commandTimeout: 240).FirstOrDefault();
                con1.Close();
                return OrderMailContent;
            }
            catch (Exception ex)
            {
                DebugLog(ex.Message + System.Environment.NewLine + ex.StackTrace);
                return 0;
            }
        }


    }
}
