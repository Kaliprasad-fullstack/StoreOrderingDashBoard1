using DAL;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BAL;
using System.Data.SqlClient;
using System.Data.Common;
using System.Data;

namespace DbLayer.Repository
{
    public interface ICustomerRepository
    {
        List<Customer> GetAllCustomers();
        int AddCustomer(Customer customer);
        int EditCustomer(Customer customer);
        Customer GetCustomerById(int id);
        ICollection<Customer> GetSelectedcustomers(IEnumerable<int> selectedCustomers);
        List<CustomerList> customerLists(int UserId);
        Answer PasswordCustomer(string password, int userid, int QuestionId);
        List<AllQuestion> GetQuestion(int UserId);
        List<Company> GetCompanies();
        List<CustomerPlan> GetCustomerPlans();
        Company Company(int id);
        CustomerPlan CustomerPlan(int id);
        Customer Customer(string CustName);
        List<AllQuestion> allQuestions();
        List<NetSuitMapCls> GetNetSuiteCls();
        List<Answer> GetQuestionAnswersForUser(int id);
        List<Customer> GetCustomerforUserId(int custId, int userId);
        List<EmailType> GetCustomerEmailTypes();
        List<CustomerEmailMaster> GetSelectedEmailsTypesForCustomer(int id);
        void UpdateCustomerEmailTypes(Customer customers);
        Customer GetAutoInventoryLogicFlag(int userId, int custId);
        int AddUpdateStatus(int custId, string type, string status, int days, int userId);
        CustomerWiseStatusMst GetCustomerWiseStatusMst(int id, string type);
        string InsertItemsBulk(List<ItemMasterUploadViewModel> lstOrderExcel, long fileId, string fileContentType);
        string InsertStoresBulk(List<StoreMasterUploadViewModel> lstOrderExcel, long fileId, string fileContentType);
        List<TicketDetails> GetTickets(int roleId, int userId, string orderNumber, List<string> ticketStatus, List<int> ticketTypeIds, DateTime? fromDate, DateTime? toDate, int storeId, int? loggedInCustId, List<int> selectedstoreIds);
        string SaveTicketFile(TicketImages t);
        List<TicketImages> GetTicketImages(int TicketId);
        string DeleteTicketImage(int imageId, int ticketId, int userId);
        DataTable GetReportDatable(int roleId, int userId, string orderNumber, List<string> ticketStatus, List<int> ticketTypeIds, DateTime? fromDate, DateTime? toDate, int storeId, int? loggedInCustId, List<int> selectedstoreIds);
    }
    public class CustomerRepository : ICustomerRepository
    {
        private readonly StoreContext _context;
        private readonly object x;

