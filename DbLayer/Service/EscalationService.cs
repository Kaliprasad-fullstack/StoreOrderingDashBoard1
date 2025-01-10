using DAL;
using DbLayer.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DbLayer.Service
{
    public interface IEscalationService
    {
        List<EscalationLevelMaster> GetEscalationLevelMasters(int CustId);
        long AddEscalationLevelMaster(EscalationLevelMaster escalationLevelMaster);
        EscalationLevelMaster GetEscalationLevelMasterById(int Id);
        long EditEscalationLevelMaster(EscalationLevelMaster escalationLevelMaster);
        List<EscalationAction> GetEscalationActionsForCustomerLevelList(int loggedInCustomer, int levelID);
        long AddEscalationAction(EscalationAction action);
        EscalationAction GetEscalationAction(int id);
        List<CustomerEmployee> GetEmployeesForCustomer(int loggedInCustomer);
        long EditEscalationAction(EscalationAction master);
    }
    public class EscalationService : IEscalationService
    {
        private readonly IEscalationRepository _escalationRepository;
        public EscalationService(IEscalationRepository escalationRepository)
        {
            _escalationRepository = escalationRepository;
        }
        public long AddEscalationLevelMaster(EscalationLevelMaster escalationLevelMaster)
        {
            return _escalationRepository.AddEscalationLevelMaster(escalationLevelMaster);
        }

        public long EditEscalationLevelMaster(EscalationLevelMaster escalationLevelMaster)
        {
            return _escalationRepository.EditEscalationLevelMaster(escalationLevelMaster);
        }

        public EscalationLevelMaster GetEscalationLevelMasterById(int Id)
        {
            return _escalationRepository.GetEscalationLevelMasterById(Id);
        }

        public List<EscalationLevelMaster> GetEscalationLevelMasters(int CustId)
        {
            return _escalationRepository.GetEscalationLevelMasters(CustId);
        }
        public List<EscalationAction> GetEscalationActionsForCustomerLevelList(int loggedInCustomer, int levelID)
        {
            return _escalationRepository.GetEscalationActionsForCustomerLevelList(loggedInCustomer, levelID);
        }
        public List<CustomerEmployee> GetEmployeesForCustomer(int loggedInCustomer)
        {
            return _escalationRepository.GetEmployeesForCustomer(loggedInCustomer);
        }

        public long AddEscalationAction(EscalationAction action)
        {
            return _escalationRepository.AddEscalationAction(action);
        }

        public EscalationAction GetEscalationAction(int id)
        {
            return _escalationRepository.GetEscalationAction(id);
        }
        public long EditEscalationAction(EscalationAction master)
        {
            return _escalationRepository.EditEscalationAction(master);
        }
    }
}
