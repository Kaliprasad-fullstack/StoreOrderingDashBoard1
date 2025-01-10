using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL;
using BAL;
using System.Data.SqlClient;
using System.Data;

namespace DbLayer.Repository
{
    public interface IStoreRepository
    {
        int AddStore(Store store);
        int AddStore2(Store store,int warehouseid,int NetSuitMapCls);
        List<Store> GetAllStores();
        Store GetStoreById(int Id);
        int EditStore(Store store);
        int DeleteStore(Store store);
        Store Login(string Username, string Password);
        Store GetStore(string Username);
        Users GetUsers(string Username);
        List<ProcessCls> Store();
        List<Store> GetStoreByCustomerId(int customerId);
        Store GetStoreByEmailAddress(Store store);
        WareHouseDC WareHouseDC(int id);
        NetSuitMapCls NetSuitMapCl(int id);
        StoreLogin StoreLogin(string UserName, string Password, string IpAddress);
        StoreLogin StoreLoginSessionExtensionDetails(string UserName, string IpAddress);
        Users GetUserById(int Id);
        int EditUser(Users storeuser);
        List<StoreUser> GetUserByStoreId(int storeId);
        int ResetPasswordForUser(Users storeuser, int LoggedInUserId);
        long AddStoreExcel(StoreUserExcelImport store, int userid);
        long AddStoreUserExcel(StoreUserExcelImport storeUser, int userid);
        UserStore GetUserStore(int id);
        int DeleteStoreUser(int storeuserId);
        List<Store> GetAvailableStoresforUserId(int custid, int userid);
        List<StoreView> GetStoresforUserCustomer(int userid, int custid,string OrderReport);
        List<CityView> GetCitiesforUserCustomer(int userid, int custid);
        List<StoreView> GetStoresForWareHouse(int userId, int custId, List<int> selectedWareHouseDCs,string OrderReport, List<int> Selectedlocations=null);
    }
    public class StoreRepository : IStoreRepository
    {
        private readonly StoreContext _context;
        public StoreRepository(StoreContext context)
        {
            _context = context;
        }
        public int AddStore(Store store)
        {
            try
            {
                _context.Stores.Add(store);
                int rowsaffected = _context.SaveChanges();
                return rowsaffected;
            }
            catch (Exception ex)
            {
                HelperCls.DebugLog(ex.Message + System.Environment.NewLine + ex.StackTrace);
                throw ex;
            }
        }
        public int AddStore2(Store store,int warehouseid,int NetSuitMapCls)
        {
            try
            {
                int id=0;


                string consString = System.Configuration.ConfigurationManager.ConnectionStrings["StoreConnectionString"].ConnectionString;// _context.Database.Connection.ConnectionString;
                using (SqlConnection con = new SqlConnection(consString))
                {
                    using (SqlCommand cmd = new SqlCommand("USP_AddStore"))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Connection = con;
                        cmd.Parameters.AddWithValue("@CustId", store.CustId);
                     cmd.Parameters.AddWithValue("@StoreCode", store.StoreCode);
                     cmd.Parameters.AddWithValue("@StoreName", store.StoreName);
                     cmd.Parameters.AddWithValue("@StoreEmailId", store.StoreEmailId);
                      cmd.Parameters.AddWithValue("@WareHouseDC_Id", warehouseid);
                     cmd.Parameters.AddWithValue("@PlaceOfSupply", store.PlaceOfSupply);
                     cmd.Parameters.AddWithValue("@Address", store.Address);
                     cmd.Parameters.AddWithValue("@StoreManager", store.StoreManager);
                     cmd.Parameters.AddWithValue("@StoreContactNo", store.StoreContactNo);
                     cmd.Parameters.AddWithValue("@CreatedBy", store.CreatedBy);
                     cmd.Parameters.AddWithValue("@ModifiedBy", store.ModifiedBy);
                     cmd.Parameters.AddWithValue("@LocationId", store.LocationId);
                      cmd.Parameters.AddWithValue("@NetSuitMapCls_Id", NetSuitMapCls);
                        cmd.Parameters.Add("@retValue", System.Data.SqlDbType.Int).Direction = System.Data.ParameterDirection.ReturnValue;

                        con.Open();

                      
                     
                        cmd.ExecuteNonQuery();
                       id = (int)cmd.Parameters["@retValue"].Value;

                        con.Close();
                    }
                }
                int rowsaffected = id;
                return rowsaffected;
            }
            catch (Exception ex)
            {
                HelperCls.DebugLog(ex.Message + System.Environment.NewLine + ex.StackTrace);
                throw ex;
            }
        }
      
