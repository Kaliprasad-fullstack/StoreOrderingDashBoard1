using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.OleDb;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Security;

namespace BAL
{
    public class HelperCls
    {
        public static string EncodeServerName(string serverName)
        {
            return Convert.ToBase64String(Encoding.UTF8.GetBytes(serverName));
        }

        public static string DecodeServerName(string encodedServername)
        {
            return Encoding.UTF8.GetString(Convert.FromBase64String(encodedServername));
        }

        public static bool SendEmail(string Email, string subj, string Message, string CCList = null)
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
                if (CCList != null)
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
                HelperCls.DebugLog(ex.Message + System.Environment.NewLine + ex.StackTrace);
                return false;
                //throw ex;
            }
        }

        public static string PopulateBodyFinalizeorder(string storename, string orderid)
        {
            string body = string.Empty;
            var path = HttpContext.Current.Server.MapPath("~");
            var filepath = path + "\\Template\\FinaliseOrder.html";
            var link = ConfigurationManager.AppSettings["Link"].ToString() + orderid;
            using (StreamReader reader = new StreamReader(filepath))
            {
                body = reader.ReadToEnd();
            }
            body = body.Replace("{StoreName}", storename);
            body = body.Replace("{OrderId}", orderid);
            body = body.Replace("{order_Link}", link);
            return body;
        }

        public static bool DebugLog(string ErrorMessage, string LogFile = "StoreOrderingError")
    {
            try
            {
                string fileName = "";
                string lsVar = null;
                string LogFilePath = HttpContext.Current.Server.MapPath("~");
                //var filepath = LogFilePath + "\\ErrorLog\\";
                if (String.IsNullOrEmpty(LogFilePath)) return false;

                LogFilePath = LogFilePath + "\\ErrorLog\\";
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
                //HelperCls.DebugLog(ex.Message + System.Environment.NewLine + ex.StackTrace);
                throw ex;
            }

            return true;
        }

        public static string PopulateForgetPassword(string storename, string storeid)
        {
            string body = string.Empty;
            var path = HttpContext.Current.Server.MapPath("~");
            var filepath = path + "\\Template\\LostPassword.html";
            var link = ConfigurationManager.AppSettings["forgotlink"].ToString() + storeid + "&Exp=" + URLExpiryData();
            using (StreamReader reader = new StreamReader(filepath))
            {
                body = reader.ReadToEnd();
            }
            body = body.Replace("{StoreName}", storename);
            //body = body.Replace("{OrderId}", orderid);
            body = body.Replace("{order_Link}", link);
            return body;
        }

        public static string GetIPAddress()
        {
            string IPAddress = null;
            IPAddress = HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
            if (IPAddress == "" || IPAddress == null)
                IPAddress = HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];
            return IPAddress;
        }

        public static DataTable ConvertCSVtoDataTable(string strFilePath)
        {
            DataTable dt = new DataTable();
            using (StreamReader sr = new StreamReader(strFilePath))
            {
                string[] headers = sr.ReadLine().Split(',');
                foreach (string header in headers)
                {
                    dt.Columns.Add(header);
                }

                while (!sr.EndOfStream)
                {
                    string[] rows = sr.ReadLine().Split(',');
                    if (rows.Length > 1)
                    {
                        DataRow dr = dt.NewRow();
                        for (int i = 0; i < headers.Length; i++)
                        {
                            dr[i] = rows[i].Trim();
                        }
                        dt.Rows.Add(dr);
                    }
                }

            }

            return dt;
        }

        public static DataTable ConvertXSLXtoDataTable(string strFilePath, string connString)
        {
            OleDbConnection oledbConn = new OleDbConnection(connString);
            DataTable dt = new DataTable();
            try
            {

                oledbConn.Open();
                using (OleDbCommand cmd = new OleDbCommand("SELECT * FROM [Sheet1$]", oledbConn))
                {
                    OleDbDataAdapter oleda = new OleDbDataAdapter();
                    oleda.SelectCommand = cmd;
                    DataSet ds = new DataSet();
                    oleda.Fill(ds);

                    dt = ds.Tables[0];
                }
            }
            catch (Exception ex)
            {
            }
            finally
            {

                oledbConn.Close();
            }

            return dt;

        }

        public static string PopulateResetPassword(string UserName)
        {
            string body = string.Empty;
            var path = HttpContext.Current.Server.MapPath("~");
            var filepath = path + "\\Template\\ResetPassword.html";
            //var link = ConfigurationManager.AppSettings["forgotlink"].ToString() + storeid;
            using (StreamReader reader = new StreamReader(filepath))
            {
                body = reader.ReadToEnd();
            }
            body = body.Replace("{UserName}", UserName);
            //body = body.Replace("{OrderId}", orderid);
            //body = body.Replace("{order_Link}", link);
            return body;
        }

        public static string PopulateForgetAnswer(string name, string adminid)
        {
            string body = string.Empty;
            var path = HttpContext.Current.Server.MapPath("~");
            var filepath = path + "\\Template\\ForgotAnswer.html";
            var link = ConfigurationManager.AppSettings["forgotanswer"].ToString() + adminid + "&Exp=" + URLExpiryData();
            using (StreamReader reader = new StreamReader(filepath))
            {
                body = reader.ReadToEnd();
            }
            body = body.Replace("{AdminName}", name);
            //body = body.Replace("{OrderId}", orderid);
            body = body.Replace("{AnswerLink}", link);
            return body;
        }

        public static bool IsValidEmail(string email)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }

        public static string URLExpiryData()
        {
            DateTime expires = DateTime.Now + TimeSpan.FromDays(GlobalValues.ForgotAnswerExpiryDays) + TimeSpan.FromHours(GlobalValues.ForgotAnswerExpiryHours) + TimeSpan.FromMinutes(GlobalValues.ForgotAnswerExpiryMins);
            string hash = EncodeServerName(expires.ToString("s"));
            return hash;
        }

        public static string GenerateOTP()
        {
            Random rnd = new Random();
            int otp = rnd.Next(1000, 9999);
            return otp.ToString();
        }

        public static string PopulateOTP(string storename, string storeid, string otp)
        {
            string body = string.Empty;
            var path = HttpContext.Current.Server.MapPath("~");
            var filepath = path + "\\Template\\LostPassword.html";
            var link = ConfigurationManager.AppSettings["otplink"].ToString() + storeid + "&Exp=" + URLExpiryData();
            using (StreamReader reader = new StreamReader(filepath))
            {
                body = reader.ReadToEnd();
            }
            body = body.Replace("{StoreName}", storename);
            body = body.Replace("{reset_otp}", otp);
            //body = body.Replace("{OrderId}", orderid);
            body = body.Replace("{order_Link}", link);
            return body;
        }
        public static DateTime FromExcelSerialDate(int SerialDate)
        {
            if (SerialDate > 59) SerialDate -= 1; //Excel/Lotus 2/29/1900 bug   
            return new DateTime(1899, 12, 31).AddDays(SerialDate);
        }
        public static DateTime FromExcelStringDate(string date)
        {
            try
            {
                return DateTime.ParseExact(date.Replace("-","/"), "dd/MM/yyyy", CultureInfo.InvariantCulture);
            }
            catch (Exception ex)
            {
                try
                {
                    return DateTime.ParseExact(date.Replace("-","/"), "dd/MM/yyyy HH:mm:ss", CultureInfo.InvariantCulture);
                }
                catch (Exception ex3)
                {

                    try
                    {
                        return FromExcelSerialDate(Convert.ToInt32(date));
                    }
                    catch (Exception ex4)
                    {
                        throw ex4;
                    }                 
                }
            }
        }
        public static DateTime FromExcelStringDateOnly(string date)
        {
            try
            {
                return DateTime.ParseExact(date, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            }
            catch (Exception ex)
            {
                try
                {
                    return DateTime.ParseExact(date, "dd/MM/yyyy HH:mm:ss", CultureInfo.InvariantCulture);
                }
                catch (Exception ex3)
                {                   
                    throw ex3;
                }
            }
        }
        public static TimeSpan? FromExcelStringTimeSpan(string time)
        {
            DateTime dt;
            if (!DateTime.TryParseExact(time, "HH:mm:ss", CultureInfo.InvariantCulture,DateTimeStyles.None, out dt))
            {
                // handle validation error
                return null;
            }
            TimeSpan time2 = dt.TimeOfDay;
            return time2;
        }
    }
    public class GlobalValues
    {
        public const int MegaBytes = 1 * 1024 * 1024;
        public static int PasswordPolicyDays
        {
            get
            {
                return Convert.ToInt32(ConfigurationManager.AppSettings["AdminPasswordPolicyDays"].ToString());
            }
        }

        public static bool PasswordPolicyStatus
        {
            get
            {
                try
                {
                    return Convert.ToBoolean(ConfigurationManager.AppSettings["AdminPasswordPolicyStatus"].ToString());
                }
                catch (Exception ex)
                {
                    HelperCls.DebugLog(ex.Message);
                    return false;
                }

            }
        }

        public static int ForgotAnswerExpiryHours
        {
            get
            {
                try
                {
                    return Convert.ToInt32(ConfigurationManager.AppSettings["ForgotAnswerExpiryHours"].ToString());
                }
                catch (Exception ex)
                {
                    HelperCls.DebugLog(ex.Message);
                    return 1;
                }

            }
        }

        public static int ForgotAnswerExpiryMins
        {
            get
            {
                try
                {
                    return Convert.ToInt32(ConfigurationManager.AppSettings["ForgotAnswerExpiryMins"].ToString());
                }
                catch (Exception ex)
                {
                    HelperCls.DebugLog(ex.Message);
                    return 0;
                }

            }
        }

        public static int ForgotAnswerExpiryDays
        {
            get
            {
                try
                {
                    return Convert.ToInt32(ConfigurationManager.AppSettings["ForgotAnswerExpiryDays"].ToString());
                }
                catch (Exception ex)
                {
                    HelperCls.DebugLog(ex.Message);
                    return 1;
                }

            }
        }

        public static bool SameOldNewPassWord
        {
            get
            {
                try
                {
                    return Convert.ToBoolean(ConfigurationManager.AppSettings["AdminOldNewPassWordSame"].ToString());
                }
                catch (Exception ex)
                {
                    HelperCls.DebugLog(ex.Message);
                    return false;
                }

            }
        }
    }
}
