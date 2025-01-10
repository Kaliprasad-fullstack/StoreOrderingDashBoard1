using System.Web.Mvc;
using Unity;
using Unity.Mvc5;
using DbLayer.Service;
using DbLayer.Repository;

namespace StoreOrderingDashBoard
{
    public static class UnityConfig
    {
        public static void RegisterComponents()
        {
			var container = new UnityContainer();
            container.RegisterType<IAuditRepository,AuditRepository>();
            container.RegisterType<IAuditService,AuditService>();
            container.RegisterType<IWareHouseRepository, WareHouseRepository>();
            container.RegisterType<IWareHouseService, WareHouseService>();
            container.RegisterType<IMenuRepository, MenuRepository>();
            container.RegisterType<IMenuService, MenuService>();
            container.RegisterType<IOrderRepository, OrderRepository>();
            container.RegisterType<IOrderService, OrderService>();
            container.RegisterType<IItemRepository, ItemRepository>();
            container.RegisterType<IItemService, ItemService>();
            container.RegisterType<IUserRepository,UsersRepository>();
            container.RegisterType<IUserService, UserService>();
            container.RegisterType<ICustomerRepository, CustomerRepository>();
            container.RegisterType<ICustomerService, CustomerService>();
            container.RegisterType<IStoreRepository, StoreRepository>();
            container.RegisterType<IStoreService, StoreService>();
            container.RegisterType<IReportRepository, ReportRepository>();
            container.RegisterType<IReportService, ReportService>();
            container.RegisterType<ILocationRepository, LocationRepository>();
            container.RegisterType<ILocationService, LocationService>();
            container.RegisterType<IEscalationRepository, EscalationRepository>();
            container.RegisterType<IEscalationService, EscalationService>();
            container.RegisterType<IReportChartsRepository, ReportChartsRepository>();    
            container.RegisterType<IReportChartsServices, ReportChartsServices>();    
            DependencyResolver.SetResolver(new UnityDependencyResolver(container));
        }
    }
}