        public CustomerRepository()
        {
        }
        public CustomerRepository(StoreContext context)
        {
            _context = context;
        }
        public List<Customer> GetAllCustomers()
        {
            try
            {
                return _context.Customers.Where(x => x.IsDeleted == false).ToList();
            }
            catch (Exception ex)
            {
                HelperCls.DebugLog(ex.Message + System.Environment.NewLine + ex.StackTrace);
                throw ex;
            }
        }
        public int AddCustomer(Customer customer)
        {
            try
            {
                _context.Customers.Add(customer);
                int rowsaffected = _context.SaveChanges();
                return rowsaffected;
            }
            catch (Exception ex)
            {
                HelperCls.DebugLog(ex.Message + System.Environment.NewLine + ex.StackTrace);
                throw ex;
            }
        }
        public Customer GetCustomerById(int Id)
        {
            try
            {
                return _context.Customers.Find(Id);
            }
            catch (Exception ex)
            {
                HelperCls.DebugLog(ex.Message + System.Environment.NewLine + ex.StackTrace);
                throw ex;
            }

        }
        public int EditCustomer(Customer customer)
        {
            try
            {
                _context.Entry(customer).State = EntityState.Modified;
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

        public ICollection<Customer> GetSelectedcustomers(IEnumerable<int> selectedCustomers)
        {
            try
            {
                return _context.Customers.Where(x => selectedCustomers.Contains(x.Id) && x.IsDeleted == false).ToList();
            }
            catch (Exception ex)
            {
                HelperCls.DebugLog(ex.Message + System.Environment.NewLine + ex.StackTrace);
                throw ex;
            }
        }

        public List<CustomerList> customerLists(int UserId)
        {
            try
            {
                return _context.Database.SqlQuery<CustomerList>("exec USP_CustomerListByUser @UserId",
                   new SqlParameter("@UserId", UserId)).ToList();
            }
            catch (Exception ex)
            {
                HelperCls.DebugLog(ex.Message + System.Environment.NewLine + ex.StackTrace);
                throw ex;
            }
        }

        public Answer PasswordCustomer(string password, int userid, int QuestionId)
        {
            try
            {
                // var st = HelperCls.EncodeServerName(password);
                return _context.answers.Where(x => x.Users.Id == userid && x.Answers == password && x.Question.Id == QuestionId && x.Question.IsDeleted == false).FirstOrDefault();
            }
            catch (Exception ex)
            {
                HelperCls.DebugLog(ex.Message + System.Environment.NewLine + ex.StackTrace);
                throw ex;
            }
        }

        public List<AllQuestion> GetQuestion(int UserId)
        {
            try
            {
                return _context.Database.SqlQuery<AllQuestion>("exec USP_AllQuestion @UserId",
                  new SqlParameter("@UserId", UserId)).ToList();
            }
            catch (Exception ex)
            {
                HelperCls.DebugLog(ex.Message + System.Environment.NewLine + ex.StackTrace);
                throw ex;
            }
        }

        public List<Company> GetCompanies()
        {
            try
            {
                return _context.Companies.Where(x => x.isDeleted == false).ToList();
            }
            catch (Exception ex)
            {
                HelperCls.DebugLog(ex.Message + System.Environment.NewLine + ex.StackTrace);
                throw ex;
            }
        }

        public List<CustomerPlan> GetCustomerPlans()
        {
            try
            {
                return _context.customerPlans.Where(x => x.IsDeleted == false).ToList();
            }
            catch (Exception ex)
            {
                HelperCls.DebugLog(ex.Message + System.Environment.NewLine + ex.StackTrace);
                throw ex;
            }
        }

        public Company Company(int id)
        {
            try
            {
                return _context.Companies.Where(x => x.isDeleted == false && x.Id == id).FirstOrDefault();
            }
            catch (Exception ex)
            {
                HelperCls.DebugLog(ex.Message + System.Environment.NewLine + ex.StackTrace);
                throw ex;
            }
        }

        public CustomerPlan CustomerPlan(int id)
        {
            try
            {
                return _context.customerPlans.Where(x => x.IsDeleted == false && x.Id == id).FirstOrDefault();
            }
            catch (Exception ex)
            {
                HelperCls.DebugLog(ex.Message + System.Environment.NewLine + ex.StackTrace);
                throw ex;
            }
        }

        public Customer Customer(string CustName)
        {
            try
            {
                return _context.Customers.Where(x => x.IsDeleted == false && x.Name == CustName).FirstOrDefault();
            }
            catch (Exception ex)
            {
                HelperCls.DebugLog(ex.Message + System.Environment.NewLine + ex.StackTrace);
                throw ex;
            }
        }

        public List<AllQuestion> allQuestions()
        {
            try
            {
                return _context.Database.SqlQuery<AllQuestion>("exec USP_GetAllQuestions").ToList();
            }
            catch (Exception ex)
            {
                HelperCls.DebugLog(ex.Message + System.Environment.NewLine + ex.StackTrace);
                throw ex;
            }
        }
        public List<NetSuitMapCls> GetNetSuiteCls()
        {
            try
            {
                return _context.netSuitMapCls.Where(x => x.IsDeleted == false).ToList();
            }
            catch (Exception ex)
            {
                HelperCls.DebugLog(ex.Message + System.Environment.NewLine + ex.StackTrace);
                throw ex;
            }
        }
        public Customer GetCustomerContextById(int Id)
        {
            try
            {
                StoreContext _Context = new StoreContext();
                return _Context.Customers.Where(x => x.Id == Id && x.IsDeleted == false).FirstOrDefault();
                //return _context.Database.SqlQuery<Customer>("exec USP_GetCustomerById @UserId",
                //  new SqlParameter("@CustId", CustId)).FirstOrDefault();
            }
            catch (Exception ex)
            {
                HelperCls.DebugLog(ex.Message + System.Environment.NewLine + ex.StackTrace);
                throw ex;
            }

        }
        public List<Answer> GetQuestionAnswersForUser(int id)
        {
            try
            {
                return _context.answers.Where(x => x.Users.Id == id).ToList();
            }
            catch (Exception ex)
            {
                HelperCls.DebugLog(ex.Message + System.Environment.NewLine + ex.StackTrace);
                throw ex;
            }
        }
        public List<Customer> GetCustomerforUserId(int custId, int userId)
        {
            try
            {
                var AllowedCustomer = _context.Permitables.Where(x => x.Users.Id == userId && x.Customer.Id == custId && x.Customer != null).Select(x => x.Customer).Distinct().ToList();
                var AllowedStoreCustomer = _context.Permitables.Where(x => x.Store != null && x.Users.Id == userId && x.Store.Customer.Id == custId).Select(x => x.Customer).Distinct().ToList();
                if (AllowedCustomer != null)
                    if (AllowedStoreCustomer != null && AllowedStoreCustomer.Count() > 0)
                        AllowedCustomer.AddRange(AllowedStoreCustomer);
                List<int> custIDs = AllowedCustomer.Select(x => x.Id).Distinct().ToList();
                return _context.Customers.Where(x => x.IsDeleted == false && custIDs.Contains(x.Id)).ToList();
            }
            catch (Exception ex)
            {
                HelperCls.DebugLog(ex.Message + System.Environment.NewLine + ex.StackTrace);
                throw ex;
            }
        }
        public List<EmailType> GetCustomerEmailTypes()
        {
            try
            {
                // return _context.Database.SqlQuery<EmailType>("exec USP_GetCustomerEmailTypes").ToList();
                return _context.EmailType.Where(x => x.IsDeleted == false).ToList();
            }
            catch (Exception ex)
            {
                HelperCls.DebugLog(ex.Message + System.Environment.NewLine + ex.StackTrace);
                throw ex;
            }
        }
        public List<CustomerEmailMaster> GetSelectedEmailsTypesForCustomer(int id)
        {
            try
            {
                return _context.Database.SqlQuery<CustomerEmailMaster>("exec USP_GetSelectedEmailsTypesForCustomer @CustId={0}", id).ToList();
            }
            catch (Exception ex)
            {
                HelperCls.DebugLog(ex.Message + System.Environment.NewLine + ex.StackTrace);
                throw ex;
            }
        }
        public void UpdateCustomerEmailTypes(Customer customers)
        {
            //try
            //{
            //    _context.Database.SqlQuery<CustomerEmailMaster>("exec USP_UpdateCustomerEmailTypes @CustId={0},@EmailType='{1}'", 
            //        customers.Id,
            //        String.Join(",",customers.EmailTypes.ToArray())
            //        ).FirstOrDefault();
            //}
            //catch (Exception ex)
            //{
            //    HelperCls.DebugLog(ex.Message + System.Environment.NewLine + ex.StackTrace);
            //    throw ex;
            //}

            int rowsAffected = 0;

            SqlParameter[] arParams = new SqlParameter[3];
            arParams[0] = new SqlParameter("@CustId", customers.Id);
            arParams[1] = new SqlParameter("@EmailType", customers.EmailTypes != null ? String.Join(",", customers.EmailTypes.ToArray()) : "");
            arParams[2] = new SqlParameter("@ModifiedBy", customers.ModifiedBy);
            DbCommand command = new SqlCommand()
            {
                CommandType = CommandType.StoredProcedure,
                CommandText = "USP_UpdateCustomerEmailTypes"
            };
            command.Connection = _context.Database.Connection;
            command.Parameters.AddRange(arParams);
            try
            {
                command.Connection.Open();
                command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                HelperCls.DebugLog(ex.Message + System.Environment.NewLine + ex.StackTrace);
            }
            finally
            {
                command.Connection.Close();
            }
        }

        public Customer GetAutoInventoryLogicFlag(int userId, int custId)
        {
            try
            {
                return _context.Database.SqlQuery<Customer>("exec USP_GetAutoInventoryLogicFlag @CustId={0},@UserId={1}", custId, userId).FirstOrDefault();
            }
            catch (Exception ex)
            {
                HelperCls.DebugLog(ex.Message + System.Environment.NewLine + ex.StackTrace);
                throw ex;
            }
        }
        public int AddUpdateStatus(int custId, string type, string status, int days, int userId)
        {
            try
            {
                return _context.Database.SqlQuery<int>("exec USP_AddUpdateStatus @CustId={0}, @UserId={1}, @Type='{2}', @Status='{3}', @Days={4}", custId, userId, type, status, days).FirstOrDefault();
            }
            catch (Exception ex)
            {
                HelperCls.DebugLog(ex.Message + System.Environment.NewLine + ex.StackTrace);
                return 0;
            }

        }
        public CustomerWiseStatusMst GetCustomerWiseStatusMst(int id, string type)
        {
            try
            {
                return _context.Database.SqlQuery<CustomerWiseStatusMst>("exec USP_GetCustomerWiseStatusMst @CustId={0}, @Type='{2}'", id, type).FirstOrDefault();
            }
            catch (Exception ex)
            {
                HelperCls.DebugLog(ex.Message + System.Environment.NewLine + ex.StackTrace);
                return null;
            }
        }
        public string InsertItemsBulk(List<ItemMasterUploadViewModel> lstOrderExcel, long fileId, string fileType)
        {
            string returnValue = null;
            DataTable dt = new DataTable();
            dt.Columns.AddRange(new DataColumn[16]
            {
                new DataColumn("Customer", typeof(string)),
                new DataColumn("SKUCode", typeof(string)),
                new DataColumn("SKUName", typeof(string)),
                new DataColumn("UOM", typeof(string)),
                new DataColumn("Category",typeof(string)),
                new DataColumn("SubCategory",typeof(string)),
                new DataColumn("ItemCategoryType",typeof(string)),
                new DataColumn("Brand",typeof(string)),
                new DataColumn("TotalWeightPerCase",typeof(string)),
                new DataColumn("MinimumOrder",typeof(string)),
                new DataColumn("MaximumOrderLimit",typeof(string)),
                new DataColumn("CaseConversion",typeof(string)),
                new DataColumn("Xls_Line_No",typeof(int)),
                new DataColumn("Upload_By",typeof(int)),
                new DataColumn("FileId",typeof(int)),
                new DataColumn("FileType",typeof(string))
            });

            foreach (ItemMasterUploadViewModel data in lstOrderExcel)
            {
                dt.Rows.Add(
                    data.Customer,
                    data.SKUCode,
                    data.SKUName,
                    data.UOM,
                    data.Category,
                    data.SubCategory,
                    data.ItemCategoryType,
                    data.Brand,
                    data.TotalWeightPerCase,
                    data.MinimumOrder,
                    data.MaximumOrderLimit,
                    data.CaseConversion,
                    data.Xls_Line_No,
                    data.Upload_By,
                    fileId,
                    fileType
                    );
            }
            if (dt.Rows.Count > 0)
            {
                string consString = _context.Database.Connection.ConnectionString;
                using (SqlConnection con = new SqlConnection(consString))
                {
                    using (SqlCommand cmd = new SqlCommand("USP_BulkInsert_ItemMaster"))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Connection = con;
                        cmd.Parameters.AddWithValue("@ItemData", dt);
                        con.Open();
                        var ScalarValue = cmd.ExecuteScalar();
                        if (ScalarValue != null)
                        {
                            returnValue = (string)ScalarValue;
                        }
                        con.Close();
                    }
                }
            }
            return returnValue;
        }

        public string InsertStoresBulk(List<StoreMasterUploadViewModel> lstOrderExcel, long fileId, string fileType)
        {
            string returnValue = null;
            DataTable dt = new DataTable();
            dt.Columns.AddRange(new DataColumn[17]
            {
                new DataColumn("Customer", typeof(string)),
                new DataColumn("StoreCode", typeof(string)),
                new DataColumn("StoreName", typeof(string)),
                new DataColumn("Address", typeof(string)),
                new DataColumn("EmailAddress",typeof(string)),
                new DataColumn("StoreManager",typeof(string)),
                new DataColumn("ContactNo",typeof(string)),
                new DataColumn("Location",typeof(string)),
                new DataColumn("Warehouse",typeof(string)),
                new DataColumn("PlaceOfSupply",typeof(string)),
                new DataColumn("NetsuiteClass",typeof(string)),
                new DataColumn("EnterpriseCode",typeof(string)),
                new DataColumn("EnterpriseName",typeof(string)),
                new DataColumn("Xls_Line_No",typeof(int)),
                new DataColumn("Upload_By",typeof(int)),
                new DataColumn("FileId",typeof(int)),
                new DataColumn("FileType",typeof(string))
            });

            foreach (StoreMasterUploadViewModel data in lstOrderExcel)
            {
                dt.Rows.Add(
                    data.Customer,
                    data.StoreCode,
                    data.StoreName,
                    data.Address,
                    data.EmailAddress,
                    data.StoreManager,
                    data.StoreContactNo,
                    data.Location,
                    data.WareHouse,
                    data.PlaceOfSupply,
                    data.NetSuitClass,
                    data.EnterpriseCode,
                    data.EnterpriseName,
                    data.Xls_Line_No,
                    data.Upload_By,
                    fileId,
                    fileType
                    );
            }
            if (dt.Rows.Count > 0)
            {
                string consString = _context.Database.Connection.ConnectionString;
                using (SqlConnection con = new SqlConnection(consString))
                {
                    using (SqlCommand cmd = new SqlCommand("USP_BulkInsert_StoreMaster"))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Connection = con;
                        cmd.Parameters.AddWithValue("@StoreData", dt);
                        con.Open();
                        var ScalarValue = cmd.ExecuteScalar();
                        if (ScalarValue != null)
                        {
                            returnValue = (string)ScalarValue;
                        }
                        con.Close();
                    }
                }
            }
            return returnValue;
        }

