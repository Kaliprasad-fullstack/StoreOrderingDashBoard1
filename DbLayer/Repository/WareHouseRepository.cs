using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BAL;
using DAL;

namespace DbLayer.Repository
{
    public interface IWareHouseRepository
    {
        List<WareHouseDC> wareHouseDCs();
        //List<TicketType2> TicketTypeMST();
        List<NetSuitMapCls> NetSuitMapCls();
        WareHouseDC WareHouseDC(int id);
        NetSuitMapCls NetSuitMapCl(int id);
        Company Company(int id);
        Locations Locations(int id);
        int InsertWareHouse(WareHouseDC wareHouseDC);
        int UpdateWareHouse(WareHouseDC wareHouseDC);
        int DeleteWareHouse(int id);
        int InsertNetsuit(NetSuitMapCls netSuitMapCls);
        int UpdateNetsuit(NetSuitMapCls netSuitMapCls);
        int DeleteNetsuit(int id);
        List<Region> RegionMst();
        List<StateMst> stateMst();
        List<string> GetWareHouse();
        List<WareHouseDCView> GetWareHouseForCustomerId(int userid, int custid, string OrderReport);
        List<WareHouseDCView2> GetWareHousesStoreItem(int loggedInUserId, int loggedInCustomer);
        List<WareHouseDCView> GetWareHouseForCustomerIdLocations(int userId, int custId, List<int> selectedLocations, int StoreId = 0);
    }
    public class WareHouseRepository : IWareHouseRepository
    {
        private readonly StoreContext _storeContext;
        public WareHouseRepository(StoreContext storeContext)
        {
            _storeContext = storeContext;
        }

        //public List<TicketType2> TicketTypeMST()
        //{
        //    try
        //    {
        //        return _storeContext.TicketTypeMST.ToList();
        //    }
        //    catch(Exception ex)
        //    {
        //        HelperCls.DebugLog(ex.Message + System.Environment.NewLine + ex.StackTrace);
        //        throw ex;
        //    }
        //}
        public List<WareHouseDC> wareHouseDCs()
        {
            try
            {
                return _storeContext.wareHouseDCs.Where(x => x.IsDeleted == false).ToList();
            }
            catch (Exception ex)
            {
                HelperCls.DebugLog(ex.Message + System.Environment.NewLine + ex.StackTrace);
                throw ex;
            }
        }

        public List<NetSuitMapCls> NetSuitMapCls()
        {
            try
            {
                return _storeContext.netSuitMapCls.Where(x=>x.IsDeleted==false).ToList();
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
                return _storeContext.wareHouseDCs.Find(id);
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
                return _storeContext.netSuitMapCls.Find(id);
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
                return _storeContext.Companies.Find(id);
            }
            catch (Exception ex)
            {
                HelperCls.DebugLog(ex.Message + System.Environment.NewLine + ex.StackTrace);
                throw ex;
            }
        }

        public Locations Locations(int id)
        {
            try
            {
                return _storeContext.Locations.Find(id);
            }
            catch (Exception ex)
            {
                HelperCls.DebugLog(ex.Message + System.Environment.NewLine + ex.StackTrace);
                throw ex;
            }
        }

        public int InsertWareHouse(WareHouseDC wareHouseDC)
        {
            try
            {
                _storeContext.wareHouseDCs.Add(wareHouseDC);
                return _storeContext.SaveChanges();
            }
            catch (Exception ex)
            {
                HelperCls.DebugLog(ex.Message + System.Environment.NewLine + ex.StackTrace);
                throw ex;
            }
        }

        public int UpdateWareHouse(WareHouseDC wareHouseDC)
        {
            try
            {
                wareHouseDC.Company = _storeContext.Companies.Find(wareHouseDC.Company.Id);
                _storeContext.Entry(wareHouseDC).State = EntityState.Modified;
                return _storeContext.SaveChanges();
              
            }
            catch (Exception ex)
            {
                HelperCls.DebugLog(ex.Message + System.Environment.NewLine + ex.StackTrace);
                throw ex;
            }
        }

        public int DeleteWareHouse(int id)
        {
            try
            {
                var war = _storeContext.wareHouseDCs.Find(id);
                war.IsDeleted = true;
                _storeContext.Entry(war).State = EntityState.Modified;
                return _storeContext.SaveChanges();
            }
            catch(Exception ex)
            {
                HelperCls.DebugLog(ex.Message + System.Environment.NewLine + ex.StackTrace);
                throw ex;
            }
        }

