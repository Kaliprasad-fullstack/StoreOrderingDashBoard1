using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using DAL;
using DbLayer.Repository;
namespace TestApp
{
    class Program
    {
        public static EscalationRepository _escalationRepository;
        static void Main(string[] args)
        {
            //TimeSpan time = DateTime.Now.TimeOfDay;
            TimeSpan time = new TimeSpan(14, 30, 00);
            int FreQuency = 60;
            string MinIntervalStr = string.Empty;
            //DBLayer dBLayer = new DBLayer();Commented;
            // _escalationRepository = new EscalationRepository();
            //   GetCustomersForEscalationMatrix(time, FreQuency);
            OrderConfirmationMailService.DBLayer layer = new OrderConfirmationMailService.DBLayer();

            MinIntervalStr = ConfigurationManager.AppSettings["IntervalMins"];//.Value.ToString();
            int Interval = Convert.ToInt32(MinIntervalStr);
            layer.GetOrderGenerated(DateTime.Now.AddDays(-1), Interval);
        }
        public static void GetCustomersForEscalationMatrix(TimeSpan time, int FreQuency)
        {
            try
            {
                var EscalationMatrixLevels = GetCustomersLevelsForTriggeredTime(time, FreQuency);
                foreach (EscalationLevelMaster escalationTriggeredLevel in EscalationMatrixLevels)
                {
                    var EscalationActions = GetEscalationActionsForCustomerLevel(escalationTriggeredLevel.CustId, escalationTriggeredLevel.LevelID);
                    var GroupedEscalations = EscalationActions.Where(x => x.EmailTo_CC_BCCFlag == 1).GroupBy(x => new { x.Level, x.CustEmp }).ToList();
                    foreach (var esc in GroupedEscalations)
                    {
                        var Stores = esc.Select(x => new { x.Store.StoreName, x.Store.StoreCode }).ToList();
                        var StoreIDs = esc.Select(x => x.StoreId).ToList();
                        var EmailList = esc.Where(x => x.CustEmp.IsDeleted == false && x.EmailTo_CC_BCCFlag == 1).Select(x => x.CustEmp.EmailAddress).ToList().Distinct();
                        var CCList = EscalationActions.Where(x => x.EmailTo_CC_BCCFlag != 1 && x.Level == esc.Key.Level && StoreIDs.Contains(x.StoreId)).Distinct();
                        string CCListString = string.Join(",", CCList.Select(x => x.CustEmp.EmailAddress));
                        //string content = string.Join("<tr><td>" + Stores.Select(x => x.StoreName) ;
                        string content = string.Join("",Stores.Select(x => "<tr><td>" +x.StoreCode+"<td>" + x.StoreName+"</td></tr>").ToList());
                        string storecount = Convert.ToString(Stores.Count());
                        string mailList = string.Join(",", EmailList);
                        string mailContent = PopulateEscalationBody(storecount, content);
                        string Subject = Convert.ToString(ConfigurationManager.AppSettings["EscalationSubject"]);
                        SendEmail(mailList, Subject, mailContent, CCListString);
                    }
                }
            }
            catch (Exception ex)
            {

            }
        }
        public static List<EscalationLevelMaster> GetCustomersLevelsForTriggeredTime(TimeSpan Time, int Frequency)
        {
            List<EscalationLevelMaster> EscalationMatrix = _escalationRepository.GetCustomersLevelsForTriggeredTime(Time, Frequency);
            return EscalationMatrix;
        }
        public static List<EscalationAction> GetEscalationActionsForCustomerLevel(int CustID, int Level)
        {
            return _escalationRepository.GetEscalationActionsForCustomerLevelList(CustID, Level);
        }
        static bool SendEmail(string Email, string subj, string Message, string CCList = null)
        {
            try
            {
                MailMessage mail = new MailMessage();
                SmtpClient client = new SmtpClient();
                client.Host = Convert.ToString(ConfigurationManager.AppSettings["Host"]);
                client.Port = Convert.ToInt32(ConfigurationManager.AppSettings["Port"]);
                client.Timeout = Convert.ToInt32(ConfigurationManager.AppSettings["Timeout"]);
                client.DeliveryMethod = SmtpDeliveryMethod.Network;
                client.UseDefaultCredentials = false;
                string uid = Convert.ToString(ConfigurationManager.AppSettings["SendMailFrom"]);
                string pwd = Convert.ToString(ConfigurationManager.AppSettings["EmailPassword"]);
                client.Credentials = new System.Net.NetworkCredential(uid, pwd);
                mail.From = new MailAddress(Convert.ToString(ConfigurationManager.AppSettings["FromAddress"]));
                string[] toMailIDs = Email.Split(new char[] { ',' });
                foreach (string toMailID in toMailIDs)
                {
                    if (!mail.To.Contains(new MailAddress(toMailID)))
                        mail.To.Add(new MailAddress(toMailID));
                }
                if (CCList != null && CCList != "")
                {
                    string[] ccMailIDs = CCList.Split(new char[] { ',' });

                    foreach (string ccMailID in ccMailIDs)
                    {
                        if (!mail.CC.Contains(new MailAddress(ccMailID)))
                            mail.CC.Add(new MailAddress(ccMailID));
                    }
                }
                client.EnableSsl = true;
                mail.Subject = subj;
                mail.Body = Message;
                mail.IsBodyHtml = true;
                client.Send(mail);
                return true;
            }
            catch (Exception ex)
            {
                return false;
                //throw ex;
            }
        }
        public static string PopulateEscalationBody(string StoreCount, string StoreNames)
        {
            string body = string.Empty;
            var path = System.IO.Directory.GetCurrentDirectory();
            var filepath = path + "\\Template\\EmailTemplate.html";
            //var link = ConfigurationManager.AppSettings["Link"].ToString() + orderid;
            using (StreamReader reader = new StreamReader(filepath))
            {
                body = reader.ReadToEnd();
            }
            body = body.Replace("{{StoreCount}}", StoreCount);
            body = body.Replace("{{StoreNames}}", StoreNames);
            return body;
        }
    }
}