        public List<TicketDetails> GetTickets(int roleId, int userId, string orderNumber, List<string> ticketStatus,
            List<int> ticketTypeIds, DateTime? fromDate, DateTime? toDate, int storeId, int? loggedInCustId, List<int> selectedstoreIds)
        {
            List<TicketDetails> d = _context.Database.SqlQuery<TicketDetails>("exec USP_TicketDetails @roleid, @userId, @ordernumber," +
                "@ticketstatus,@tickettypeids, @storeid,@custid,@selectedstoreIds,@fromdate, @todate",
                   new SqlParameter("@roleid", roleId),
                   new SqlParameter("@userId", userId),
                   new SqlParameter("@ordernumber", orderNumber),
                   new SqlParameter("@fromdate", fromDate.HasValue ? fromDate.Value.ToString("yyyy-MM-dd") : ""),
                   new SqlParameter("@todate", toDate.HasValue ? toDate.Value.ToString("yyyy-MM-dd") : ""),
                   new SqlParameter("@ticketstatus", ticketStatus == null ? "" : string.Join(",", ticketStatus.ToArray())),
                   new SqlParameter("@tickettypeids", ticketTypeIds == null ? "" : string.Join(",", ticketTypeIds.ToArray())),
                   new SqlParameter("@storeid", storeId),
                   new SqlParameter("@custid", loggedInCustId),
                   new SqlParameter("@selectedstoreIds", selectedstoreIds == null ? "" : string.Join(",", selectedstoreIds.ToArray()))
                   ).ToList();
            List<TicketImages> images = _context.Database.SqlQuery<TicketImages>("exec USP_TicketImages @roleid, @userId, @ordernumber," +
                "@ticketstatus,@tickettypeids, @storeid,@custid,@selectedstoreIds,@fromdate, @todate",
                   new SqlParameter("@roleid", roleId),
                   new SqlParameter("@userId", userId),
                   new SqlParameter("@ordernumber", orderNumber),
                   new SqlParameter("@fromdate", fromDate.HasValue ? fromDate.Value.ToString("yyyy-MM-dd") : ""),
                   new SqlParameter("@todate", toDate.HasValue ? toDate.Value.ToString("yyyy-MM-dd") : ""),
                   new SqlParameter("@ticketstatus", ticketStatus == null ? "" : string.Join(",", ticketStatus.ToArray())),
                   new SqlParameter("@tickettypeids", ticketTypeIds == null ? "" : string.Join(",", ticketTypeIds.ToArray())),
                   new SqlParameter("@storeid", storeId),
                   new SqlParameter("@custid", loggedInCustId),
                   new SqlParameter("@selectedstoreIds", selectedstoreIds == null ? "" : string.Join(",", selectedstoreIds.ToArray()))
                   ).ToList();
            foreach (TicketDetails ticket in d)
            {
                ticket.TicketImages = images.Where(x => x.TicketId == ticket.TicketId).ToList();
            }
            return d;
        }
        public string SaveTicketFile(TicketImages t)
        {
            string consString = _context.Database.Connection.ConnectionString;
            using (SqlConnection con = new SqlConnection(consString))
            {
                string returnValue = "";
                using (SqlCommand cmd = new SqlCommand("usp_save_ticket_images"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Connection = con;
                    cmd.Parameters.AddWithValue("@TicketId", t.TicketId);
                    cmd.Parameters.AddWithValue("@ImageName", t.ImagePath);
                    cmd.Parameters.AddWithValue("@OriginalFileName", t.OriginalFileName);
                    cmd.Parameters.AddWithValue("@CreatedBy", t.CreatedBy);
                    cmd.Parameters.AddWithValue("@CreatedDate", t.CreatedDate);
                    con.Open();
                    var ScalarValue = cmd.ExecuteScalar();
                    if (ScalarValue != null)
                    {
                        returnValue = (string)ScalarValue;
                    }
                    con.Close();
                }
                return returnValue;
            }
        }
        public List<TicketImages> GetTicketImages(int TicketId)
        {
            return _context.Database.SqlQuery<TicketImages>("exec usp_getTicketImages @TicketId",
                   new SqlParameter("@TicketId", TicketId)
                   ).ToList();
        }
        public string DeleteTicketImage(int imageId, int ticketId, int userId)
        {
            string consString = _context.Database.Connection.ConnectionString;
            using (SqlConnection con = new SqlConnection(consString))
            {
                string returnValue = "";
                using (SqlCommand cmd = new SqlCommand("usp_delete_ticket_images"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Connection = con;
                    cmd.Parameters.AddWithValue("@TicketId", ticketId);
                    cmd.Parameters.AddWithValue("@imageId", imageId);                    
                    cmd.Parameters.AddWithValue("@LoggedinUser", userId);
                    con.Open();
                    var ScalarValue = cmd.ExecuteScalar();
                    if (ScalarValue != null)
                    {
                        returnValue = (string)ScalarValue;
                    }
                    con.Close();
                }
                return returnValue;
            }
        }
        public DataTable GetReportDatable(int roleId, int userId, string orderNumber, List<string> ticketStatus, List<int> ticketTypeIds, DateTime? fromDate, DateTime? toDate, int storeId, int? loggedInCustId, List<int> selectedstoreIds)
        {
            DataTable dt = new DataTable();
            string consString = _context.Database.Connection.ConnectionString;
            using (SqlConnection con = new SqlConnection(consString))
            {
                string returnValue = "";
                using (SqlCommand cmd = new SqlCommand("USP_TicketDetailsReport"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Connection = con;
                    cmd.Parameters.AddWithValue("@roleid", roleId);
                    cmd.Parameters.AddWithValue("@userId", userId);
                    cmd.Parameters.AddWithValue("@ordernumber", orderNumber);
                    cmd.Parameters.AddWithValue("@ticketstatus", ticketStatus == null ? "" : string.Join(",", ticketStatus.ToArray()));
                    cmd.Parameters.AddWithValue("@tickettypeids", ticketTypeIds == null ? "" : string.Join(",", ticketTypeIds.ToArray()));
                    cmd.Parameters.AddWithValue("@storeid", storeId);
                    cmd.Parameters.AddWithValue("@custid", loggedInCustId);
                    cmd.Parameters.AddWithValue("@selectedstoreIds", selectedstoreIds == null ? "" : string.Join(",", selectedstoreIds.ToArray()));
                    cmd.Parameters.AddWithValue("@fromdate", fromDate.HasValue ? fromDate.Value.ToString("yyyy-MM-dd") : "");
                    cmd.Parameters.AddWithValue("@todate", toDate.HasValue ? toDate.Value.ToString("yyyy-MM-dd") : "");
                    con.Open();
                    SqlDataAdapter da = new SqlDataAdapter(cmd);                    
                    da.Fill(dt);                    
                    con.Close();
                }                
            }
            return dt;
        }
    }
}
