using DAL;
using System;
using DbLayer.Repository;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DbLayer.Service
{
    public interface IUserService
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
        List<ViewPermitableCls> VGetPermitablesForUserId(int id);
        int DeletePermissionsforUser(int id);
        int EditAdminUser(Users user);
        int UpdateORInsertAnswer(Answer answer);
        int DeleteAdminUser(int userId);
        List<Answer> GetAnswersForUserId(int loggedInUserId);
        int ChangeAnswerForUserQuestions(AllQuestion question);
        int DisableAdminUser(string email, int Roleid);
        int logout(int UserId);
        int? GetCompanyAssigned(int userid);
    }
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }
        public Users GetUser(string Username, string Password)
        {
            return _userRepository.GetUser(Username, Password);
        }

        public Users GetUsersByEmail(string Email)
        {
            return _userRepository.GetUsersByEmail(Email);
        }
        public Users GetUserByUserId(int id)
        {
            return _userRepository.GetUserByUserId(id);
        }
        public List<string> GetDepartments()
        {
            return _userRepository.GetDepartments();
        }
        public List<Role> GetRoles()
        {
            return _userRepository.GetRoles();
        }
        public int AddAdminUser(Users user)
        {
            return _userRepository.AddAdminUser(user);
        }
        public Users GetStoreUsersByEmail(string Email)
        {
            return _userRepository.GetStoreUsersByEmail(Email);
        }
        public int AddStoreUser(Users user)
        {
            return _userRepository.AddStoreUser(user);
        }
        public int EditStoreUser(Users user)
        {
            return _userRepository.EditStoreUser(user);
        }
        public List<DcAdmin> GetAdminUsers()
        {
            return _userRepository.GetAdminUsers();
        }
        public int InsertAnswer(Answer answer)
        {
            return _userRepository.InsertAnswer(answer);
        }
        public int InsertPermitable(PermitableCls permitableCls)
        {
            return _userRepository.InsertPermitable(permitableCls);
        }
        public Users GetAdminUserByUserId(int UserId)
        {
            return _userRepository.GetAdminUserByUserId(UserId);
        }
        public List<ViewPermitableCls> VGetPermitablesForUserId(int UserId)
        {
            return _userRepository.VGetPermitablesForUserId(UserId);
        }
        public int EditAdminUser(Users user)
        {
            return _userRepository.EditAdminUser(user);
        }
        public int UpdateORInsertAnswer(Answer answer)
        {
            return _userRepository.UpdateORInsertAnswer(answer);
        }
        public int DeletePermissionsforUser(int id)
        {
            return _userRepository.DeletePermissionsforUser(id);
        }
        public int DeleteAdminUser(int userId)
        {
            return _userRepository.DeleteAdminUser(userId);
        }
        public List<Answer> GetAnswersForUserId(int loggedInUserId)
        {
            return _userRepository.GetAnswersForUserId(loggedInUserId);
        }
        public int ChangeAnswerForUserQuestions(AllQuestion question)
        {
            return _userRepository.ChangeAnswerForUserQuestions(question);
        }

        public int DisableAdminUser(string email,int Roleid)
        {
            return _userRepository.DisableAdminUser(email,Roleid);
        }

        public int logout(int UserId)
        {
            return _userRepository.logout(UserId);
        }
        public int? GetCompanyAssigned(int userid)
        {
            return _userRepository.GetCompanyAssigned(userid);
        }
    }
}
