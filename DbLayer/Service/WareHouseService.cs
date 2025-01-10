using DAL;
using System;
using System.Collections.Generic;
using DbLayer.Repository;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DbLayer.Service
{
    public interface IWareHouseService
    {

        List<WareHouseDC> wareHouseDCs();
        //List<TicketType2> TicketTypeMST();
        List<WareHouseDCView> GetWareHouseForCustomerId(int userid, int custid,string OrderReport); 
        List<NetSuitMapCls> NetSuitMapCls();
        WareHouseDC WareHouseDC(int id);
        NetSuitMapCls NetSuitMapCl(int id);
        Company Company(int id);
        Locations Locations(int id);
        int InsertWareHouse(WareHouseDC wareHouseDC);
        int DeleteWareHouse(int id);
        int UpdateWareHouse(WareHouseDC wareHouseDC);
        int InsertNetsuit(NetSuitMapCls netSuitMapCls);
        int UpdateNetsuit(NetSuitMapCls netSuitMapCls);
        int DeleteNetsuit(int id);
        List<Region> RegionMst();
        List<StateMst> stateMst();
        List<string> GetWareHouse();
        List<WareHouseDCView2> GetWareHousesStoreItem(int loggedInUserId, int loggedInCustomer);
        List<WareHouseDCView> GetWareHouseForCustomerIdLocations(int userId, int custId, List<int> selectedLocations,int StoreId=0);
    }
    public class WareHouseService : IWareHouseService
    {
        private readonly IWareHouseRepository _wareHouseRepository;

        public WareHouseService(IWareHouseRepository wareHouseRepository)
        {
            _wareHouseRepository = wareHouseRepository;
        }

        //public List<TicketType2> TicketTypeMST()
        //{
        //    return _wareHouseRepository.TicketTypeMST();
        //}

        public List<WareHouseDC> wareHouseDCs()
        {
            return _wareHouseRepository.wareHouseDCs();
        }

        public List<NetSuitMapCls> NetSuitMapCls()
        {
            return _wareHouseRepository.NetSuitMapCls();
        }

        public WareHouseDC WareHouseDC(int id)
        {
            return _wareHouseRepository.WareHouseDC(id);
        }

        public NetSuitMapCls NetSuitMapCl(int id)
        {
            return _wareHouseRepository.NetSuitMapCl(id);
        }

        public Company Company(int id)
        {
            return _wareHouseRepository.Company(id);
        }

        public Locations Locations(int id)
        {
            return _wareHouseRepository.Locations(id);
        }

        public int InsertWareHouse(WareHouseDC wareHouseDC)
        {
            return _wareHouseRepository.InsertWareHouse(wareHouseDC);
        }

        public int DeleteWareHouse(int id)
        {
            return _wareHouseRepository.DeleteWareHouse(id);
        }

        public int UpdateWareHouse(WareHouseDC wareHouseDC)
        {
            return _wareHouseRepository.UpdateWareHouse(wareHouseDC);
        }

        public int InsertNetsuit(NetSuitMapCls netSuitMapCls)
        {
            return _wareHouseRepository.InsertNetsuit(netSuitMapCls);
        }

        public int UpdateNetsuit(NetSuitMapCls netSuitMapCls)
        {
            return _wareHouseRepository.UpdateNetsuit(netSuitMapCls);
        }

        public int DeleteNetsuit(int id)
        {
            return _wareHouseRepository.DeleteNetsuit(id);
        }


        public List<Region> RegionMst()
        {
            return _wareHouseRepository.RegionMst();
        }

        public List<StateMst> stateMst()
        {
            return _wareHouseRepository.stateMst();
        }

        public List<string> GetWareHouse()
        {
            return _wareHouseRepository.GetWareHouse();
        }
        public List<WareHouseDCView> GetWareHouseForCustomerId(int userid, int custid, string OrderReport)
        {
            return _wareHouseRepository.GetWareHouseForCustomerId(userid,custid,OrderReport);
        }
        public List<WareHouseDCView2> GetWareHousesStoreItem(int loggedInUserId, int loggedInCustomer)
        {
            return _wareHouseRepository.GetWareHousesStoreItem(loggedInUserId, loggedInCustomer);
        }
        public List<WareHouseDCView> GetWareHouseForCustomerIdLocations(int userId, int custId, List<int> selectedLocations, int StoreId = 0)
        {
            return _wareHouseRepository.GetWareHouseForCustomerIdLocations(userId, custId, selectedLocations,StoreId);
        }
    }
}
