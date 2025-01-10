using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BAL;
using DAL;
using Dapper;

namespace DbLayer.Repository
{
    public interface IAuditRepository
    {
        Int64 InsertAudit(Audit audit);
    }

    public class AuditRepository : IAuditRepository
    {
        public SqlConnection con;
        //To Handle connection related activities      
        private void connection()
        {
            string constr = ConfigurationManager.ConnectionStrings["StoreConnectionString"].ToString();
            con = new SqlConnection(constr);
        }

        public Int64 InsertAudit(Audit audit)
        {
            var a =new Int64();
            try
            {
                DynamicParameters ObjParm = new DynamicParameters();
                ObjParm.Add("@IpAddress", audit.IpAddress);
                ObjParm.Add("@JsonData", audit.JsonData);
                ObjParm.Add("@Link", audit.Link);
                ObjParm.Add("@UserId", audit.UserId == 0 ? null : audit.UserId);
                ObjParm.Add("@StoreId", audit.StoreId == 0 ? null : audit.StoreId);
                connection();
                con.Open();
                a = con.Query<Int64>("USP_InsertAudit", ObjParm, commandType: CommandType.StoredProcedure).FirstOrDefault();
            }
            catch(Exception ex)
            {
                HelperCls.DebugLog(ex.Message + System.Environment.NewLine + ex.StackTrace);
                //throw ex;
            }
            finally
            {
                con.Close();
            }
            return a;
            }

    }
}