        public int InsertNetsuit(NetSuitMapCls netSuitMapCls)
        {
            try
            {
                _storeContext.netSuitMapCls.Add(netSuitMapCls);
                return _storeContext.SaveChanges();
            }
            catch (Exception ex)
            {
                HelperCls.DebugLog(ex.Message + System.Environment.NewLine + ex.StackTrace);
                throw ex;
            }
            
        }

        public int UpdateNetsuit(NetSuitMapCls netSuitMapCls)
        {
            try
            {
                //netSuitMapCls.Company = _storeContext.Companies.Find(wareHouseDC.Company.Id);
                _storeContext.Entry(netSuitMapCls).State = EntityState.Modified;
                return _storeContext.SaveChanges();

            }
            catch (Exception ex)
            {
                HelperCls.DebugLog(ex.Message + System.Environment.NewLine + ex.StackTrace);
                throw ex;
            }
        }

        public int DeleteNetsuit(int id)
        {
            try
            {
                var war = _storeContext.netSuitMapCls.Find(id);
                war.IsDeleted= true;
                _storeContext.Entry(war).State = EntityState.Modified;
                return _storeContext.SaveChanges();
            }
            catch (Exception ex)
            {
                HelperCls.DebugLog(ex.Message + System.Environment.NewLine + ex.StackTrace);
                throw ex;
            }
        }
        public List<Region> RegionMst()
        {
            try
            {
                return _storeContext.regions.Where(x => x.IsDeleted == false).ToList();
            }
            catch (Exception ex)
            {
                HelperCls.DebugLog(ex.Message + System.Environment.NewLine + ex.StackTrace);
                throw ex;
            }
        }

        public List<StateMst> stateMst()
        {
            try
            {
                return _storeContext.stateMsts.Where(x => x.IsDeleted == false).ToList();
            }
            catch (Exception ex)
            {
                HelperCls.DebugLog(ex.Message + System.Environment.NewLine + ex.StackTrace);
                throw ex;
            }
        }

        public List<string> GetWareHouse()
        {
            try
            {
                return _storeContext.Database.SqlQuery<string>("exec USP_GetWareHouse").ToList();
            }
            catch (Exception ex)
            {
                HelperCls.DebugLog(ex.Message + System.Environment.NewLine + ex.StackTrace);
                throw ex;
            }
        }
        public List<WareHouseDCView> GetWareHouseForCustomerId(int userid, int custid,string OrderReport)
        {
            try
            {
                return _storeContext.Database.SqlQuery<WareHouseDCView>(
                    string.Format("exec USP_GetWareHouseStoreForCustomer @UserId={0}, @CustId={1}, @CallType={2},  @OrderReport='{3}'", userid, custid,2, OrderReport)).ToList();
            }
            catch (Exception ex)
            {
                HelperCls.DebugLog(ex.Message + System.Environment.NewLine + ex.StackTrace);
                throw ex;
            }
        }
        public List<WareHouseDCView2> GetWareHousesStoreItem(int loggedInUserId, int loggedInCustomer)
        {
            try
            {
                return _storeContext.Database.SqlQuery<WareHouseDCView2>(
                    string.Format("exec USP_GetWareHousesForStoreItemCustomer @UserId={0}, @CustId={1}", loggedInUserId, loggedInCustomer)).ToList();
            }
            catch (Exception ex)
            {
                HelperCls.DebugLog(ex.Message + System.Environment.NewLine + ex.StackTrace);
                throw ex;
            }
        }
        public List<WareHouseDCView> GetWareHouseForCustomerIdLocations(int loggedInUserId, int loggedInCustomer, List<int> selectedLocations, int StoreId = 0)
        {
            try
            {
                return _storeContext.Database.SqlQuery<WareHouseDCView>(
                    string.Format("exec USP_GetWareHouseStoreForCustomer @UserId={0}, @CustId={1},@CallType={2},@Locations='{3}',@StoreId={4}", loggedInUserId, loggedInCustomer,10, string.Join(",", selectedLocations.ToArray()),StoreId)).ToList();
            }
            catch (Exception ex)
            {
                HelperCls.DebugLog(ex.Message + System.Environment.NewLine + ex.StackTrace);
                throw ex;
            }
        }
    }
}