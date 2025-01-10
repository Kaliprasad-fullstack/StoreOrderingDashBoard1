using ClosedXML.Excel;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace TicketPendingMailTaskService
{
    class Program
    {
        static void Main(string[] args)
        {
            DataTable dt = GeneratePendingTickets();
            SendEmail("Ticket", dt);
        }

        private static void SendEmail(string reporttype, DataTable dt)
        {
            XLWorkbook wb = new XLWorkbook();

            var ws = wb.Worksheets.Add(dt, "PendingTickets");

            System.IO.MemoryStream memoryStream = new System.IO.MemoryStream(); wb.SaveAs(memoryStream);

            memoryStream.Seek(0, System.IO.SeekOrigin.Begin);

            System.Net.Mail.Attachment attachment = new System.Net.Mail.Attachment(memoryStream, "PendingTickets" + ".xlsx", "application/vnd.ms-excel");
            DataTable tblReport = GetDataTable("usp_getemailparameters", new List<SqlParameter> { new SqlParameter("@ReportCode", "Ticket") });
            string To = "", CC = "", Bcc = "", Subject = "", Body = "";

            foreach (DataRow dr in tblReport.Rows)
            {
                if (dr["ReportCode"].ToString() == "Ticket")
                {
                    To = dr["To"].ToString();
                    CC = dr["CC"].ToString();
                    Bcc = dr["BCC"].ToString();
                    Subject = dr["Subject"].ToString();
                    Body = dr["Body"].ToString();
                }
            }
            //DataRow dr=tblReport.Rows[]

            SendEmailAttachement(To, CC, Bcc, Subject, Body, attachment);
        }

        private static bool SendEmailAttachement(string to, string cC, string bcc, string subject, string body, Attachment attachment)
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
                string[] toMailIDs = to.Split(new char[] { ',' });
                foreach (string toMailID in toMailIDs)
                {
                    if (!mail.To.Contains(new MailAddress(toMailID)))
                        mail.To.Add(new MailAddress(toMailID));
                }
                if (cC != null && cC!="")
                {
                    string[] ccMailIDs = cC.Split(new char[] { ',' });

                    foreach (string ccMailID in ccMailIDs)
                    {
                        if (!mail.CC.Contains(new MailAddress(ccMailID)))
                            mail.CC.Add(new MailAddress(ccMailID));
                    }
                }

                if (bcc != null && bcc != "")
                {
                    string[] bccMailIDs = bcc.Split(new char[] { ',' });

                    foreach (string MailID in bccMailIDs)
                    {
                        if (!mail.CC.Contains(new MailAddress(MailID)))
                            mail.CC.Add(new MailAddress(MailID));
                    }
                }
                client.EnableSsl = true;
                mail.Subject = subject;
                mail.Body = body;
                mail.IsBodyHtml = true;

                //mail.Attachments.Add(attachment);
                client.Send(mail);
                return true;
            }
            catch (Exception ex)
            {
                return false;
                //throw ex;
            }
        }

        private static DataTable GetDataTable(string v, List<SqlParameter> p = null)
        {
            DataTable dt = new DataTable();
            string consString = System.Configuration.ConfigurationManager.ConnectionStrings["StoreConnectionString"].ConnectionString;
            using (SqlConnection con = new SqlConnection(consString))
            {
                using (SqlCommand cmd = new SqlCommand(v))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    if (p != null)
                    {
                        foreach (var kvp in p)
                        {
                            cmd.Parameters.Add(kvp);
                        }
                    }
                    cmd.Connection = con;
                    con.Open();
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.Fill(dt);
                    con.Close();
                }
            }
            return dt;
        }
        private static DataTable GeneratePendingTickets()
        {
            return GetDataTable("USP_TicketDetailsEmailReport");
        }
    }
}
