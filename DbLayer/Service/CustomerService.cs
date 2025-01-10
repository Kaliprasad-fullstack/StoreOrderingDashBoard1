using DAL;
using DbLayer.Repository;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DbLayer.Service
{
    public interface ICustomerService
    {
        List<Customer> GetAllCustomers();
        int AddCustomer(Customer customer);
        Customer GetCustomerById(int Id);
        int EditCustomer(Customer customer);
        ICollection<Customer> GetSelectedCustomers(IEnumerable<int> selectedCustomers);
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
        List<Answer> GetquestionAnswersForUser(int id);
        List<Customer> GetCustomerforUserId(int custId,int userId);
        List<EmailType> GetCustomerEmailTypes();
        List<CustomerEmailMaster> GetSelectedEmailsTypesForCustomer(int id);
        void UpdateCustomerEmailTypes(Customer customers);
        Customer GetAutoInventoryLogicFlag(int userId, int custId);
        int AddUpdateStatus(int CustId, string Type, string Status, int Days, int UserId);
        CustomerWiseStatusMst GetCustomerWiseStatusMst(int id, string Type);
        string InsertItemsBulk(List<ItemMasterUploadViewModel> lstOrderExcel, long fileId, string fileContentType);
        string InsertStoresBulk(List<StoreMasterUploadViewModel> lstOrderExcel, long fileId, string fileContentType);
        List<TicketDetails> GetTickets(int roleId, int userId, string orderNumber, List<string> ticketStatus, List<int> ticketTypeIds, DateTime? fromDate, DateTime? toDate, int storeId, int? loggedInCustId, List<int> selectedstoreIds);
        string SaveTicketFile(TicketImages t);
        List<TicketImages> GetTicketImages(int TicketId);
        string DeleteTicketImage(int imageId, int ticketId, int userId);
        DataTable GetReportDatable(int roleId, int userId, string orderNumber, List<string> ticketStatus, List<int> ticketTypeIds, DateTime? fromDate, DateTime? toDate, int storeId, int? loggedInCustId, List<int> selectedstoreIds);
    }
    public class CustomerService : ICustomerService
    {
        private readonly ICustomerRepository _customerRepository;
        public CustomerService(ICustomerRepository customerRepository)
        {
            _customerRepository = customerRepository;
        }
        public int AddCustomer(Customer customer)
        {
            return _customerRepository.AddCustomer(customer);
        }
        public List<Customer> GetAllCustomers()
        {
            return _customerRepository.GetAllCustomers();
        }
        public Customer GetCustomerById(int Id)
        {
            return _customerRepository.GetCustomerById(Id);
        }
        public int EditCustomer(Customer customer)
        {
            return _customerRepository.EditCustomer(customer);
        }
        public ICollection<Customer> GetSelectedCustomers(IEnumerable<int> selectedCustomers)
        {
            return _customerRepository.GetSelectedcustomers(selectedCustomers);
        }

        public List<CustomerList> customerLists(int UserId)
        {
            return _customerRepository.customerLists(UserId);
        }

        public Answer PasswordCustomer(string password, int userid, int QuestionId)
        {
            return _customerRepository.PasswordCustomer(password,userid,QuestionId);
        }

        public List<AllQuestion> GetQuestion(int UserId)
        {
            return _customerRepository.GetQuestion(UserId);
        }

        public List<Company> GetCompanies()
        {
            return _customerRepository.GetCompanies();
        }

        public List<CustomerPlan> GetCustomerPlans()
        {
            return _customerRepository.GetCustomerPlans();
        }

        public Company Company(int id)
        {
            return _customerRepository.Company(id);
        }

        public CustomerPlan CustomerPlan(int id)
        {
            return _customerRepository.CustomerPlan(id);
        }

        public Customer Customer(string CustName)
        {
            return _customerRepository.Customer(CustName);
        }

        public List<AllQuestion> allQuestions()
        {
            return _customerRepository.allQuestions();
        }
        public List<NetSuitMapCls> GetNetSuiteCls()
        {
            return _customerRepository.GetNetSuiteCls();
        }
        public List<Answer> GetquestionAnswersForUser(int id)
        {
            return _customerRepository.GetQuestionAnswersForUser(id);
        }

        public List<Customer> GetCustomerforUserId(int custId, int userId)
        {
            return _customerRepository.GetCustomerforUserId(custId, userId);
        }
        public List<EmailType> GetCustomerEmailTypes()
        {
            return _customerRepository.GetCustomerEmailTypes();
        }
        public List<CustomerEmailMaster> GetSelectedEmailsTypesForCustomer(int id)
        {
            return _customerRepository.GetSelectedEmailsTypesForCustomer(id);
        }
        public void UpdateCustomerEmailTypes(Customer customers)
        {
            _customerRepository.UpdateCustomerEmailTypes(customers);
        }
        public Customer GetAutoInventoryLogicFlag(int userId, int custId)
        {
            return _customerRepository.GetAutoInventoryLogicFlag(userId, custId);
        }
        public int AddUpdateStatus(int CustId, string Type, string Status, int Days, int UserId)
        {
            return _customerRepository.AddUpdateStatus(CustId,Type,Status,Days,UserId);
        }
        public CustomerWiseStatusMst GetCustomerWiseStatusMst(int id, string Type)
        {
            return _customerRepository.GetCustomerWiseStatusMst(id, Type);
        }
        public string InsertItemsBulk(List<ItemMasterUploadViewModel> lstOrderExcel, long fileId, string fileContentType)
        {
            return _customerRepository.InsertItemsBulk(lstOrderExcel, fileId, fileContentType);
        }
        public string InsertStoresBulk(List<StoreMasterUploadViewModel> lstOrderExcel, long fileId, string fileContentType)
        {
            return _customerRepository.InsertStoresBulk(lstOrderExcel, fileId, fileContentType);
        }
        public List<TicketDetails> GetTickets(int roleId, int userId, string orderNumber, List<string> ticketStatus, List<int> ticketTypeIds, DateTime? fromDate, DateTime? toDate, int storeId, int? loggedInCustId, List<int> selectedstoreIds)
        {
            return _customerRepository.GetTickets(roleId, userId, orderNumber, ticketStatus, ticketTypeIds, fromDate, toDate, storeId, loggedInCustId,selectedstoreIds);
        }
        public string SaveTicketFile(TicketImages t)
        {
            return _customerRepository.SaveTicketFile(t);
        }
        public List<TicketImages> GetTicketImages(int TicketId)
        {
            return _customerRepository.GetTicketImages(TicketId);
        }
        public string DeleteTicketImage(int imageId, int ticketId, int userId)
        {
            return _customerRepository.DeleteTicketImage(imageId,ticketId,userId);
        }
        public DataTable GetReportDatable(int roleId, int userId, string orderNumber, List<string> ticketStatus, List<int> ticketTypeIds, DateTime? fromDate, DateTime? toDate, int storeId, int? loggedInCustId, List<int> selectedstoreIds)
        {
            return _customerRepository.GetReportDatable(roleId, userId, orderNumber, ticketStatus, ticketTypeIds, fromDate, toDate, storeId, loggedInCustId, selectedstoreIds);
        }
    }
}
