using BAL;
using DAL;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DbLayer.Repository
{
    public interface ILocationRepository
    {
        int AddLocation(Locations store);
        List<Locations> GetAllLocations();
        Locations GetLocationById(int locationId);
        int UpdateLocation(Locations locations);
        int DeleteLocation(int Id);
        List<Region> GetAllRegions();
        Region GetRegionById(int id);
    }
    public class LocationRepository : ILocationRepository
    {
        private readonly StoreContext _context;
        public LocationRepository(StoreContext context)
        {
            _context = context;
        }

        public int AddLocation(Locations location)
        {
            try
            {
                _context.Locations.Add(location);
                int rowsaffected = _context.SaveChanges();
                return rowsaffected;
            }
            catch (Exception ex)
            {
                HelperCls.DebugLog(ex.Message + System.Environment.NewLine + ex.StackTrace);
                throw ex;
            }
        }

        public int DeleteLocation(int Id)
        {
            try
            {
                var loc = _context.Locations.Find(Id);
                loc.IsDeleted = true;
                _context.Entry(loc).State = EntityState.Modified;
                return _context.SaveChanges();
            }
            catch (Exception ex)
            {
                HelperCls.DebugLog(ex.Message + System.Environment.NewLine + ex.StackTrace);
                throw ex;
            }
        }

        public List<Locations> GetAllLocations()
        {
            try
            {
                return _context.Locations.Where(x => x.IsDeleted == false).ToList();
            }
            catch (Exception ex)
            {
                HelperCls.DebugLog(ex.Message + System.Environment.NewLine + ex.StackTrace);
                throw ex;
            }
        }
        public Locations GetLocationById(int locationId)
        {
            try
            {
                return _context.Locations.Where(x => x.IsDeleted == false && x.LocationId == locationId).First();
            }
            catch (Exception ex)
            {
                HelperCls.DebugLog(ex.Message + System.Environment.NewLine + ex.StackTrace);
                throw ex;
            }
        }

        public int UpdateLocation(Locations locations)
        {
            try
            {
                _context.Entry(locations).State = EntityState.Modified;
                return _context.SaveChanges();
            }
            catch (Exception ex)
            {
                HelperCls.DebugLog(ex.Message + System.Environment.NewLine + ex.StackTrace);
                throw ex;
            }
        }
        public List<Region> GetAllRegions()
        {
            try
            {
                return _context.regions.Where(x => x.IsDeleted == false).ToList();
            }
            catch (Exception ex)
            {
                HelperCls.DebugLog(ex.Message + System.Environment.NewLine + ex.StackTrace);
                throw ex;
            }
        }
        public Region GetRegionById(int id)
        {
            try
            {
                return _context.regions.Where(x => x.IsDeleted == false).FirstOrDefault();
            }
            catch (Exception ex)
            {
                HelperCls.DebugLog(ex.Message + System.Environment.NewLine + ex.StackTrace);
                throw ex;
            }
        }
    }
}
