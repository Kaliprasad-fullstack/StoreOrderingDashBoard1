using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL;
using BAL;
using System.Data.SqlClient;
using System.Data.Entity;

namespace DbLayer.Repository
{
    public interface IUserRepository
    {
        Users GetUser(string Username, string Password);
        Users GetUsersByEmail(string Email);
        Users GetUserByUserId(int id);
        List<string> GetDepartments();
        List<Role> GetRoles();
        int AddAdminUser(Users user);
        Users GetStoreUsersByEmail(string Email);
        int AddStoreUser(Users user);
        int EditStoreUser(Users user);
        List<DcAdmin> GetAdminUsers();
        int InsertAnswer(Answer answer);
        int InsertPermitable(PermitableCls permitableCls);
        Users GetAdminUserByUserId(int userId);
        List<ViewPermitableCls> VGetPermitablesForUserId(int userId);
        int EditAdminUser(Users user);
        int UpdateORInsertAnswer(Answer answer);
        int DeletePermissionsforUser(int id);
        int DeleteAdminUser(int userId);
        List<Answer> GetAnswersForUserId(int loggedInUserId);
        int ChangeAnswerForUserQuestions(AllQuestion question);
        int DisableAdminUser(string email, int Roleid);
        int logout(int UserId);
        int? GetCompanyAssigned(int userid);
    }
    public class UsersRepository : IUserRepository
    {
        private readonly StoreContext _context;
        public UsersRepository(StoreContext context)
        {
            _context = context;
        }
        public UsersRepository()
        {            
        }
        public Users GetUser(string Username, string Password)
        {
            try
            {
                return _context.Users.Where(x => x.EmailAddress == Username && x.Password == Password && x.RoleId == 2 && x.IsDeleted == false).FirstOrDefault();
                //return _context.Users.Where(x => x.EmailAddress == Username && x.Password == Password && x.IsDeleted == false).FirstOrDefault();
            }
            catch (Exception ex)
            {
                HelperCls.DebugLog(ex.Message + System.Environment.NewLine + ex.StackTrace);
                throw ex;
            }

        }
        public Users GetUserToken(string Username, string Password)
        {
            try
            {
                StoreContext _Context = new StoreContext();
                return _Context.Users.Where(x => x.EmailAddress == Username && x.Password == Password && x.IsDeleted == false).FirstOrDefault();
            }
            catch (Exception ex)
            {
                HelperCls.DebugLog(ex.Message + System.Environment.NewLine + ex.StackTrace);
                throw ex;
            }

        }

        public Users GetUsersByEmail(string Email)
        {
            try
            {
                return _context.Users.Where(x => x.EmailAddress == Email && x.RoleId == 2 && x.IsDeleted == false).FirstOrDefault();
            }
            catch (Exception ex)
            {
                HelperCls.DebugLog(ex.Message + System.Environment.NewLine + ex.StackTrace);
                throw ex;
            }
        }

        public Users GetUserByUserId(int id)
        {
            try
            {
                return _context.Users.Where(x => x.UserId == id).FirstOrDefault();
            }
            catch (Exception ex)
            {
                HelperCls.DebugLog(ex.Message + System.Environment.NewLine + ex.StackTrace);
                throw ex;
            }
        }

        public List<string> GetDepartments()
        {
            try
            {
                return _context.Users.Where(x => x.Department != "Store" && x.IsDeleted == false).Select(x => x.Department).Distinct().ToList();
            }
            catch (Exception ex)
            {
                HelperCls.DebugLog(ex.Message + System.Environment.NewLine + ex.StackTrace);
                throw ex;
            }
        }

        public List<Role> GetRoles()
        {
            try
            {
                return _context.roles.Where(x => x.IsDeleted == false).ToList();
            }
            catch (Exception ex)
            {
                HelperCls.DebugLog(ex.Message + System.Environment.NewLine + ex.StackTrace);
                throw ex;
            }
        }

        public int AddAdminUser(Users user)
        {
            try
            {

                return _context.Database.SqlQuery<int>("exec USP_AddAdminUser @Name,@Department,@EmailAddress,@Password,@IsDeleted,@LocationId,@RoleId,@CompanyId,@CustomerPlanId,@CreatedBy",
                    new SqlParameter("@Name", user.Name),
                    new SqlParameter("@Department", user.Department),
                    new SqlParameter("@EmailAddress", user.EmailAddress),
                    new SqlParameter("@Password", user.Password),
                    new SqlParameter("@IsDeleted", user.IsDeleted),
                    new SqlParameter("@LocationId", user.Locations.LocationId),
                    new SqlParameter("@RoleId", user.RoleId),
                    new SqlParameter("@CompanyId", user.Company.Id),
                    new SqlParameter("@CustomerPlanId", user.CustomerPlan.Id),
                    new SqlParameter("@CreatedBy", user.CreatedBy)).FirstOrDefault();
            }
            catch (Exception ex)
            {
                HelperCls.DebugLog(ex.Message + System.Environment.NewLine + ex.StackTrace);
                throw ex;
            }
        }

