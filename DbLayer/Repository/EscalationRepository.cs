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
    public interface IEscalationRepository
    {
        List<EscalationLevelMaster> GetEscalationLevelMasters(int CustId);
        long AddEscalationLevelMaster(EscalationLevelMaster escalationLevelMaster);
        EscalationLevelMaster GetEscalationLevelMasterById(int Id);
        long EditEscalationLevelMaster(EscalationLevelMaster escalationLevelMaster);
        List<Customer> GetCustomersForEscalationMatrix();
        List<EscalationAction> GetEscalationActionsForCustomerLevelList(int loggedInCustomer, int levelID);
        List<CustomerEmployee> GetEmployeesForCustomer(int loggedInCustomer);
        EscalationAction GetEscalationAction(int id);
        long EditEscalationAction(EscalationAction master);
        long AddEscalationAction(EscalationAction action);
    }
    public class EscalationRepository : IEscalationRepository
    {
        private readonly StoreContext _context;
        public EscalationRepository()
        {
            _context = new StoreContext();
        }
        public EscalationRepository(StoreContext context)
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

        public List<EscalationLevelMaster> GetEscalationLevelMasters(int CustId)
        {
            try
            {
                return _context.EscalationLevelMasters.Where(x => x.CustId == CustId && x.Customer.IsDeleted == false).ToList();
                //return _context.EscalationLevelMasters.Where(x => /*x.IsDeleted == false&&*/x.CustId==CustId&&x.Customer.IsDeleted==false).ToList();
            }
            catch (Exception ex)
            {
                HelperCls.DebugLog(ex.Message + System.Environment.NewLine + ex.StackTrace);
                throw ex;
            }
        }
        public List<Customer> GetCustomersForEscalationMatrix()
        {
            try
            {
                return _context.Customers.Where(x => x.IsDeleted == false && x.HasEscalationMatrix && x.EscalationLevelMaster.Count() > 0).ToList();
            }
            catch (Exception ex)
            {
                HelperCls.DebugLog(ex.Message + System.Environment.NewLine + ex.StackTrace);
                throw ex;
            }
        }
        public List<EscalationLevelMaster> GetCustomersLevelsForTriggeredTime(TimeSpan Time, int Frequency)
        {
            //var Customers=_context.Customers.Where(x => x.IsDeleted == false && x.HasEscalationMatrix && x.EscalationLevelMaster.Count() > 0).ToList();
            try
            {
                var timespan = TimeSpan.FromMinutes(Frequency);
                var timetocompare = Time.Subtract(timespan);
                //var TriggeredEscalations = _context.EscalationLevelMasters.Where(
                //    x => x.Customer.IsDeleted == false && x.IsDeleted == false &&
                //    x.Customer.HasEscalationMatrix && x.TriggerTime <= Time &&
                //    x.TriggerTime>= timetocompare
                //    ).ToList();
                return _context.Database.SqlQuery<EscalationLevelMaster>("exec USP_GetCustomersLevelsForTriggeredTime @TriggerTime,@TriggerFrequency",
                  new SqlParameter("@TriggerTime", Time),
                  new SqlParameter("@TriggerFrequency", Frequency)).ToList();
                //return TriggeredEscalations;
            }
            catch (Exception ex)
            {
                HelperCls.DebugLog(ex.Message + System.Environment.NewLine + ex.StackTrace);
                throw ex;
            }
        }
        public List<EscalationAction> GetEscalationActionsForCustomerLevel(int CustID, int LevelId)
        {
            try
            {
                //var TriggeredAction = _context.EscalationActions.Where(x => x.Store.CustId == CustID && x.Store.IsDeleted == false && x.Level == LevelId).ToList();

                var Date = DateTime.Now.Date;
                var OrderedStores = _context.orderHeaders.Where(x => x.OrderDate == Date && x.Store.CustId == CustID && x.IsOrderStatus == true).Select(x => x.StoreId).ToList();
                //var TriggeredAction = (from EscalationAction in _context.EscalationActions
                //                   join OrderHeader in _context.orderHeaders
                //                   on EscalationAction.StoreId equals OrderHeader.StoreId 
                //                   where OrderHeader.IsOrderStatus == false && EscalationAction.Store.CustId == CustID &&
                //                   EscalationAction.Store.IsDeleted == false && EscalationAction.Level == LevelId
                //                   && OrderHeader.OrderDate== Date && OrderHeader.OrderSource==1
                //                   && OrderHeader.Isdeleted == false
                //                   select EscalationAction).ToList();
                var TriggeredAction = (from EscalationAction in _context.EscalationActions
                                       where EscalationAction.Store.CustId == CustID &&
                                       !OrderedStores.Contains(EscalationAction.StoreId) &&
                                       EscalationAction.Store.IsDeleted == false && EscalationAction.Level == LevelId
                                       select EscalationAction).ToList();
                return TriggeredAction;
            }
            catch (Exception ex)
            {
                HelperCls.DebugLog(ex.Message + System.Environment.NewLine + ex.StackTrace);
                throw ex;
            }
        }
        public List<EscalationAction> GetEscalationActionsForCustomerLevelList(int loggedInCustomer, int levelID)
        {
            try
            {
                var EscalationActionsist = _context.EscalationActions.Where(x => x.Store.CustId == loggedInCustomer && x.Level == levelID && x.Store.IsDeleted == false && x.Store.Customer.IsDeleted == false).OrderBy(x => x.Level).ToList();
                return EscalationActionsist;
            }
            catch (Exception ex)
            {
                HelperCls.DebugLog(ex.Message + System.Environment.NewLine + ex.StackTrace);
                throw ex;
            }
        }
        public List<CustomerEmployee> GetEmployeesForCustomer(int loggedInCustomer)
        {
            try
            {
                var CustomerEmployees = _context.customerEmployees.Where(x => x.CustId == loggedInCustomer && x.IsDeleted == false && x.Customer.IsDeleted == false).ToList();
                return CustomerEmployees;
            }
            catch (Exception ex)
            {
                HelperCls.DebugLog(ex.Message + System.Environment.NewLine + ex.StackTrace);
                throw ex;
            }
        }
        public EscalationAction GetEscalationAction(int id)
        {
            try
            {
                var EscalationAction = _context.EscalationActions.Where(x => x.Id == id && x.IsDeleted == false ).FirstOrDefault();
                return EscalationAction;
            }
            catch (Exception ex)
            {
                HelperCls.DebugLog(ex.Message + System.Environment.NewLine + ex.StackTrace);
                throw ex;
            }
        }
        public long EditEscalationAction(EscalationAction escalationAction)
        {
            try
            {
                return _context.Database.SqlQuery<long>("exec USP_EditEscalationAction @Id, @LevelID, @StoreId, @CustEmp, @EmailTo_CC_BCCFlag, @IsDeleted, @ModifiedBy, @ModifiedDate",
                  new SqlParameter("@Id", escalationAction.Id),
                  new SqlParameter("@LevelID", escalationAction.Level),
                  new SqlParameter("@StoreId", escalationAction.StoreId),
                  new SqlParameter("@CustEmp", escalationAction.CustEmpId),
                  new SqlParameter("@EmailTo_CC_BCCFlag", escalationAction.EmailTo_CC_BCCFlag),
                  new SqlParameter("@IsDeleted", escalationAction.IsDeleted),                            
                  new SqlParameter("@ModifiedBy", escalationAction.ModifiedBy),
                  new SqlParameter("@ModifiedDate", escalationAction.ModifiedDate)
                  ).FirstOrDefault();
            }
            catch (Exception ex)
            {
                HelperCls.DebugLog(ex.Message + System.Environment.NewLine + ex.StackTrace);
                throw ex;
            }
        }
        public long AddEscalationAction(EscalationAction escalationAction)
        {
            try
            {
                return _context.Database.SqlQuery<long>("exec USP_AddEscalationAction @LevelID, @StoreId, @CustEmp, @EmailTo_CC_BCCFlag, @IsDeleted, @CreatedBy, @CreatedDate",
                  new SqlParameter("@LevelID", escalationAction.Level),
                  new SqlParameter("@StoreId", escalationAction.StoreId),
                  new SqlParameter("@CustEmp", escalationAction.CustEmpId),
                  new SqlParameter("@EmailTo_CC_BCCFlag", escalationAction.EmailTo_CC_BCCFlag),
                  new SqlParameter("@IsDeleted", escalationAction.IsDeleted),
                  new SqlParameter("@CreatedBy", escalationAction.CreatedBy),
                  new SqlParameter("@CreatedDate", escalationAction.CreatedDate)
                  ).FirstOrDefault();
            }
            catch (Exception ex)
            {
                HelperCls.DebugLog(ex.Message + System.Environment.NewLine + ex.StackTrace);
                throw ex;
            }
        }
    }
}
