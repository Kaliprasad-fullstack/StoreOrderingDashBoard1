using DAL;
using DbLayer.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace StoreOrderingDashBoard.Controllers
{
    public class UserController : Controller
    {
        // GET: User
        private readonly IUserService _userService;
        private readonly IAuditService _auditService;
        private readonly ICustomerService _customerService;
        private readonly ILocationService _locationService;
        private readonly IMenuService _menuService;
        private readonly IStoreService _storeService;

        public UserController(ICustomerService customerService, ILocationService locationService, IUserService userService, IMenuService menuService, IStoreService storeService, IAuditService auditService)
        {
            _customerService = customerService;
            _locationService = locationService;
            _userService = userService;
            _menuService = menuService;
            _storeService = storeService;
            _auditService = auditService;
        }

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult AddStoreUser()
        {
            List<Locations> locations = _locationService.GetAllLocations();
            List<Company> companies = _customerService.GetCompanies();
            List<CustomerPlan> plans = _customerService.GetCustomerPlans();
            List<String> Departments = _userService.GetDepartments();
            List<AllQuestion> allQuestions = _customerService.allQuestions();
            List<Customer> customers = _customerService.GetAllCustomers();
            //List<Role> Roles = _userService.GetRoles();
            Users users = new Users();
            users.RoleId = 2;
            List<SelectListItem> SelectLocations = locations.ConvertAll(a =>
            {
                return new SelectListItem()
                {
                    Text = a.Name,
                    Value = a.LocationId.ToString(),
                    Selected = false
                };
            });
            List<SelectListItem> SelectCustomers = customers.ConvertAll(a =>
            {
                return new SelectListItem()
                {
                    Text = a.Name,
                    Value = a.Id.ToString(),
                    Selected = false
                };
            });
            ViewBag.Locations = SelectLocations;
            ViewBag.Customers = SelectCustomers;
            ViewBag.Companies = companies;
            ViewBag.Plans = plans;
            ViewBag.Departments = Departments;
            ViewBag.Questions = allQuestions;
            users.AllQuestions = allQuestions;
            //users.
            users.Customers = customers;
            //ViewBag.Roles = Roles;
            return View(users);
        }
    }
}