//using Dapper;
using DbLayer.Repository;
using DAL;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;

namespace EscalationService
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
        public static EscalationRepository _escalationRepository;        
        public DBLayer()
        {
            _escalationRepository = new EscalationRepository();
        }
        public void GetCustomersForEscalationMatrix(TimeSpan time, int FreQuency)
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
                        string CCListString = String.Join(",", CCList.Select(x => x.CustEmp.EmailAddress));
                        string mailcontent = "Stores - Yet to Order: " + string.Join(", ", Stores.Select(x => x.StoreName));
                        string mailList = String.Join(",", EmailList);
                        BAL.HelperCls.SendEmail(mailList, "Temp", mailcontent, CCListString);
                    }
                }
            }
            catch (Exception ex)
            {
                DebugLog(ex.Message + System.Environment.NewLine + ex.StackTrace);
            }
        }
        public List<EscalationLevelMaster> GetCustomersLevelsForTriggeredTime(TimeSpan Time, int Frequency)
        {
            List<EscalationLevelMaster> EscalationMatrix = _escalationRepository.GetCustomersLevelsForTriggeredTime(Time, Frequency);
            return EscalationMatrix;
        }
        public List<EscalationAction> GetEscalationActionsForCustomerLevel(int CustID, int Level)
        {
            return _escalationRepository.GetEscalationActionsForCustomerLevel(CustID, Level);
        }
        public static bool DebugLog(string ErrorMessage, string LogFile = "EscalationMatrixServiceError")
        {
            try
            {
                string fileName = "";
                string lsVar = null;
                string LogFilePath = "C:";
                //var filepath = LogFilePath + "\\ErrorLog\\";
                if (String.IsNullOrEmpty(LogFilePath)) return false;

                LogFilePath = LogFilePath + "\\NetSuitApiLogSO\\EscalationMatrixService\\";
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
        #region --
        //    public static List<Order> GetOrderno(bool IsOrderStatus, bool IsProcessed, bool IsSoApprove, bool IsFullFillment, bool IsInvoice)
        //    {
        //        try
        //        {
        //            SqlConnection con1 = new SqlConnection(ConfigurationManager.ConnectionStrings["StoreConnectionString"].ToString());
        //            DynamicParameters ObjParm = new DynamicParameters();
        //            ObjParm.Add("@IsOrderStatus", IsOrderStatus);
        //            ObjParm.Add("@IsProcessed", IsProcessed);
        //            ObjParm.Add("@IsSoApprove", IsSoApprove);
        //            ObjParm.Add("@IsFullFillment", IsFullFillment);
        //            ObjParm.Add("@IsInvoice", IsInvoice);
        //            con1.Open();
        //            var order = con1.Query<Order>("USP_ShowOrder", ObjParm, commandType: CommandType.StoredProcedure).ToList();
        //            con1.Close();
        //            return order;
        //        }
        //        catch (Exception ex)
        //        {
        //            DebugLog(ex.Message + System.Environment.NewLine + ex.StackTrace);
        //            throw ex;
        //        }
        //    }
        #endregion
    }
}