        public Users GetStoreUsersByEmail(string Email)
        {
            try
            {
                return _context.Users.Where(x => x.EmailAddress == Email && x.RoleId == 1 && x.IsDeleted == false).FirstOrDefault();
            }
            catch (Exception ex)
            {
                HelperCls.DebugLog(ex.Message + System.Environment.NewLine + ex.StackTrace);
                throw ex;
            }
        }

        public int AddStoreUser(Users user)
        {
            try
            {
                return _context.Database.SqlQuery<int>("exec USP_AddStoreUser @Name,@Department,@EmailAddress,@LocationId,@StoreId,@ChrMkrFlag,@CreatedBy",
                   new SqlParameter("@Name", user.Name),
                   new SqlParameter("@Department", user.Department),
                   new SqlParameter("@EmailAddress", user.EmailAddress),
                   new SqlParameter("@LocationId", user.Locations.LocationId),
                   new SqlParameter("@StoreId", user.StoreId),
                   new SqlParameter("@ChrMkrFlag", user.ChrMkrFlag),
                   new SqlParameter("@CreatedBy", user.CreatedBy)).FirstOrDefault();
            }
            catch (Exception ex)
            {
                HelperCls.DebugLog(ex.Message + System.Environment.NewLine + ex.StackTrace);
                throw ex;
            }

        }

        public int EditStoreUser(Users user)
        {
            try
            {
                return _context.Database.SqlQuery<int>("exec USP_EditStoreUser @Id, @Name,@Department,@EmailAddress,@Password,@LocationId,@StoreId,@ChrMkrFlag,@ModifiedBy",
                    new SqlParameter("@Id", user.Id),
                   new SqlParameter("@Name", user.Name),
                   new SqlParameter("@Department", user.Department),
                   new SqlParameter("@EmailAddress", user.EmailAddress),
                   new SqlParameter("@Password", user.Password),
                   new SqlParameter("@LocationId", user.Locations.LocationId),
                   new SqlParameter("@StoreId", user.StoreId),
                   new SqlParameter("@ChrMkrFlag", user.ChrMkrFlag),
                   new SqlParameter("@ModifiedBy", user.ModifiedBy)).FirstOrDefault();
            }
            catch (Exception ex)
            {
                HelperCls.DebugLog(ex.Message + System.Environment.NewLine + ex.StackTrace);
                throw ex;
            }

        }

        public List<DcAdmin> GetAdminUsers()
        {
            try
            {
                return _context.Database.SqlQuery<DcAdmin>("exec USP_GetAdminUser").ToList();
            }
            catch (Exception ex)
            {
                HelperCls.DebugLog(ex.Message + System.Environment.NewLine + ex.StackTrace);
                throw ex;
            }

        }

        public int InsertAnswer(Answer answer)
        {
            try
            {

                return _context.Database.SqlQuery<int>("exec USP_InsertAnswertbl @UsersId,@QuestionId,@Answer",
                    new SqlParameter("@UsersId", answer.Users.Id),
                    new SqlParameter("@QuestionId", answer.Id),
                    new SqlParameter("@Answer", answer.Answers)).FirstOrDefault();
            }
            catch (Exception ex)
            {
                HelperCls.DebugLog(ex.Message + System.Environment.NewLine + ex.StackTrace);
                throw ex;
            }
        }

        public int InsertPermitable(PermitableCls permitableCls)
        {
            try
            {

                return _context.Database.SqlQuery<int>("exec USP_InsertPermit @CreatedBy,@CustomerId,@UsersId,@StoreId",
                    new SqlParameter("@CreatedBy", permitableCls.Users.Id),
                    new SqlParameter("@CustomerId", permitableCls.Customer.Id),
                    new SqlParameter("@UsersId", permitableCls.Users.Id),
                    new SqlParameter("@StoreId", (object)permitableCls.StoreId ?? DBNull.Value)).FirstOrDefault();

            }
            catch (Exception ex)
            {
                HelperCls.DebugLog(ex.Message + System.Environment.NewLine + ex.StackTrace);
                throw ex;
            }
        }

        public Users GetAdminUserByUserId(int UserId)
        {
            try
            {
                return _context.Users.Where(x => x.IsDeleted == false && x.RoleId == 2 && x.Id == UserId).FirstOrDefault();
            }
            catch (Exception ex)
            {
                HelperCls.DebugLog(ex.Message + System.Environment.NewLine + ex.StackTrace);
                throw ex;
            }
        }

        public List<ViewPermitableCls> VGetPermitablesForUserId(int UserId)
        {
            try
            {
                return _context.Database.SqlQuery<ViewPermitableCls>("exec USP_GetPermitablesforUser @UserId", new SqlParameter("@UserId", UserId)).ToList();
            }
            catch (Exception ex)
            {
                HelperCls.DebugLog(ex.Message + System.Environment.NewLine + ex.StackTrace);
                throw ex;
            }
        }

