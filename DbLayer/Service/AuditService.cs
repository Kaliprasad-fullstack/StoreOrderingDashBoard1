using DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DbLayer.Repository;

namespace DbLayer.Service
{
    public interface IAuditService
    {
        Int64 InsertAudit(string IpAddress, string JsonData, string Link, int? UserId, int? StoreId);
    }
    public class AuditService : IAuditService
    {
        private readonly IAuditRepository _auditRepository;

        public AuditService(IAuditRepository auditRepository)
        {
            _auditRepository = auditRepository;
        }

        public long InsertAudit(string IpAddress, string JsonData, string Link, int? UserId, int? StoreId)
        {
            Audit audit = new Audit();
            audit.IpAddress = IpAddress;
            audit.JsonData = JsonData;
            audit.Link = Link;
            audit.Users = new Users();
            if (UserId != null)
                audit.UserId = (int)UserId;
            audit.Store = new Store();
            if (StoreId != null)
                audit.StoreId = (int)StoreId;
            return _auditRepository.InsertAudit(audit);
        }
    }
}
