using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DbLayer.Repository;
using DAL;
namespace DbLayer.Service
{
    public interface IStoreService
    {
        //Store GetUser(string Username, string Password);
        //Store GetUsersByEmail(string Email);
        int AddStore(Store store);
        int AddStore2(Store store,int warehouseid,int NetSuitMapCls);
        List<Store> GetAllStores();
        Store GetStoreById(int id);
        int EditStore(Store store);
        int DeleteStore(Store store);
        Store Login(string Username, string Password);
        Store GetStore(string Username);
        Users GetUsers(string Username);
        List<ProcessCls> Store();
        List<Store> GetStoresByCustomerId(int CustomerId);
        Store GetStoreByEmailAddress(Store store);
        WareHouseDC WareHouseDC(int id);
        NetSuitMapCls NetSuitMapCl(int id);
        StoreLogin StoreLogin(string UserName, string Password, string IpAddress);
        StoreLogin StoreLoginSessionExtensionDetails(string UserName, string IpAddress);
        Users GetUserById(int id);
        int EditUser(Users storeuser);
        List<StoreUser> GetUserByStoreId(int storeId);
        int ResetPasswordForUser(Users storeuser, int LoggedInUserId);
        long AddStoreExcel(StoreUserExcelImport store, int userid);
        long AddStoreUser(StoreUserExcelImport storeUser, int userid);
        List<StoreUser> GetUsersByStoreId(int StoreId);
        UserStore GetUserStore(int id);
        int DeleteStoreUser(int StoreUserId);
        List<Store> GetAvailableStoresforUserId(int custid, int userid);
        List<StoreView> GetStoresforUserCustomer(int userid, int custid,string OrderReport);
        List<CityView> GetCitiesforUserCustomer(int userid, int custid);
        List<StoreView> GetStoresForWareHouse(int userId, int custId, List<int> selectedWareHouseDCs, string OrderReport, List<int> Locations=null);
    }
    public class StoreService : IStoreService
    {
        private readonly IStoreRepository _storeRepository;
        public StoreService(IStoreRepository storeRepository)
        {
            _storeRepository = storeRepository;
        }
        public int AddStore(Store store)
        {
            return _storeRepository.AddStore(store);
        }
        public int AddStore2(Store store,int warehouseid,int NetSuitMapCls)
        {
            return _storeRepository.AddStore2(store, warehouseid, NetSuitMapCls);
        }
        public List<Store> GetAllStores()
        {
            return _storeRepository.GetAllStores();

        }
        public Store GetStoreById(int id)
        {
            return _storeRepository.GetStoreById(id);
        }
        public int EditStore(Store store)
        {
            return _storeRepository.EditStore(store);
        }
        public int DeleteStore(Store store)
        {
            return _storeRepository.DeleteStore(store);
        }

        public Store Login(string Username, string Password)
        {
            return _storeRepository.Login(Username, Password);
        }
        public Users GetUsers(string Username)
        {
            return _storeRepository.GetUsers(Username);
        }

        public Store GetStore(string Username)
        {
            return _storeRepository.GetStore(Username);
        }

        public List<ProcessCls> Store()
        {
            return _storeRepository.Store();
        }
        public List<Store> GetStoresByCustomerId(int CustomerId)
        {
            return _storeRepository.GetStoreByCustomerId(CustomerId);
        }
        public Store GetStoreByEmailAddress(Store store)
        {
            return _storeRepository.GetStoreByEmailAddress(store);
        }

        public WareHouseDC WareHouseDC(int id)
        {
            return _storeRepository.WareHouseDC(id);
        }

        public NetSuitMapCls NetSuitMapCl(int id)
        {
            return _storeRepository.NetSuitMapCl(id);
        }

        public StoreLogin StoreLogin(string UserName, string Password, string IpAddress)
        {
            return _storeRepository.StoreLogin(UserName, Password, IpAddress);
        }

        public StoreLogin StoreLoginSessionExtensionDetails(string UserName, string IpAddress)
        {
            return _storeRepository.StoreLoginSessionExtensionDetails(UserName, IpAddress);
        }
        public Users GetUserById(int id)
        {
            return _storeRepository.GetUserById(id);
        }
        public int EditUser(Users storeuser)
        {
            return _storeRepository.EditUser(storeuser);
        }
        public List<StoreUser> GetUserByStoreId(int storeId)
        {
            return _storeRepository.GetUserByStoreId(storeId);
        }
        public int ResetPasswordForUser(Users storeuser, int LoggedInUserId)
        {
            return _storeRepository.ResetPasswordForUser(storeuser, LoggedInUserId);
        }
        public long AddStoreExcel(StoreUserExcelImport store, int userid)
        {
            return _storeRepository.AddStoreExcel(store, userid);
        }
        public long AddStoreUser(StoreUserExcelImport storeUser, int userid)
        {
            return _storeRepository.AddStoreUserExcel(storeUser, userid);
        }
        public List<StoreUser> GetUsersByStoreId(int StoreId)
        {
            return _storeRepository.GetUserByStoreId(StoreId);
        }
        public UserStore GetUserStore(int id)
        {
            return _storeRepository.GetUserStore(id);
        }
        public int DeleteStoreUser(int StoreUserId)
        {
            return _storeRepository.DeleteStoreUser(StoreUserId);
        }
        public List<Store> GetAvailableStoresforUserId(int custid, int userid)
        {
            return _storeRepository.GetAvailableStoresforUserId(custid, userid);
        }
        public List<StoreView> GetStoresforUserCustomer(int userid, int custid,string OrderReport)
        {
            return _storeRepository.GetStoresforUserCustomer(userid,custid,OrderReport);
        }
        public List<CityView> GetCitiesforUserCustomer(int userid, int custid)
        {
            return _storeRepository.GetCitiesforUserCustomer(userid, custid);
        }
        public List<StoreView> GetStoresForWareHouse(int userId, int custId, List<int> selectedWareHouseDCs,string OrderReport, List<int> selectedLocations=null)
        {
            return _storeRepository.GetStoresForWareHouse(userId, custId,selectedWareHouseDCs,OrderReport, selectedLocations);
        }
    }
}