        public List<Store> GetAllStores()
        {
            try
            {
                return _context.Stores.Where(x => x.IsDeleted == false).ToList();
            }
            catch (Exception ex)
            {
                HelperCls.DebugLog(ex.Message + System.Environment.NewLine + ex.StackTrace);
                throw ex;
            }
        }
        public Store GetStoreById(int Id)
        {
            try
            {
                return _context.Stores.Where(x => x.Id == Id && x.IsDeleted == false).FirstOrDefault();
            }
            catch (Exception ex)
            {
                HelperCls.DebugLog(ex.Message + System.Environment.NewLine + ex.StackTrace);
                throw ex;
            }

        }
        public int EditStore(Store store)
        {
            try
            {
                //_context.Entry(Store).CurrentValues.SetValues(store);
                _context.Entry(store).State = EntityState.Modified;
                int rowsaffected = _context.SaveChanges();
                return rowsaffected;
            }
            catch (System.Data.Entity.Validation.DbEntityValidationException dbEx)
            {
                Exception raise = dbEx;
                foreach (var validationErrors in dbEx.EntityValidationErrors)
                {
                    foreach (var validationError in validationErrors.ValidationErrors)
                    {
                        string message = string.Format("{0}:{1}",
                            validationErrors.Entry.Entity.ToString(),
                            validationError.ErrorMessage);
                        // raise a new exception nesting  
                        // the current instance as InnerException  
                        raise = new InvalidOperationException(message, raise);
                    }
                }
                HelperCls.DebugLog(dbEx.Message + System.Environment.NewLine + dbEx.StackTrace);
                throw raise;
            }
        }
        public int DeleteStore(Store store)
        {
            StoreItemMst storeitemmst=new StoreItemMst();
            try
            {
                //store.ModifiedDate = DateTime.Now;
                //store.IsDeleted = true;
                //_context.Entry(store).State = EntityState.Modified;
                //int rowsaffected = _context.SaveChanges();

                //return _context.Database.ExecuteSqlCommand("exec USP_DeleteUser @UserId",
                //  new SqlParameter("@UserId", storeuserId));

                int return1= _context.Database.ExecuteSqlCommand("exec USP_StoreDelete @StoreId,@CustId",
                    new SqlParameter("@StoreId", store.Id),
                    new SqlParameter("@CustId", store.CustId)     );

                return return1;
            }
            catch (System.Data.Entity.Validation.DbEntityValidationException dbEx)
            {
                Exception raise = dbEx;
                foreach (var validationErrors in dbEx.EntityValidationErrors)
                {
                    foreach (var validationError in validationErrors.ValidationErrors)
                    {
                        string message = string.Format("{0}:{1}",
                            validationErrors.Entry.Entity.ToString(),
                            validationError.ErrorMessage);
                        // raise a new exception nesting  
                        // the current instance as InnerException  
                        raise = new InvalidOperationException(message, raise);
                    }
                }
                HelperCls.DebugLog(dbEx.Message + System.Environment.NewLine + dbEx.StackTrace);
                throw raise;
            }
        }

        public Store Login(string Username, string Password)
        {
            try
            {
                //string pass = HelperCls.DecodeServerName(Password);
                //var a = _context.Stores.ToList();
                return _context.Stores.Where(x => x.StoreEmailId == Username && x.Password == Password && x.IsDeleted == false).FirstOrDefault();
            }
            catch (Exception ex)
            {
                HelperCls.DebugLog(ex.Message + System.Environment.NewLine + ex.StackTrace);
                throw ex;
            }
        }

