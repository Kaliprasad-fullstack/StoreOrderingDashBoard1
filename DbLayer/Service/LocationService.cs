using DAL;
using DbLayer.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DbLayer.Service
{
    public interface ILocationService
    {
        int AddLocation(Locations store);
        List<Locations> GetAllLocations();
        Locations GetLocationById(int LocationId);
        int UpdateLocation(Locations locations);
        int DeleteLocation(int Id);
        List<Region> GetAllRegions();
        Region GetRegionById(int id);
    }
    public class LocationService : ILocationService
    {
        private readonly ILocationRepository _locationRepository;
        public LocationService(ILocationRepository locationRepository)
        {
            _locationRepository = locationRepository;
        }
        public int AddLocation(Locations location)
        {
            return _locationRepository.AddLocation(location);
        }

        public int DeleteLocation(int Id)
        {
            return _locationRepository.DeleteLocation(Id);
        }

        public List<Locations> GetAllLocations()
        {
            return _locationRepository.GetAllLocations();
        }

        public Locations GetLocationById(int LocationId)
        {
            return _locationRepository.GetLocationById(LocationId);
        }

        public int UpdateLocation(Locations locations)
        {
            return _locationRepository.UpdateLocation(locations);
        }
        public List<Region> GetAllRegions()
        {
            return _locationRepository.GetAllRegions();
        }
        public Region GetRegionById(int id)
        {
            return _locationRepository.GetRegionById(id);
        }
    }
}
