using BAL;
using DAL;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
namespace DbLayer.Repository
{
    public interface IOrderConfirmationRepository
    {
        //List<EscalationLevelMaster> GetEscalationLevelMasters(int CustId);
        //long AddEscalationLevelMaster(EscalationLevelMaster escalationLevelMaster);
        //EscalationLevelMaster GetEscalationLevelMasterById(int Id);
        //long EditEscalationLevelMaster(EscalationLevelMaster escalationLevelMaster);
        //List<Customer> GetCustomersForEscalationMatrix();
        //List<EscalationAction> GetEscalationActionsForCustomerLevelList(int loggedInCustomer, int levelID);
        //List<CustomerEmployee> GetEmployeesForCustomer(int loggedInCustomer);
        //EscalationAction GetEscalationAction(int id);
        //long EditEscalationAction(EscalationAction master);
        //long AddEscalationAction(EscalationAction action);
    }
    public class OrderConfirmationRepository : IOrderConfirmationRepository
    {
        private readonly StoreContext _context;
        public OrderConfirmationRepository()
        {
            _context = new StoreContext();
        }
        public OrderConfirmationRepository(StoreContext context)
        {
            _context = context;
        }

        public long AddEscalationLevelMaster(EscalationLevelMaster escalationLevelMaster)
        {
            try
            {
                return _context.Database.SqlQuery<long>("exec USP_AddEscalationLevelMaster @CustId, @IsDeleted, @LevelID, @ProcessId, @TriggerTime, @TriggerType, @CreatedBy, @CreatedDate",
                  new SqlParameter("@CustId", escalationLevelMaster.CustId),
                  new SqlParameter("@IsDeleted", escalationLevelMaster.IsDeleted),
                  new SqlParameter("@LevelID", escalationLevelMaster.LevelID),
                  new SqlParameter("@ProcessId", escalationLevelMaster.ProcessId),
                  new SqlParameter("@TriggerTime", escalationLevelMaster.TriggerTime),
                  new SqlParameter("@TriggerType", escalationLevelMaster.TriggerType),
                  new SqlParameter("@CreatedBy", escalationLevelMaster.CreatedBy),
                  new SqlParameter("@CreatedDate", escalationLevelMaster.CreatedDate)
                  ).FirstOrDefault();
            }
            catch (Exception ex)
            {
                HelperCls.DebugLog(ex.Message + System.Environment.NewLine + ex.StackTrace);
                throw ex;
            }
        }

        public long AddMailLog(int custId, long groupid, bool isMailSent, string mailList, int? emailTypeId)
        {
            try
            {
                return _context.Database.SqlQuery<long>(
                    string.Format("exec USP_AddOrderMailLog @CustId={0}, @GroupId={1},@IsMailSent={2},@MailList='{3}',@EmailTypeId={4}", custId,groupid,isMailSent,mailList,emailTypeId)
                  ).FirstOrDefault();
            }
            catch (Exception ex)
            {
                HelperCls.DebugLog(ex.Message + System.Environment.NewLine + ex.StackTrace);
                return 0;
            }
        }

        public List<Users> GetEmailsfromCCList(string cclist)
        {
            var ccemployees = Array.ConvertAll(cclist.Split(','), int.Parse).ToList();
            return _context.Users.Where(x => x.IsDeleted == false && ccemployees.Contains(x.Id)).ToList();
        }

        public long EditEscalationLevelMaster(EscalationLevelMaster escalationLevelMaster)
        {
            try
            {
                return _context.Database.SqlQuery<long>("exec USP_EditEscalationLevelMaster @Id, @CustId, @IsDeleted, @LevelID ,@ProcessId, @TriggerTime, @TriggerType, @ModifiedBy, @ModifiedDate",
                  new SqlParameter("@Id", escalationLevelMaster.Id),
                  new SqlParameter("@CustId", escalationLevelMaster.CustId),
                  new SqlParameter("@IsDeleted", escalationLevelMaster.IsDeleted),
                  new SqlParameter("@LevelID", escalationLevelMaster.LevelID),
                  new SqlParameter("@ProcessId", escalationLevelMaster.ProcessId),
                  new SqlParameter("@TriggerTime", escalationLevelMaster.TriggerTime),
                  new SqlParameter("@TriggerType", escalationLevelMaster.TriggerType),
                  new SqlParameter("@ModifiedBy", escalationLevelMaster.ModifiedBy),
                  new SqlParameter("@ModifiedDate", escalationLevelMaster.ModifiedDate)
                  ).FirstOrDefault();
            }
            catch (Exception ex)
            {
                HelperCls.DebugLog(ex.Message + System.Environment.NewLine + ex.StackTrace);
                throw ex;
            }
        }

        public List<CustomerEmailMaster> GetCustomersMailcontentType(DateTime triggerTime, int frequency)
        {
            try
            {
                return _context.Database.SqlQuery<CustomerEmailMaster>(
                    string.Format("exec GetCustomersMailcontentType @TriggeredDatetime='{0}',@Frequency='{1}'", triggerTime, frequency)
                  ).ToList();
            }
            catch (Exception ex)
            {
                HelperCls.DebugLog(ex.Message + System.Environment.NewLine + ex.StackTrace);
                throw ex;
            }
        }

        public List<OrderMailContent> GetCustomerMailContentfromcontentType(DateTime TriggerTime, int frequency, int custId, int? emailTypeId)
        {
            try
            {
                return _context.Database.SqlQuery<OrderMailContent>(
                    string.Format("exec USP_GetMailDataforDatetimeEmailType @TriggeredDatetime='{0}', @EmailType='{1}',@CustId='{2}',@Frequency='{3}'", TriggerTime.ToString("yyyy-MM-dd HH:mm:ss"), emailTypeId,custId,frequency)
                  ).ToList();
            }
            catch (Exception ex)
            {
                HelperCls.DebugLog(ex.Message + System.Environment.NewLine + ex.StackTrace);
                throw ex;
            }
        }

        public EscalationLevelMaster GetEscalationLevelMasterById(int Id)
        {
            try
            {
                return _context.EscalationLevelMasters.Where(x => x.Id == Id).FirstOrDefault();
            }
            catch (Exception ex)
            {
                HelperCls.DebugLog(ex.Message + System.Environment.NewLine + ex.StackTrace);
                throw ex;
            }
        }
        
    }
}