        public int EditAdminUser(Users user)
        {
            try
            {
                return _context.Database.SqlQuery<int>("exec USP_EditAdminUser @UserId, @Name,@Department,@EmailAddress,@Password,@LocationId,@CompanyId,@CustomerPlanId,@ModifiedBy",
                    new SqlParameter("@UserId", user.Id),
                    new SqlParameter("@Name", user.Name),
                    new SqlParameter("@Department", user.Department),
                    new SqlParameter("@EmailAddress", user.EmailAddress),
                    new SqlParameter("@Password", user.Password),
                    new SqlParameter("@LocationId", user.Locations.LocationId),
                    new SqlParameter("@CompanyId", user.Company.Id),
                    new SqlParameter("@CustomerPlanId", user.CustomerPlan.Id),
                    new SqlParameter("@ModifiedBy", user.ModifiedBy)).FirstOrDefault();
            }
            catch (Exception ex)
            {
                HelperCls.DebugLog(ex.Message + System.Environment.NewLine + ex.StackTrace);
                throw ex;
            }
        }
        public int UpdateORInsertAnswer(Answer answer)
        {
            try
            {
                return _context.Database.SqlQuery<int>("exec USP_UpdateORInsertAnswertbl @UsersId, @QuestionId, @Answer",
                    new SqlParameter("@UsersId", answer.Users.Id),
                    new SqlParameter("@QuestionId", answer.Id),
                    new SqlParameter("@Answer", answer.Answers)).FirstOrDefault();
            }
            catch (Exception ex)
            {
                HelperCls.DebugLog(ex.Message + System.Environment.NewLine + ex.StackTrace);
                throw ex;
            }
        }
        public int DeletePermissionsforUser(int Id)
        {
            try
            {
                return _context.Database.SqlQuery<int>("exec USP_DeletePermissionsforUser @UsersId",
                    new SqlParameter("@UsersId", Id)).FirstOrDefault();
            }
            catch (Exception ex)
            {
                HelperCls.DebugLog(ex.Message + System.Environment.NewLine + ex.StackTrace);
                throw ex;
            }
        }
        public int DeleteAdminUser(int userId)
        {
            try
            {
                return _context.Database.SqlQuery<int>("exec USP_DeleteUser @UsersId",
                    new SqlParameter("@UsersId", userId)).FirstOrDefault();
            }
            catch (Exception ex)
            {
                HelperCls.DebugLog(ex.Message + System.Environment.NewLine + ex.StackTrace);
                throw ex;
            }
        }
        public List<Answer> GetAnswersForUserId(int loggedInUserId)
        {
            try
            {
                return _context.answers.Where(x => x.Users.Id == loggedInUserId).ToList();
            }
            catch (Exception ex)
            {
                HelperCls.DebugLog(ex.Message + System.Environment.NewLine + ex.StackTrace);
                throw ex;
            }
        }
        public int ChangeAnswerForUserQuestions(AllQuestion question)
        {
            try
            {
                return _context.Database.SqlQuery<int>("exec USP_ChangeAnswerForUserQuestions @UsersId, @QuestionId, @Answer",
                    new SqlParameter("@UsersId", question.UserId),
                    new SqlParameter("@QuestionId", question.Id),
                    new SqlParameter("@Answer", question.answer)).FirstOrDefault();
            }
            catch (Exception ex)
            {
                HelperCls.DebugLog(ex.Message + System.Environment.NewLine + ex.StackTrace);
                throw ex;
            }
        }

        public int DisableAdminUser(string email, int Roleid)
        {
            try
            {
                var user = _context.Users.Where(x => x.EmailAddress == email && x.IsDeleted == false && x.RoleId == Roleid).FirstOrDefault();
                user.IsDeleted = true;
                _context.Entry(user).State = EntityState.Modified;
                return _context.SaveChanges();
            }
            catch (Exception ex)
            {
                HelperCls.DebugLog(ex.Message + System.Environment.NewLine + ex.StackTrace);
                throw ex;
            }
        }

        public int logout(int UserId)
        {
            try
            {
                var user = _context.Users.Find(UserId);
                user.LogOutTime = DateTime.Now;
                user.IsLogin = false;
                _context.Entry(user).State = EntityState.Modified;
                return _context.SaveChanges();
            }
            catch (Exception ex)
            {
                HelperCls.DebugLog(ex.Message + System.Environment.NewLine + ex.StackTrace);
                throw ex;
            }
        }
        public int? GetCompanyAssigned(int userid)
        {
            try
            {
                return _context.Database.SqlQuery<int>("exec GetCompanyAssigned @UserId",
                    new SqlParameter("@UserId", userid)).FirstOrDefault();
            }
            catch (Exception ex)
            {
                HelperCls.DebugLog(ex.Message + System.Environment.NewLine + ex.StackTrace);
                throw ex;
            }
        }
    }
}