        public Store GetStore(string Username)
        {
            try
            {
                return _context.Stores.Where(x => x.StoreEmailId == Username).FirstOrDefault();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public Users GetUsers(string Username)
        {
            try
            {
                return _context.Users.Where(x => x.EmailAddress == Username && x.RoleId == 1 && x.IsDeleted != true).FirstOrDefault();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public Store GetStoreByEmailAddress(Store store)
        {
            try
            {
                return _context.Stores.Where(x => x.StoreCode == store.StoreCode || x.StoreEmailId == store.StoreEmailId||x.StoreName==store.StoreName).FirstOrDefault();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<ProcessCls> Store()
        {
            try
            {
                return _context.Database.SqlQuery<ProcessCls>("exec USP_GetFiniliseOrder").ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<Store> GetStoreByCustomerId(int customerId)
        {
            try
            {
                if (customerId == 0) { return _context.Stores.Where(x => x.IsDeleted == false).ToList(); }
                else { return _context.Stores.Where(x => x.CustId == customerId && x.IsDeleted == false).ToList(); }
                
            }
            catch (Exception ex)
            {
                HelperCls.DebugLog(ex.Message + System.Environment.NewLine + ex.StackTrace);
                throw ex;
            }
        }

        public WareHouseDC WareHouseDC(int id)
        {
            try
            {
                return _context.wareHouseDCs.Find(id);
            }
            catch (Exception ex)
            {
                HelperCls.DebugLog(ex.Message + System.Environment.NewLine + ex.StackTrace);
                throw ex;
            }
        }

        public NetSuitMapCls NetSuitMapCl(int id)
        {
            try
            {
                return _context.netSuitMapCls.Find(id);
            }
            catch (Exception ex)
            {
                HelperCls.DebugLog(ex.Message + System.Environment.NewLine + ex.StackTrace);
                throw ex;
            }
        }

        public StoreLogin StoreLogin(string UserName, string Password, string IpAddress)
        {
            try
            {
                return _context.Database.SqlQuery<StoreLogin>("exec USP_StoreLogin @Email,@Password,@CurrentIp",
                   new SqlParameter("@Email", UserName),
                   new SqlParameter("@Password", Password),
                   new SqlParameter("@CurrentIp", IpAddress)).FirstOrDefault();
            }
            catch (Exception ex)
            {
                HelperCls.DebugLog(ex.Message + System.Environment.NewLine + ex.StackTrace);
                throw ex;
            }
        }

        public StoreLogin StoreLoginSessionExtensionDetails(string UserName, string IpAddress)
        {
            try
            {
                return _context.Database.SqlQuery<StoreLogin>("exec [USP_StoreLoginExtensionDetials] @Email,@CurrentIp",
                   new SqlParameter("@Email", UserName),
                   new SqlParameter("@CurrentIp", IpAddress)).FirstOrDefault();
            }
            catch (Exception ex)
            {
                HelperCls.DebugLog(ex.Message + System.Environment.NewLine + ex.StackTrace);
                throw ex;
            }
        }
        public Users GetUserById(int Id)
        {
            try
            {
                return _context.Users.Where(x => x.Id == Id && x.RoleId == 1).FirstOrDefault();
            }
            catch (Exception ex)
            {
                HelperCls.DebugLog(ex.Message + System.Environment.NewLine + ex.StackTrace);
                throw ex;
            }

        }
        public int EditUser(Users storeuser)
        {
            try
            {
                //_context.Entry(Store).CurrentValues.SetValues(store);
                _context.Entry(storeuser).State = EntityState.Modified;
                int rowsaffected = _context.SaveChanges();
                return rowsaffected;
            }
            catch (System.Data.Entity.Validation.DbEntityValidationException dbEx)
            {
                Exception raise = dbEx;
                foreach (var validationErrors in dbEx.EntityValidationErrors)
                {
                    foreach (var validationError in validationErrors.ValidationErrors)
                    {
                        string message = string.Format("{0}:{1}",
                            validationErrors.Entry.Entity.ToString(),
                            validationError.ErrorMessage);
                        // raise a new exception nesting  
                        // the current instance as InnerException  
                        raise = new InvalidOperationException(message, raise);
                    }
                }
                HelperCls.DebugLog(dbEx.Message + System.Environment.NewLine + dbEx.StackTrace);
                throw raise;
            }
        }
        public List<StoreUser> GetUserByStoreId(int StoreId)
        {
            try
            {
                return _context.Database.SqlQuery<StoreUser>("exec USP_GetUserByStoreId @StoreId",
                   new SqlParameter("@StoreId", StoreId)).ToList();
                //List<StoreUser> StoreUsers = _context.userStores.Where(x => x.StoreId == StoreId).Join(_context.Users.Where(x => x.IsDeleted == false), x => x.UserId, s => s.Id, ((x, s) => new StoreUser(x.MkrChkrFlag, x.StoreId, s.Id, s.Name, s.Department, s.EmailAddress, s.Password, s.IsDeleted, s.ReportToId, s.LocationId, s.RoleId, s.CreatedOn, s.CreatedBy, s.Modified, s.ModifiedBy, s.CustomerPlan.Id))).ToList();
                //return StoreUsers;
            }
            catch (Exception ex)
            {
                HelperCls.DebugLog(ex.Message + System.Environment.NewLine + ex.StackTrace);
                throw ex;
            }

        }
        public int ResetPasswordForUser(Users storeuser, int LoggedInUserId)
        {
            try
            {
                return _context.Database.SqlQuery<int>("exec USP_ResetPasswordForStoreUser @UsersId",
                    new SqlParameter("@UsersId", storeuser.Id),
                    new SqlParameter("@ModifiedById", LoggedInUserId)
                    ).FirstOrDefault();
            }
            catch (Exception ex)
            {
                HelperCls.DebugLog(ex.Message + System.Environment.NewLine + ex.StackTrace);
                throw ex;
            }
        }
        public long AddStoreExcel(StoreUserExcelImport store, int userid)
        {
            try
            {

                return _context.Database.SqlQuery<Int64>("exec USP_AddStoreExcel @Address,@CustomerName,@Location,@PlaceOfSupply,@Region,@Route,@StoreCode,@StoreContactNo,@StoreEmailId,@StoreManager,@StoreName,@WarehouseDc,@NetSuitMapCls_Id,@CreatedBy",
                    new SqlParameter("@Address", store.Address),
                    new SqlParameter("@CustomerName", store.CustomerName),
                    new SqlParameter("@Location", store.Location),
                    new SqlParameter("@PlaceOfSupply", store.PlaceOfSupply),
                    new SqlParameter("@Region", store.Region),
                    new SqlParameter("@Route", store.Route),
                    new SqlParameter("@StoreCode", store.StoreCode),
                    new SqlParameter("@StoreContactNo", store.StoreContactNo),
                    new SqlParameter("@StoreEmailId", store.StoreEmailId),
                    new SqlParameter("@StoreManager", store.StoreManager),
                    new SqlParameter("@StoreName", store.StoreName),
                    new SqlParameter("@WarehouseDc", store.WarehouseDc),
                    new SqlParameter("@NetSuitMapCls_Id", store.NetSuitMapCls_Id),
                    new SqlParameter("@CreatedBy", userid)
                    ).FirstOrDefault();
            }
            catch (Exception ex)
            {
                HelperCls.DebugLog(ex.Message + System.Environment.NewLine + ex.StackTrace);
                //throw ex;
                return 0;
            }
        }

        public long AddStoreUserExcel(StoreUserExcelImport storeUser, int userid)
        {
            try
            {

                return _context.Database.SqlQuery<Int64>("exec USP_AddStoreUserExcel @UserEmailAddress,@Location,@PlanName,@CustomerName,@MkrChkrFlag,@StoreCode,@StoreId,@CreatedBy",
                    new SqlParameter("@UserEmailAddress", storeUser.UserEmailAddress),
                    new SqlParameter("@Location", storeUser.Location),
                    new SqlParameter("@PlanName", storeUser.PlanName),
                    new SqlParameter("@CustomerName", storeUser.CustomerName),
                    new SqlParameter("@MkrChkrFlag", storeUser.MkrChkrFlag),
                    new SqlParameter("@StoreCode", storeUser.StoreCode),
                    new SqlParameter("@StoreId", storeUser.StoreId),
                    //new SqlParameter("@StoreEmailId", store.StoreEmailId),
                    //new SqlParameter("@StoreManager", store.StoreManager),
                    //new SqlParameter("@StoreName", store.StoreName),
                    //new SqlParameter("@WarehouseDc", store.WarehouseDc),
                    new SqlParameter("@CreatedBy", userid)
                    ).FirstOrDefault();
            }
            catch (Exception ex)
            {
                HelperCls.DebugLog(ex.Message + System.Environment.NewLine + ex.StackTrace);
                //throw ex;
                return 0;
            }
        }

        public UserStore GetUserStore(int id)
        {
            try
            {
                return _context.userStores.Where(x => x.UserId == id).FirstOrDefault();
            }
            catch (Exception ex)
            {
                HelperCls.DebugLog(ex.Message + System.Environment.NewLine + ex.StackTrace);
                throw ex;
            }
        }

        public int DeleteStoreUser(int storeuserId)
        {
            try
            {
                return _context.Database.ExecuteSqlCommand("exec USP_DeleteUser @UserId",
                   new SqlParameter("@UserId", storeuserId));
            }
            catch (Exception ex)
            {
                HelperCls.DebugLog(ex.Message + System.Environment.NewLine + ex.StackTrace);
                throw ex;
            }
        }
        public List<Store> GetAvailableStoresforUserId(int custid, int userid)
        {
            try
            {
                var allowedcustomer = _context.Permitables.Where(x => x.Users.Id == userid && (x.Customer.Id == custid && x.Customer.IsDeleted == false) && x.IsDeleted == false).Select(x => x.Customer.Id).Distinct().ToList();
                var allowedstores = _context.Permitables.Where(x => x.Users.Id == userid && x.Store != null && x.IsDeleted == false).Select(x => x.Store.Id).ToList();
                var stores = from store in _context.Stores
                             where allowedstores.Contains(store.Id) && store.Customer.IsDeleted == false || (allowedcustomer.Contains(store.Customer.Id) && store.IsDeleted == false) && store.Customer.IsDeleted == false
                             select store;
                return stores.ToList();
            }
            catch (Exception ex)
            {
                HelperCls.DebugLog(ex.Message + System.Environment.NewLine + ex.StackTrace);
                throw ex;
            }
        }
        public List<StoreView> GetStoresforUserCustomer(int userid, int custid,string OrderReport)
        {
            try
            {
                return _context.Database.SqlQuery<StoreView>(
                    string.Format("exec usp_getwarehousestoreforcustomer @userid={0}, @custid={1}, @calltype={2}, @orderreport='{3}'", userid, custid, 1, OrderReport)).ToList();
                //return _context.Database.SqlQuery<StoreView>(
                //    string.Format("exec USP_GetWareHouseStoreForCustomer @UserId={0}, @CustId={1}, @CallType={2}", userid, custid, 1)).ToList();
            }
            catch (Exception ex)
            {
                HelperCls.DebugLog(ex.Message + System.Environment.NewLine + ex.StackTrace);
                throw ex;
            }
        }
        public List<CityView> GetCitiesforUserCustomer(int userid, int custid)
        {
            try
            {
                return _context.Database.SqlQuery<CityView>(
                    string.Format("exec USP_GetWareHouseStoreForCustomer @UserId={0}, @CustId={1}, @CallType={2}", userid, custid, 6)).ToList();
            }
            catch (Exception ex)
            {
                HelperCls.DebugLog(ex.Message + System.Environment.NewLine + ex.StackTrace);
                throw ex;
            }
        }
        public List<StoreView> GetStoresForWareHouse(int userId, int custId, List<int> selectedWareHouseDCs,string OrderReport, List<int> Selectedlocations=null)
        {
            try
            {
                return _context.Database.SqlQuery<StoreView>(
                    string.Format("exec USP_GetWareHouseStoreForCustomer @UserId={0}, @CustId={1}, @CallType={2},@DCs='{3}',@Locations ='{4}', @OrderReport='{5}'",
                    userId,
                    custId,
                    7,
                    string.Join(",", selectedWareHouseDCs.ToArray()),
                    Selectedlocations != null ? string.Join(",", Selectedlocations.ToArray()) : "",
                    OrderReport)
                    //string.Format("exec USP_GetWareHouseStoreForCustomer @UserId={0}, @CustId={1}, @CallType={2},@DCs='{3}',@Locations ='{4}'",
                    //userId,
                    //custId,
                    //7,
                    //string.Join(",", selectedWareHouseDCs.ToArray()),
                    //Selectedlocations != null ? string.Join(",", Selectedlocations.ToArray()) : "")                    
                    ).ToList();
            }
            catch (Exception ex)
            {
                HelperCls.DebugLog(ex.Message + System.Environment.NewLine + ex.StackTrace);
                throw ex;
            }
        }
    }
}
