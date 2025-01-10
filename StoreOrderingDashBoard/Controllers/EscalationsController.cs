using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DbLayer.Service;
using DAL;
using StoreOrderingDashBoard.Models;
using System.Web.Script.Serialization;
using BAL;
namespace StoreOrderingDashBoard.Controllers
{
    [AdminAuthorization]
    public class EscalationsController : Controller
    {
        private readonly IEscalationService _escalationService;
        private readonly IAuditService _auditService;
        private readonly IStoreService _storeService;

        public EscalationsController(IEscalationService escalationService, IAuditService auditService, IStoreService storeService)
        {
            _escalationService = escalationService;
            _auditService = auditService;
            _storeService = storeService;
        }
        // GET: Escalations
        public ActionResult Index()
        {
            var LoggedInCustomer = Convert.ToInt32(Session["CustId"]);
            List<EscalationLevelMaster> escalationLevels = _escalationService.GetEscalationLevelMasters(LoggedInCustomer);
            ViewBag.escalationLevels = escalationLevels;
            return View();
        }
        public ActionResult AddEscalationLevel()
        {
            var LoggedInCustomer = Convert.ToInt32(Session["CustId"]);
            EscalationLevelMaster master = new EscalationLevelMaster();
            var TriggerTypeList = new List<SelectListItem>()
            {
                new SelectListItem { Selected = false, Text = "Daily", Value = ((int)1).ToString()},
                new SelectListItem { Selected = false, Text = "Weekly", Value = ((int)2).ToString()}
            };
            ViewBag.TriggerTypeList = TriggerTypeList;
            var ProcessList = new List<SelectListItem>()
            {
                new SelectListItem { Selected = false, Text = "Process1", Value = ((int)1).ToString()},
                new SelectListItem { Selected = false, Text = "Process2", Value = ((int)2).ToString()}
            };
            ViewBag.ProcessList = ProcessList;
            return View(master);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddEscalationLevel(EscalationLevelMaster master)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var LoggedInCustomer = Convert.ToInt32(Session["CustId"]);
                    master.CustId = LoggedInCustomer;
                    master.CreatedBy = Convert.ToInt32(Session["UserId"]);
                    master.CreatedDate = DateTime.Now;
                    _escalationService.AddEscalationLevelMaster(master);
                    return RedirectToAction("Index", "Escalations");
                }
                else
                    return View(master);
            }
            catch (Exception ex)
            {
                HelperCls.DebugLog(ex.Message + System.Environment.NewLine + ex.StackTrace);
                return RedirectToAction("Error500", "Home");
            }

        }

        [HttpGet]
        public ActionResult EditEscalationLevel(int Id)
        {
            var LoggedInCustomer = Convert.ToInt32(Session["CustId"]);
            var CustEscalationLevel = _escalationService.GetEscalationLevelMasterById(Id);
            EscalationLevelMaster master = new EscalationLevelMaster();
            var TriggerTypeList = new List<SelectListItem>()
            {
                new SelectListItem { Selected = false, Text = "Daily", Value = ((int)1).ToString()},
                new SelectListItem { Selected = false, Text = "Weekly", Value = ((int)2).ToString()}
            };
            ViewBag.TriggerTypeList = TriggerTypeList;
            var ProcessList = new List<SelectListItem>()
            {
                new SelectListItem { Selected = false, Text = "Process1", Value = ((int)1).ToString()},
                new SelectListItem { Selected = false, Text = "Process2", Value = ((int)2).ToString()}
            };
            ViewBag.ProcessList = ProcessList;
            return View(CustEscalationLevel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditEscalationLevel(EscalationLevelMaster master)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var LoggedInCustomer = Convert.ToInt32(Session["CustId"]);
                    master.CustId = LoggedInCustomer;
                    master.ModifiedBy = Convert.ToInt32(Session["UserId"]);
                    master.ModifiedDate = DateTime.Now;
                    _escalationService.EditEscalationLevelMaster(master);
                    return RedirectToAction("Index", "Escalations");
                }
                else
                {
                    var LoggedInCustomer = Convert.ToInt32(Session["CustId"]);
                    var TriggerTypeList = new List<SelectListItem>()
            {
                new SelectListItem { Selected = false, Text = "Daily", Value = ((int)1).ToString()},
                new SelectListItem { Selected = false, Text = "Weekly", Value = ((int)2).ToString()}
            };
                    ViewBag.TriggerTypeList = TriggerTypeList;
                    var ProcessList = new List<SelectListItem>()
            {
                new SelectListItem { Selected = false, Text = "Process1", Value = ((int)1).ToString()},
                new SelectListItem { Selected = false, Text = "Process2", Value = ((int)2).ToString()}
            };
                    ViewBag.ProcessList = ProcessList;
                    return View(master);
                }

            }
            catch (Exception ex)
            {
                HelperCls.DebugLog(ex.Message + System.Environment.NewLine + ex.StackTrace);
                return RedirectToAction("Error500", "Home");
            }

        }

        public ActionResult EscalationActions(int LevelID)
        {
            var LoggedInCustomer = Convert.ToInt32(Session["CustId"]);
            var EscalationActionList = _escalationService.GetEscalationActionsForCustomerLevelList(LoggedInCustomer, LevelID);
            ViewBag.EscalationActionList = EscalationActionList;

            return View();
        }

        [HttpGet]
        public ActionResult AddEscalationAction()
        {
            var LoggedInCustomer = Convert.ToInt32(Session["CustId"]);
            EscalationAction CustEscalationAction = new EscalationAction();
            List<Store> CustomerStores = _storeService.GetStoresByCustomerId(LoggedInCustomer);
            List<CustomerEmployee> customerEmployees = _escalationService.GetEmployeesForCustomer(LoggedInCustomer);
            ViewBag.CustomerStores = CustomerStores;
            ViewBag.Employees = customerEmployees;
            var EmailToType = new List<SelectListItem>()
            {
                new SelectListItem { Selected = false, Text = "To", Value = ((int)1).ToString()},
                new SelectListItem { Selected = false, Text = "CC", Value = ((int)2).ToString()}
            };
            ViewBag.EmailToType = EmailToType;
            return View(CustEscalationAction);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddEscalationAction(EscalationAction action)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var LoggedInCustomer = Convert.ToInt32(Session["CustId"]);
                    //action.CustId = LoggedInCustomer;
                    action.CreatedBy = Convert.ToInt32(Session["UserId"]);
                    action.CreatedDate = DateTime.Now;
                    _escalationService.AddEscalationAction(action);
                    return RedirectToAction("EscalationActions", "Escalations", new { LevelID = action.Level });
                }
                else
                {
                    var LoggedInCustomer = Convert.ToInt32(Session["CustId"]);
                    List<Store> CustomerStores = _storeService.GetStoresByCustomerId(LoggedInCustomer);
                    List<CustomerEmployee> customerEmployees = _escalationService.GetEmployeesForCustomer(LoggedInCustomer);
                    ViewBag.CustomerStores = CustomerStores;
                    ViewBag.Employees = customerEmployees;
                    var EmailToType = new List<SelectListItem>()
            {
                new SelectListItem { Selected = false, Text = "To", Value = ((int)1).ToString()},
                new SelectListItem { Selected = false, Text = "CC", Value = ((int)2).ToString()}
            };
                    ViewBag.EmailToType = EmailToType;
                    return View(action);
                }

            }
            catch (Exception ex)
            {
                HelperCls.DebugLog(ex.Message + System.Environment.NewLine + ex.StackTrace);
                return RedirectToAction("Error500", "Home");
            }

        }

        [HttpGet]
        public ActionResult EditEscalationAction(int Id)
        {
            var LoggedInCustomer = Convert.ToInt32(Session["CustId"]);
            var CustEscalationAction = _escalationService.GetEscalationAction(Id);
            EscalationAction master = new EscalationAction();
            List<Store> CustomerStores = _storeService.GetStoresByCustomerId(LoggedInCustomer);
            List<CustomerEmployee> customerEmployees = _escalationService.GetEmployeesForCustomer(LoggedInCustomer);
            ViewBag.CustomerStores = CustomerStores;
            ViewBag.Employees = customerEmployees;
            var EmailToType = new List<SelectListItem>()
            {
                new SelectListItem { Selected = false, Text = "To", Value = ((int)1).ToString()},
                new SelectListItem { Selected = false, Text = "CC", Value = ((int)2).ToString()}
            };
            ViewBag.EmailToType = EmailToType;
            return View(CustEscalationAction);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditEscalationAction(EscalationAction master)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var LoggedInCustomer = Convert.ToInt32(Session["CustId"]);
                    master.ModifiedBy = Convert.ToInt32(Session["UserId"]);
                    master.ModifiedDate = DateTime.Now;
                    _escalationService.EditEscalationAction(master);
                    return RedirectToAction("EscalationActions", "Escalations", new { LevelID = master.Level });
                }
                else
                {
                    var LoggedInCustomer = Convert.ToInt32(Session["CustId"]);
                    List<Store> CustomerStores = _storeService.GetStoresByCustomerId(LoggedInCustomer);
                    List<CustomerEmployee> customerEmployees = _escalationService.GetEmployeesForCustomer(LoggedInCustomer);
                    ViewBag.CustomerStores = CustomerStores;
                    ViewBag.Employees = customerEmployees;
                    var EmailToType = new List<SelectListItem>()
            {
                new SelectListItem { Selected = false, Text = "To", Value = ((int)1).ToString()},
                new SelectListItem { Selected = false, Text = "CC", Value = ((int)2).ToString()}
            };
                    ViewBag.EmailToType = EmailToType;
                    return View(master);
                }

            }
            catch (Exception ex)
            {
                HelperCls.DebugLog(ex.Message + System.Environment.NewLine + ex.StackTrace);
                return RedirectToAction("Error500", "Home");
            }

        }

    }
}